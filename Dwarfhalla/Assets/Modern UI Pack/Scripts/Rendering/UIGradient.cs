﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Modern UI Pack/Effects/UI Gradient")]
    public class UIGradient : BaseMeshEffect
    {
        public enum Blend
        {
            Override,
            Add,
            Multiply
        }

        public enum Type
        {
            Horizontal,
            Vertical,
            Diamond
        }

        [SerializeField]
        Type _gradientType;

        [SerializeField]
        Blend _blendMode = Blend.Multiply;

        [SerializeField]
        bool _modifyVertices = true;

        [SerializeField]
        [Range(-1, 1)]
        float _offset = 0f;

        [SerializeField]
        [Range(0.1f, 10)]
        float _zoom = 1f;

        [SerializeField]
        Gradient _effectGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(Color.black, 0), new GradientColorKey(Color.white, 1)
            }
        };

        public Blend BlendMode
        {
            get { return _blendMode; }
            set
            {
                _blendMode = value;
                graphic.SetVerticesDirty();
            }
        }

        public Gradient EffectGradient
        {
            get { return _effectGradient; }
            set
            {
                _effectGradient = value;
                graphic.SetVerticesDirty();
            }
        }

        public Type GradientType
        {
            get { return _gradientType; }
            set
            {
                _gradientType = value;
                graphic.SetVerticesDirty();
            }
        }

        public bool ModifyVertices
        {
            get { return _modifyVertices; }
            set
            {
                _modifyVertices = value;
                graphic.SetVerticesDirty();
            }
        }

        public float Offset
        {
            get { return _offset; }
            set
            {
                _offset = Mathf.Clamp(value, -1f, 1f);
                graphic.SetVerticesDirty();
            }
        }

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = Mathf.Clamp(value, 0.1f, 10f);
                graphic.SetVerticesDirty();
            }
        }

        public override void ModifyMesh (VertexHelper helper)
        {
            if (!IsActive() || helper.currentVertCount == 0)
                return;

            List<UIVertex> _vertexList = new List<UIVertex>();
            helper.GetUIVertexStream(_vertexList);
            int nCount = _vertexList.Count;

            switch (GradientType)
            {
                case Type.Horizontal:
                case Type.Vertical:
                {
                    Rect bounds = GetBounds(_vertexList);
                    float min = bounds.xMin;
                    float w = bounds.width;
                    Func<UIVertex, float> GetPosition = v => v.position.x;

                    if (GradientType == Type.Vertical)
                    {
                        min = bounds.yMin;
                        w = bounds.height;
                        GetPosition = v => v.position.y;
                    }

                    float width = w == 0f ? 0f : 1f / w / Zoom;
                    float zoomOffset = (1 - (1 / Zoom)) * 0.5f;
                    float offset = (Offset * (1 - zoomOffset)) - zoomOffset;

                    if (ModifyVertices)
                        SplitTrianglesAtGradientStops(_vertexList, bounds, zoomOffset, helper);

                    UIVertex vertex = new UIVertex();
                    for (int i = 0; i < helper.currentVertCount; i++)
                    {
                        helper.PopulateUIVertex(ref vertex, i);
                        vertex.color = BlendColor(vertex.color,
                            EffectGradient.Evaluate((GetPosition(vertex) - min) * width - offset));
                        helper.SetUIVertex(vertex, i);
                    }
                }
                    break;

                case Type.Diamond:
                {
                    Rect bounds = GetBounds(_vertexList);
                    float height = bounds.height == 0f ? 0f : 1f / bounds.height / Zoom;
                    float radius = bounds.center.y / 2f;
                    Vector3 center = (Vector3.right + Vector3.up) * radius
                                     + Vector3.forward * _vertexList[0].position.z;

                    if (ModifyVertices)
                    {
                        helper.Clear();
                        for (int i = 0; i < nCount; i++)
                            helper.AddVert(_vertexList[i]);

                        UIVertex centralVertex = new UIVertex();
                        centralVertex.position = center;
                        centralVertex.normal = _vertexList[0].normal;
                        centralVertex.uv0 = new Vector2(0.5f, 0.5f);
                        centralVertex.color = Color.white;
                        helper.AddVert(centralVertex);

                        for (int i = 1; i < nCount; i++)
                            helper.AddTriangle(i - 1, i, nCount);
                        helper.AddTriangle(0, nCount - 1, nCount);
                    }

                    UIVertex vertex = new UIVertex();

                    for (int i = 0; i < helper.currentVertCount; i++)
                    {
                        helper.PopulateUIVertex(ref vertex, i);
                        vertex.color = BlendColor(vertex.color, EffectGradient.Evaluate(
                            Vector3.Distance(vertex.position, center) * height - Offset));
                        helper.SetUIVertex(vertex, i);
                    }
                }
                    break;
            }
        }

        Rect GetBounds (List<UIVertex> vertices)
        {
            float left = vertices[0].position.x;
            float right = left;
            float bottom = vertices[0].position.y;
            float top = bottom;

            for (int i = vertices.Count - 1; i >= 1; --i)
            {
                float x = vertices[i].position.x;
                float y = vertices[i].position.y;

                if (x > right)
                    right = x;
                else if (x < left)
                    left = x;

                if (y > top)
                    top = y;
                else if (y < bottom)
                    bottom = y;
            }

            return new Rect(left, bottom, right - left, top - bottom);
        }

        void SplitTrianglesAtGradientStops (List<UIVertex> _vertexList, Rect bounds, float zoomOffset,
            VertexHelper helper)
        {
            List<float> stops = FindStops(zoomOffset, bounds);
            if (stops.Count > 0)
            {
                helper.Clear();
                int nCount = _vertexList.Count;

                for (int i = 0; i < nCount; i += 3)
                {
                    float[] positions = GetPositions(_vertexList, i);
                    List<int> originIndices = new List<int>(3);
                    List<UIVertex> starts = new List<UIVertex>(3);
                    List<UIVertex> ends = new List<UIVertex>(2);

                    for (int s = 0; s < stops.Count; s++)
                    {
                        int initialCount = helper.currentVertCount;
                        bool hadEnds = ends.Count > 0;
                        bool earlyStart = false;

                        for (int p = 0; p < 3; p++)
                        {
                            if (!originIndices.Contains(p) && positions[p] < stops[s])
                            {
                                int p1 = (p + 1) % 3;
                                var start = _vertexList[p + i];

                                if (positions[p1] > stops[s])
                                {
                                    originIndices.Insert(0, p);
                                    starts.Insert(0, start);
                                    earlyStart = true;
                                }

                                else
                                {
                                    originIndices.Add(p);
                                    starts.Add(start);
                                }
                            }
                        }

                        if (originIndices.Count == 0)
                            continue;
                        if (originIndices.Count == 3)
                            break;

                        foreach (var start in starts)
                            helper.AddVert(start);

                        ends.Clear();
                        foreach (int index in originIndices)
                        {
                            int oppositeIndex = (index + 1) % 3;

                            if (positions[oppositeIndex] < stops[s])
                                oppositeIndex = (oppositeIndex + 1) % 3;
                            ends.Add(
                                CreateSplitVertex(_vertexList[index + i], _vertexList[oppositeIndex + i], stops[s]));
                        }

                        if (ends.Count == 1)
                        {
                            int oppositeIndex = (originIndices[0] + 2) % 3;
                            ends.Add(CreateSplitVertex(_vertexList[originIndices[0] + i],
                                _vertexList[oppositeIndex + i], stops[s]));
                        }

                        foreach (var end in ends)
                            helper.AddVert(end);

                        if (hadEnds)
                        {
                            helper.AddTriangle(initialCount - 2, initialCount, initialCount + 1);
                            helper.AddTriangle(initialCount - 2, initialCount + 1, initialCount - 1);

                            if (starts.Count > 0)
                            {
                                if (earlyStart)
                                    helper.AddTriangle(initialCount - 2, initialCount + 3, initialCount);
                                else
                                    helper.AddTriangle(initialCount + 1, initialCount + 3, initialCount - 1);
                            }
                        }

                        else
                        {
                            int vertexCount = helper.currentVertCount;
                            helper.AddTriangle(initialCount, vertexCount - 2, vertexCount - 1);

                            if (starts.Count > 1)
                                helper.AddTriangle(initialCount, vertexCount - 1, initialCount + 1);
                        }

                        starts.Clear();
                    }

                    if (ends.Count > 0)
                    {
                        if (starts.Count == 0)
                        {
                            for (int p = 0; p < 3; p++)
                            {
                                if (!originIndices.Contains(p) && positions[p] > stops[stops.Count - 1])
                                {
                                    int p1 = (p + 1) % 3;
                                    UIVertex end = _vertexList[p + i];

                                    if (positions[p1] > stops[stops.Count - 1])
                                        starts.Insert(0, end);
                                    else
                                        starts.Add(end);
                                }
                            }
                        }

                        foreach (var start in starts)
                            helper.AddVert(start);

                        int vertexCount = helper.currentVertCount;

                        if (starts.Count > 1)
                        {
                            helper.AddTriangle(vertexCount - 4, vertexCount - 2, vertexCount - 1);
                            helper.AddTriangle(vertexCount - 4, vertexCount - 1, vertexCount - 3);
                        }

                        else if (starts.Count > 0)
                            helper.AddTriangle(vertexCount - 3, vertexCount - 1, vertexCount - 2);
                    }

                    else
                    {
                        helper.AddVert(_vertexList[i]);
                        helper.AddVert(_vertexList[i + 1]);
                        helper.AddVert(_vertexList[i + 2]);
                        int vertexCount = helper.currentVertCount;
                        helper.AddTriangle(vertexCount - 3, vertexCount - 2, vertexCount - 1);
                    }
                }
            }
        }

        float[] GetPositions (List<UIVertex> _vertexList, int index)
        {
            float[] positions = new float[3];

            if (GradientType == Type.Horizontal)
            {
                positions[0] = _vertexList[index].position.x;
                positions[1] = _vertexList[index + 1].position.x;
                positions[2] = _vertexList[index + 2].position.x;
            }

            else
            {
                positions[0] = _vertexList[index].position.y;
                positions[1] = _vertexList[index + 1].position.y;
                positions[2] = _vertexList[index + 2].position.y;
            }

            return positions;
        }

        List<float> FindStops (float zoomOffset, Rect bounds)
        {
            List<float> stops = new List<float>();
            var offset = Offset * (1 - zoomOffset);
            var startBoundary = zoomOffset - offset;
            var endBoundary = (1 - zoomOffset) - offset;

            foreach (var color in EffectGradient.colorKeys)
            {
                if (color.time >= endBoundary)
                    break;

                if (color.time > startBoundary)
                    stops.Add((color.time - startBoundary) * Zoom);
            }

            foreach (var alpha in EffectGradient.alphaKeys)
            {
                if (alpha.time >= endBoundary)
                    break;

                if (alpha.time > startBoundary)
                    stops.Add((alpha.time - startBoundary) * Zoom);
            }

            float min = bounds.xMin;
            float size = bounds.width;

            if (GradientType == Type.Vertical)
            {
                min = bounds.yMin;
                size = bounds.height;
            }

            stops.Sort();

            for (int i = 0; i < stops.Count; i++)
            {
                stops[i] = (stops[i] * size) + min;

                if (i > 0 && Math.Abs(stops[i] - stops[i - 1]) < 2)
                {
                    stops.RemoveAt(i);
                    --i;
                }
            }

            return stops;
        }

        UIVertex CreateSplitVertex (UIVertex vertex1, UIVertex vertex2, float stop)
        {
            if (GradientType == Type.Horizontal)
            {
                float sx = vertex1.position.x - stop;
                float dx = vertex1.position.x - vertex2.position.x;
                float dy = vertex1.position.y - vertex2.position.y;
                float uvx = vertex1.uv0.x - vertex2.uv0.x;
                float uvy = vertex1.uv0.y - vertex2.uv0.y;
                float ratio = sx / dx;
                float splitY = vertex1.position.y - (dy * ratio);

                UIVertex splitVertex = new UIVertex();
                splitVertex.position = new Vector3(stop, splitY, vertex1.position.z);
                splitVertex.normal = vertex1.normal;
                splitVertex.uv0 = new Vector2(vertex1.uv0.x - (uvx * ratio), vertex1.uv0.y - (uvy * ratio));
                splitVertex.color = Color.white;
                return splitVertex;
            }

            else
            {
                float sy = vertex1.position.y - stop;
                float dy = vertex1.position.y - vertex2.position.y;
                float dx = vertex1.position.x - vertex2.position.x;
                float uvx = vertex1.uv0.x - vertex2.uv0.x;
                float uvy = vertex1.uv0.y - vertex2.uv0.y;
                float ratio = sy / dy;
                float splitX = vertex1.position.x - (dx * ratio);

                UIVertex splitVertex = new UIVertex();
                splitVertex.position = new Vector3(splitX, stop, vertex1.position.z);
                splitVertex.normal = vertex1.normal;
                splitVertex.uv0 = new Vector2(vertex1.uv0.x - (uvx * ratio), vertex1.uv0.y - (uvy * ratio));
                splitVertex.color = Color.white;
                return splitVertex;
            }
        }

        Color BlendColor (Color colorA, Color colorB)
        {
            switch (BlendMode)
            {
                default:
                    return colorB;

                case Blend.Add:
                    return colorA + colorB;

                case Blend.Multiply:
                    return colorA * colorB;
            }
        }
    }
}