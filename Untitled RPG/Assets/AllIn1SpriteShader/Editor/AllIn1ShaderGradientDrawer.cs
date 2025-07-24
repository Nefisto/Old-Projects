#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AllIn1SpriteShader
{
    public class AllIn1ShaderGradientDrawer : MaterialPropertyDrawer
    {
        private readonly int resolution;
        private Texture2D textureAsset;

        public AllIn1ShaderGradientDrawer()
            => resolution = 64;

        public AllIn1ShaderGradientDrawer (float res)
            => resolution = (int)res;

        private static bool IsPropertyTypeSuitable (MaterialProperty prop)
            => prop.type == MaterialProperty.PropType.Texture;

        public string TextureName (MaterialProperty prop) => $"{prop.name}Tex";

        public override void OnGUI (Rect position, MaterialProperty prop, GUIContent label, MaterialEditor editor)
        {
            if (!IsPropertyTypeSuitable(prop))
            {
                EditorGUI.HelpBox(position, $"[Gradient] used on non-texture property \"{prop.name}\"", MessageType.Error);
                return;
            }

            if (!AssetDatabase.Contains(prop.targets.FirstOrDefault()))
            {
                EditorGUI.HelpBox(position, "Save Material To Folder to use this effect. Or use the regular Color Ramp instead", MessageType.Error);
                return;
            }

            var textureName = TextureName(prop);

            Gradient currentGradient = null;
            if (prop.targets.Length == 1)
            {
                var target = (Material)prop.targets[0];
                var path = AssetDatabase.GetAssetPath(target);
                textureAsset = GetTextureAsset(path, textureName);
                if (textureAsset != null)
                    currentGradient = DecodeGradient(prop, textureAsset.name);
                if (currentGradient == null)
                    currentGradient = new Gradient();

                EditorGUI.showMixedValue = false;
            }
            else
            {
                EditorGUI.showMixedValue = true;
            }

            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                currentGradient = EditorGUILayout.GradientField(label, currentGradient, GUILayout.Height(15));

                if (changeScope.changed)
                {
                    var encodedGradient = EncodeGradient(currentGradient);
                    var fullAssetName = textureName + encodedGradient;
                    foreach (var target in prop.targets)
                    {
                        if (!AssetDatabase.Contains(target))
                            continue;

                        var path = AssetDatabase.GetAssetPath(target);
                        var textureAsset = GetTexture(path, textureName);
                        Undo.RecordObject(textureAsset, "Change Material Gradient");
                        textureAsset.name = fullAssetName;
                        BakeGradient(currentGradient, textureAsset);
                        EditorUtility.SetDirty(textureAsset);

                        var material = (Material)target;
                        material.SetTexture(prop.name, textureAsset);
                    }
                }
            }

            EditorGUI.showMixedValue = false;
        }

        private Texture2D GetTexture (string path, string name)
        {
            textureAsset = GetTextureAsset(path, name);
            if (textureAsset == null)
                CreateTexture(path, name);
            if (textureAsset.width != resolution)
                textureAsset.Reinitialize(resolution, 1);
            return textureAsset;
        }

        private void CreateTexture (string path, string name = "unnamed texture")
        {
            textureAsset = new Texture2D(resolution, 1, TextureFormat.RGBA32, false);
            textureAsset.wrapMode = TextureWrapMode.Clamp;
            textureAsset.filterMode = FilterMode.Bilinear;
            textureAsset.name = name;
            AssetDatabase.AddObjectToAsset(textureAsset, path);
            AssetDatabase.Refresh();
        }

        private string EncodeGradient (Gradient gradient)
        {
            if (gradient == null)
                return null;
            return JsonUtility.ToJson(new GradientRepresentation(gradient));
        }

        private Gradient DecodeGradient (MaterialProperty prop, string name)
        {
            var json = name.Substring(TextureName(prop).Length);
            try
            {
                return JsonUtility.FromJson<GradientRepresentation>(json).ToGradient();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Texture2D GetTextureAsset (string path, string name)
            => AssetDatabase.LoadAllAssetsAtPath(path).FirstOrDefault(asset => asset.name.StartsWith(name)) as Texture2D;

        private void BakeGradient (Gradient gradient, Texture2D texture)
        {
            if (gradient == null)
                return;
            for (var x = 0; x < texture.width; x++)
            {
                var color = gradient.Evaluate((float)x / (texture.width - 1));
                for (var y = 0; y < texture.height; y++)
                    texture.SetPixel(x, y, color);
            }

            texture.Apply();
        }

        [MenuItem("Assets/AllIn1Shader/Remove All Gradient Textures")]
        private static void RemoveAllSubassets()
        {
            foreach (var asset in Selection.GetFiltered<Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.ImportAsset(path);
                foreach (var subAsset in AssetDatabase.LoadAllAssetRepresentationsAtPath(path))
                    Object.DestroyImmediate(subAsset, true);
                AssetDatabase.ImportAsset(path);
            }
        }

        private class GradientRepresentation
        {
            public AlphaKey[] alphaKeys;
            public ColorKey[] colorKeys;
            public GradientMode mode;

            public GradientRepresentation() { }

            public GradientRepresentation (Gradient source)
                => FromGradient(source);

            public void FromGradient (Gradient source)
            {
                mode = source.mode;
                colorKeys = source.colorKeys.Select(key => new ColorKey(key)).ToArray();
                alphaKeys = source.alphaKeys.Select(key => new AlphaKey(key)).ToArray();
            }

            public void ToGradient (Gradient gradient)
            {
                gradient.mode = mode;
                gradient.colorKeys = colorKeys.Select(key => key.ToGradientKey()).ToArray();
                gradient.alphaKeys = alphaKeys.Select(key => key.ToGradientKey()).ToArray();
            }

            public Gradient ToGradient()
            {
                var gradient = new Gradient();
                ToGradient(gradient);
                return gradient;
            }

            [Serializable]
            public struct ColorKey
            {
                public Color color;
                public float time;

                public ColorKey (GradientColorKey source)
                {
                    color = default;
                    time = default;
                    FromGradientKey(source);
                }

                public void FromGradientKey (GradientColorKey source)
                {
                    color = source.color;
                    time = source.time;
                }

                public GradientColorKey ToGradientKey()
                {
                    GradientColorKey key;
                    key.color = color;
                    key.time = time;
                    return key;
                }
            }

            [Serializable]
            public struct AlphaKey
            {
                public float alpha;
                public float time;

                public AlphaKey (GradientAlphaKey source)
                {
                    alpha = default;
                    time = default;
                    FromGradientKey(source);
                }

                public void FromGradientKey (GradientAlphaKey source)
                {
                    alpha = source.alpha;
                    time = source.time;
                }

                public GradientAlphaKey ToGradientKey()
                {
                    GradientAlphaKey key;
                    key.alpha = alpha;
                    key.time = time;
                    return key;
                }
            }
        }
    }
}
#endif