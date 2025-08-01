﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
    [CustomEditor(typeof(AllIn1Shader))]
    [CanEditMultipleObjects]
    public class AllIn1ShaderScriptEditor : Editor
    {
        private ImageType imageType;
        private SerializedProperty m_NormalStrength, m_NormalSmoothing;
        private bool showUrpWarning;
        private double warningTime;

        private void OnEnable()
        {
            m_NormalStrength = serializedObject.FindProperty("normalStrength");
            m_NormalSmoothing = serializedObject.FindProperty("normalSmoothing");
        }

        public override void OnInspectorGUI()
        {
            ChooseAndDiplayAssetImage();

            var myScript = (AllIn1Shader)target;

            SetCurrentShaderType(myScript);

            if (GUILayout.Button("Deactivate All Effects"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).ClearAllKeywords();


            if (GUILayout.Button("New Clean Material"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).TryCreateNew();


            if (GUILayout.Button("Create New Material With Same Properties (SEE DOC)"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).MakeCopy();

            if (GUILayout.Button("Save Material To Folder (SEE DOC)"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).SaveMaterial();

            if (GUILayout.Button("Apply Material To All Children"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).ApplyMaterialToHierarchy();

            if (myScript.shaderTypes != AllIn1Shader.ShaderTypes.Urp2dRenderer)
                if (GUILayout.Button("Render Material To Image"))
                    for (var i = 0; i < targets.Length; i++)
                        (targets[i] as AllIn1Shader).RenderToImage();

            var isUrp = false;
            var temp = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
            if (temp != null)
                isUrp = true;
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Change Shader Variant:", GUILayout.MaxWidth(140));
                var previousShaderType = (int)myScript.shaderTypes;
                myScript.shaderTypes = (AllIn1Shader.ShaderTypes)EditorGUILayout.EnumPopup(myScript.shaderTypes);
                if (previousShaderType != (int)myScript.shaderTypes)
                {
                    for (var i = 0; i < targets.Length; i++)
                        (targets[i] as AllIn1Shader).CheckIfValidTarget();
                    if (myScript == null)
                        return;
                    if (isUrp || myScript.shaderTypes != AllIn1Shader.ShaderTypes.Urp2dRenderer)
                    {
                        Debug.Log(myScript.gameObject.name + " shader variant has been changed to: " + myScript.shaderTypes);
                        myScript.SetSceneDirty();

                        var sr = myScript.GetComponent<Renderer>();
                        if (sr != null)
                        {
                            if (sr.sharedMaterial != null)
                            {
                                var renderingQueue = sr.sharedMaterial.renderQueue;
                                if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Default)
                                    sr.sharedMaterial.shader = Resources.Load("AllIn1SpriteShader", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.ScaledTime)
                                    sr.sharedMaterial.shader = Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.MaskedUI)
                                    sr.sharedMaterial.shader = Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
                                    sr.sharedMaterial.shader = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
                                else
                                    SetCurrentShaderType(myScript);
                                sr.sharedMaterial.renderQueue = renderingQueue;
                            }
                        }
                        else
                        {
                            var img = myScript.GetComponent<Graphic>();
                            if (img != null && img.material != null)
                            {
                                var renderingQueue = img.material.renderQueue;
                                if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Default)
                                    img.material.shader = Resources.Load("AllIn1SpriteShader", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.ScaledTime)
                                    img.material.shader = Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.MaskedUI)
                                    img.material.shader = Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader;
                                else if (myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
                                    img.material.shader = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
                                else
                                    SetCurrentShaderType(myScript);
                                img.material.renderQueue = renderingQueue;
                            }
                        }
                    }
                    else if (!isUrp && myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
                    {
                        myScript.shaderTypes = (AllIn1Shader.ShaderTypes)previousShaderType;
                        showUrpWarning = true;
                        warningTime = EditorApplication.timeSinceStartup + 5;
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            if (warningTime < EditorApplication.timeSinceStartup)
                showUrpWarning = false;
            if (isUrp)
                showUrpWarning = false;
            if (showUrpWarning)
                EditorGUILayout.HelpBox(
                    "You can't set the URP 2D Renderer variant since you didn't import the URP package available in the asset root folder (SEE DOCUMENTATION)",
                    MessageType.Error,
                    true);

            if (isUrp && myScript.shaderTypes == AllIn1Shader.ShaderTypes.Urp2dRenderer)
            {
                EditorGUILayout.Space();
                DrawLine(Color.grey, 1, 3);
                EditorGUILayout.Space();

                if (GUILayout.Button("Create And Add Normal Map"))
                    for (var i = 0; i < targets.Length; i++)
                        (targets[i] as AllIn1Shader).CreateAndAssignNormalMap();
                serializedObject.Update();
                EditorGUILayout.PropertyField(m_NormalStrength, new GUIContent("Normal Strength"), GUILayout.Height(20));
                EditorGUILayout.PropertyField(m_NormalSmoothing, new GUIContent("Normal Blur"), GUILayout.Height(20));
                if (myScript.computingNormal)
                    EditorGUILayout.LabelField("Normal Map is currently being created, be patient", EditorStyles.boldLabel, GUILayout.Height(40));
                serializedObject.ApplyModifiedProperties();

                EditorGUILayout.Space();
            }

            DrawLine(Color.grey, 1, 3);
            EditorGUILayout.Space();

            if (GUILayout.Button("Sprite Atlas Auto Setup"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).ToggleSetAtlasUvs(true);
            if (GUILayout.Button("Remove Sprite Atlas Configuration"))
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).ToggleSetAtlasUvs(false);

            EditorGUILayout.Space();
            DrawLine(Color.grey, 1, 3);

            if (GUILayout.Button("Remove Component"))
                for (var i = targets.Length - 1; i >= 0; i--)
                {
                    DestroyImmediate(targets[i] as AllIn1Shader);
                    (targets[i] as AllIn1Shader).SetSceneDirty();
                }

            if (GUILayout.Button("REMOVE COMPONENT AND MATERIAL"))
            {
                for (var i = 0; i < targets.Length; i++)
                    (targets[i] as AllIn1Shader).CleanMaterial();
                for (var i = targets.Length - 1; i >= 0; i--)
                    DestroyImmediate(targets[i] as AllIn1Shader);
            }
        }

        private void ChooseAndDiplayAssetImage()
        {
            if (!EditorPrefs.HasKey("allIn1ImageConfig"))
                EditorPrefs.SetInt("allIn1ImageConfig", (int)ImageType.ShowImage);

            imageType = (ImageType)EditorPrefs.GetInt("allIn1ImageConfig");
            Texture2D imageInspector = null;
            switch (imageType)
            {
                case ImageType.ShowImage:
                {
                    imageInspector =
                        (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/AllIn1SpriteShader/Textures/CustomEditorImage.png",
                            typeof(Texture2D));
                    break;
                }

                case ImageType.HideInComponent:
                    imageInspector =
                        (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/AllIn1SpriteShader/Textures/CustomEditorImage.png",
                            typeof(Texture2D));
                    break;
            }

            if (imageInspector && imageType != ImageType.HideInComponent && imageType != ImageType.HideEverywhere && imageInspector)
            {
                var rect = EditorGUILayout.GetControlRect(GUILayout.Height(40));
                GUI.DrawTexture(rect, imageInspector, ScaleMode.ScaleToFit, true);
            }
        }

        private void SetCurrentShaderType (AllIn1Shader myScript)
        {
            var shaderName = "";
            var sr = myScript.GetComponent<Renderer>();
            if (sr != null)
            {
                shaderName = sr.sharedMaterial.shader.name;
            }
            else
            {
                var img = myScript.GetComponent<Graphic>();
                if (img != null)
                    shaderName = img.material.shader.name;
            }

            shaderName = shaderName.Replace("AllIn1SpriteShader/", "");

            if (shaderName.Equals("AllIn1SpriteShader"))
                myScript.shaderTypes = AllIn1Shader.ShaderTypes.Default;
            else if (shaderName.Equals("AllIn1SpriteShaderScaledTime"))
                myScript.shaderTypes = AllIn1Shader.ShaderTypes.ScaledTime;
            else if (shaderName.Equals("AllIn1SpriteShaderUiMask"))
                myScript.shaderTypes = AllIn1Shader.ShaderTypes.MaskedUI;
            else if (shaderName.Equals("AllIn1Urp2dRenderer"))
                myScript.shaderTypes = AllIn1Shader.ShaderTypes.Urp2dRenderer;
        }

        private void DrawLine (Color color, int thickness = 2, int padding = 10)
        {
            var r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }

        private enum ImageType
        {
            ShowImage,
            HideInComponent,
            HideEverywhere
        }
    }
}
#endif