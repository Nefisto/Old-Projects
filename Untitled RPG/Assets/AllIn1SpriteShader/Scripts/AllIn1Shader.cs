using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

namespace AllIn1SpriteShader
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [AddComponentMenu("AllIn1SpriteShader/AddAllIn1Shader")]
    public class AllIn1Shader : MonoBehaviour
    {
        public enum ShaderTypes
        {
            Default,
            ScaledTime,
            MaskedUI,
            Urp2dRenderer,
            Invalid
        }

        public ShaderTypes shaderTypes = ShaderTypes.Invalid;

        [Range(1f, 20f)]
        public float normalStrength = 5f;

        [Range(0f, 3f)]
        public int normalSmoothing = 1;

        [HideInInspector]
        public bool computingNormal;

        private Material currMaterial, prevMaterial;
        private bool matAssigned, destroyed;

        private void MakeNewMaterial (bool getShaderTypeFromPrefs, string shaderName = "AllIn1SpriteShader")
            => SetMaterial(AfterSetAction.Clear, getShaderTypeFromPrefs, shaderName);

        public void MakeCopy()
            => SetMaterial(AfterSetAction.CopyMaterial, false, GetStringFromShaderType());

        private void ResetAllProperties (bool getShaderTypeFromPrefs, string shaderName)
            => SetMaterial(AfterSetAction.Reset, getShaderTypeFromPrefs, shaderName);

        private string GetStringFromShaderType()
        {
            if (shaderTypes == ShaderTypes.Default)
                return "AllIn1SpriteShader";
            if (shaderTypes == ShaderTypes.ScaledTime)
                return "AllIn1SpriteShaderScaledTime";
            if (shaderTypes == ShaderTypes.MaskedUI)
                return "AllIn1SpriteShaderUiMask";
            if (shaderTypes == ShaderTypes.Urp2dRenderer)
                return "AllIn1Urp2dRenderer";
            return "AllIn1SpriteShader";
        }

        private void SetMaterial (AfterSetAction action, bool getShaderTypeFromPrefs, string shaderName)
        {
            var allIn1Shader = Resources.Load(shaderName, typeof(Shader)) as Shader;
            if (getShaderTypeFromPrefs)
            {
                var shaderVariant = PlayerPrefs.GetInt("allIn1DefaultShader");
                if (shaderVariant == 1)
                    allIn1Shader = Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader;
                else if (shaderVariant == 2)
                    allIn1Shader = Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader;
                else if (shaderVariant == 3)
                    allIn1Shader = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
            }

            if (!Application.isPlaying && Application.isEditor && allIn1Shader != null)
            {
                var rendererExists = false;
                var sr = GetComponent<Renderer>();
                if (sr != null)
                {
                    rendererExists = true;
                    var renderingQueue = 3000;
                    if (action == AfterSetAction.CopyMaterial)
                        renderingQueue = GetComponent<Renderer>().sharedMaterial.renderQueue;
                    prevMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
                    currMaterial = new Material(allIn1Shader);
                    currMaterial.renderQueue = renderingQueue;
                    GetComponent<Renderer>().sharedMaterial = currMaterial;
                    GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
                    matAssigned = true;
                    DoAfterSetAction(action);
                }
                else
                {
                    var img = GetComponent<Graphic>();
                    if (img != null)
                    {
                        rendererExists = true;
                        var renderingQueue = 3000;
                        if (action == AfterSetAction.CopyMaterial)
                            renderingQueue = img.material.renderQueue;
                        prevMaterial = new Material(img.material);
                        currMaterial = new Material(allIn1Shader);
                        currMaterial.renderQueue = renderingQueue;
                        img.material = currMaterial;
                        img.material.hideFlags = HideFlags.None;
                        matAssigned = true;
                        DoAfterSetAction(action);
                    }
                }

                if (!rendererExists)
                {
                    MissingRenderer();
                    return;
                }

                SetSceneDirty();
            }
            else if (allIn1Shader == null)
            {
                Debug.LogError("Make sure the AllIn1SpriteShader shader variants are inside the Resource folder!");
            }
        }

        private void DoAfterSetAction (AfterSetAction action)
        {
            switch (action)
            {
                case AfterSetAction.Clear:
                    ClearAllKeywords();
                    break;

                case AfterSetAction.CopyMaterial:
                    currMaterial.CopyPropertiesFromMaterial(prevMaterial);
                    break;
            }
        }

        public void TryCreateNew()
        {
            var rendererExists = false;
            var sr = GetComponent<Renderer>();
            if (sr != null)
            {
                rendererExists = true;
                if (sr != null && sr.sharedMaterial != null && sr.sharedMaterial.name.Contains("AllIn1"))
                {
                    ResetAllProperties(false, GetStringFromShaderType());
                    ClearAllKeywords();
                }
                else
                {
                    CleanMaterial();
                    MakeNewMaterial(false, GetStringFromShaderType());
                }
            }
            else
            {
                var img = GetComponent<Graphic>();
                if (img != null)
                {
                    rendererExists = true;
                    if (img.material.name.Contains("AllIn1"))
                    {
                        ResetAllProperties(false, GetStringFromShaderType());
                        ClearAllKeywords();
                    }
                    else
                    {
                        MakeNewMaterial(false, GetStringFromShaderType());
                    }
                }
            }

            if (!rendererExists)
                MissingRenderer();
            SetSceneDirty();
        }

        public void ClearAllKeywords()
        {
            SetKeyword("RECTSIZE_ON");
            SetKeyword("OFFSETUV_ON");
            SetKeyword("CLIPPING_ON");
            SetKeyword("POLARUV_ON");
            SetKeyword("TWISTUV_ON");
            SetKeyword("ROTATEUV_ON");
            SetKeyword("FISHEYE_ON");
            SetKeyword("PINCH_ON");
            SetKeyword("SHAKEUV_ON");
            SetKeyword("WAVEUV_ON");
            SetKeyword("ROUNDWAVEUV_ON");
            SetKeyword("DOODLE_ON");
            SetKeyword("ZOOMUV_ON");
            SetKeyword("FADE_ON");
            SetKeyword("TEXTURESCROLL_ON");
            SetKeyword("GLOW_ON");
            SetKeyword("OUTBASE_ON");
            SetKeyword("ONLYOUTLINE_ON");
            SetKeyword("OUTTEX_ON");
            SetKeyword("OUTDIST_ON");
            SetKeyword("DISTORT_ON");
            SetKeyword("WIND_ON");
            SetKeyword("GRADIENT_ON");
            SetKeyword("GRADIENT2COL_ON");
            SetKeyword("RADIALGRADIENT_ON");
            SetKeyword("COLORSWAP_ON");
            SetKeyword("HSV_ON");
            SetKeyword("HITEFFECT_ON");
            SetKeyword("PIXELATE_ON");
            SetKeyword("NEGATIVE_ON");
            SetKeyword("GRADIENTCOLORRAMP_ON");
            SetKeyword("COLORRAMP_ON");
            SetKeyword("GREYSCALE_ON");
            SetKeyword("POSTERIZE_ON");
            SetKeyword("BLUR_ON");
            SetKeyword("MOTIONBLUR_ON");
            SetKeyword("GHOST_ON");
            SetKeyword("ALPHAOUTLINE_ON");
            SetKeyword("INNEROUTLINE_ON");
            SetKeyword("ONLYINNEROUTLINE_ON");
            SetKeyword("HOLOGRAM_ON");
            SetKeyword("CHROMABERR_ON");
            SetKeyword("GLITCH_ON");
            SetKeyword("FLICKER_ON");
            SetKeyword("SHADOW_ON");
            SetKeyword("SHINE_ON");
            SetKeyword("CONTRAST_ON");
            SetKeyword("OVERLAY_ON");
            SetKeyword("OVERLAYMULT_ON");
            SetKeyword("ALPHACUTOFF_ON");
            SetKeyword("ALPHAROUND_ON");
            SetKeyword("CHANGECOLOR_ON");
            SetKeyword("CHANGECOLOR2_ON");
            SetKeyword("CHANGECOLOR3_ON");
            SetKeyword("FOG_ON");
            SetSceneDirty();
        }

        private void SetKeyword (string keyword, bool state = false)
        {
            if (destroyed)
                return;
            if (currMaterial == null)
            {
                FindCurrMaterial();
                if (currMaterial == null)
                {
                    MissingRenderer();
                    return;
                }
            }

            if (!state)
                currMaterial.DisableKeyword(keyword);
            else
                currMaterial.EnableKeyword(keyword);
        }

        private void FindCurrMaterial()
        {
            var sr = GetComponent<Renderer>();
            if (sr != null)
            {
                currMaterial = GetComponent<Renderer>().sharedMaterial;
                matAssigned = true;
            }
            else
            {
                var img = GetComponent<Graphic>();
                if (img != null)
                {
                    currMaterial = img.material;
                    matAssigned = true;
                }
            }
        }

        public void CleanMaterial()
        {
            var sr = GetComponent<Renderer>();
            if (sr != null)
            {
                sr.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
                matAssigned = false;
            }
            else
            {
                var img = GetComponent<Graphic>();
                if (img != null)
                {
                    img.material = new Material(Shader.Find("Sprites/Default"));
                    matAssigned = false;
                }
            }

            SetSceneDirty();
        }

        public void SaveMaterial()
        {
#if UNITY_EDITOR
            var sameMaterialPath = AllIn1ShaderWindow.materialsSavesPath;
            if (PlayerPrefs.HasKey("All1ShaderMaterials"))
                sameMaterialPath = PlayerPrefs.GetString("All1ShaderMaterials");
            else
                PlayerPrefs.SetString("All1ShaderMaterials", AllIn1ShaderWindow.materialsSavesPath);
            sameMaterialPath += "/";
            if (!Directory.Exists(sameMaterialPath))
            {
                EditorUtility.DisplayDialog("The desired Material Save Path doesn't exist",
                    "Go to Window -> AllIn1ShaderWindow and set a valid folder", "Ok");
                return;
            }

            sameMaterialPath += gameObject.name;
            var fullPath = sameMaterialPath + ".mat";
            if (File.Exists(fullPath))
                SaveMaterialWithOtherName(sameMaterialPath);
            else
                DoSaving(fullPath);
            SetSceneDirty();
#endif
        }

        private void SaveMaterialWithOtherName (string path, int i = 1)
        {
            var number = i;
            var newPath = path + "_" + number;
            var fullPath = newPath + ".mat";
            if (File.Exists(fullPath))
            {
                number++;
                SaveMaterialWithOtherName(path, number);
            }
            else
            {
                DoSaving(fullPath);
            }
        }

        private void DoSaving (string fileName)
        {
#if UNITY_EDITOR
            var rendererExists = false;
            var sr = GetComponent<Renderer>();
            Material matToSave = null;
            Material createdMat = null;
            if (sr != null)
            {
                rendererExists = true;
                matToSave = sr.sharedMaterial;
            }
            else
            {
                var img = GetComponent<Graphic>();
                if (img != null)
                {
                    rendererExists = true;
                    matToSave = img.material;
                }
            }

            if (!rendererExists)
            {
                MissingRenderer();
                return;
            }

            createdMat = new Material(matToSave);
            currMaterial = createdMat;
            AssetDatabase.CreateAsset(createdMat, fileName);
            Debug.Log(fileName + " has been saved!");
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(fileName, typeof(Material)));
            if (sr != null)
            {
                sr.material = createdMat;
            }
            else
            {
                var img = GetComponent<Graphic>();
                img.material = createdMat;
            }
#endif
        }

        public void SetSceneDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                EditorSceneManager.MarkAllScenesDirty();

            //If you get an error here please delete the 2 lines below
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
#endif
        }

        private void MissingRenderer()
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("Missing Renderer", "This GameObject (" +
                                                            gameObject.name + ") has no Renderer or UI Image component. This AllIn1Shader component will be removed.", "Ok");
            destroyed = true;
            DestroyImmediate(this);
#endif
        }

        public void ToggleSetAtlasUvs (bool activate)
        {
            var atlasUvs = GetComponent<SetAtlasUvs>();
            if (activate)
            {
                if (atlasUvs == null)
                    atlasUvs = gameObject.AddComponent<SetAtlasUvs>();
                atlasUvs.GetAndSetUVs();
                SetKeyword("ATLAS_ON", true);
            }
            else
            {
                if (atlasUvs != null)
                {
                    atlasUvs.ResetAtlasUvs();
                    DestroyImmediate(atlasUvs);
                }

                SetKeyword("ATLAS_ON");
            }

            SetSceneDirty();
        }

        public void ApplyMaterialToHierarchy()
        {
            var sr = GetComponent<Renderer>();
            var image = GetComponent<Graphic>();
            Material matToApply = null;
            if (sr != null)
            {
                matToApply = sr.sharedMaterial;
            }
            else if (image != null)
            {
                matToApply = image.material;
            }
            else
            {
                MissingRenderer();
                return;
            }

            var children = new List<Transform>();
            GetAllChildren(transform, ref children);
            foreach (var t in children)
            {
                sr = t.gameObject.GetComponent<Renderer>();
                if (sr != null)
                {
                    sr.material = matToApply;
                }
                else
                {
                    image = t.gameObject.GetComponent<Graphic>();
                    if (image != null)
                        image.material = matToApply;
                }
            }
        }

        public void CheckIfValidTarget()
        {
            var sr = GetComponent<Renderer>();
            var image = GetComponent<Graphic>();
            if (sr == null && image == null)
                MissingRenderer();
        }

        private void GetAllChildren (Transform parent, ref List<Transform> transforms)
        {
            foreach (Transform child in parent)
            {
                transforms.Add(child);
                GetAllChildren(child, ref transforms);
            }
        }

        public void RenderToImage()
        {
#if UNITY_EDITOR
            if (currMaterial == null)
            {
                FindCurrMaterial();
                if (currMaterial == null)
                {
                    MissingRenderer();
                    return;
                }
            }

            var tex = currMaterial.GetTexture("_MainTex");
            if (tex != null)
            {
                RenderAndSaveTexture(currMaterial, tex);
            }
            else
            {
                var sr = GetComponent<SpriteRenderer>();
                var i = GetComponent<Graphic>();
                if (sr != null)
                    tex = sr.sprite.texture;
                else if (i != null)
                    tex = i.mainTexture;

                if (tex != null)
                    RenderAndSaveTexture(currMaterial, tex);
                else
                    EditorUtility.DisplayDialog("No valid target texture found", "All In 1 Shader component couldn't find a valid Main Texture in this GameObject (" +
                                                                                 gameObject.name + "). This means that the material you are using has no Main Texture or that the texture couldn't be reached through the Renderer component you are using." +
                                                                                 " Please make sure to have a valid Main Texture in the Material", "Ok");
            }
#endif
        }

        public void RenderAndSaveTexture (Material targetMaterial, Texture targetTexture)
        {
#if UNITY_EDITOR
            float scaleSlider = 1;
            if (PlayerPrefs.HasKey("All1ShaderRenderImagesScale"))
                scaleSlider = PlayerPrefs.GetFloat("All1ShaderRenderImagesScale");
            var renderTarget = new RenderTexture((int)(targetTexture.width * scaleSlider), (int)(targetTexture.height * scaleSlider), 0, RenderTextureFormat.ARGB32);
            Graphics.Blit(targetTexture, renderTarget, targetMaterial);
            var reaultTex = new Texture2D(renderTarget.width, renderTarget.height, TextureFormat.ARGB32, false);
            reaultTex.ReadPixels(new Rect(0, 0, renderTarget.width, renderTarget.height), 0, 0);
            reaultTex.Apply();

            var path = AllIn1ShaderWindow.renderImagesSavesPath;
            if (PlayerPrefs.HasKey("All1ShaderRenderImages"))
                path = PlayerPrefs.GetString("All1ShaderRenderImages");
            else
                PlayerPrefs.SetString("All1ShaderRenderImages", AllIn1ShaderWindow.renderImagesSavesPath);
            path += "/";
            if (!Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("The desired Material to Image Save Path doesn't exist",
                    "Go to Window -> AllIn1ShaderWindow and set a valid folder", "Ok");
                return;
            }

            var fullPath = path + gameObject.name + ".png";
            if (File.Exists(fullPath))
                fullPath = GetNewValidPath(path + gameObject.name);
            var pingPath = fullPath;

            var fileName = fullPath.Replace(path, "");
            fileName = fileName.Replace(".png", "");
            fullPath = EditorUtility.SaveFilePanel("Save Render Image", path, fileName, "png");

            var bytes = reaultTex.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);
            AssetDatabase.ImportAsset(subPath);
            AssetDatabase.Refresh();
            DestroyImmediate(reaultTex);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(pingPath, typeof(Texture)));
            Debug.Log("Render Image saved to: " + fullPath + " with scale: " + scaleSlider + " (it can be changed in Window -> AllIn1ShaderWindow)");
#endif
        }

        private string GetNewValidPath (string path, int i = 1)
        {
            var number = i;
            var newPath = path + "_" + number;
            var fullPath = newPath + ".png";
            if (File.Exists(fullPath))
            {
                number++;
                fullPath = GetNewValidPath(path, number);
            }

            return fullPath;
        }

        private enum AfterSetAction
        {
            Clear,
            CopyMaterial,
            Reset
        }

#if UNITY_EDITOR
        private static float timeLastReload = -1f;

        private void Start()
        {
            if (timeLastReload < 0)
                timeLastReload = Time.time;
        }

        private void Update()
        {
            if (matAssigned || Application.isPlaying || !gameObject.activeSelf)
                return;
            var sr = GetComponent<Renderer>();
            if (sr != null)
            {
                if (sr.sharedMaterial == null)
                {
                    CleanMaterial();
                    MakeNewMaterial(true);
                }

                if (sr.sharedMaterial.name.Contains("Default"))
                    MakeNewMaterial(true);
                else
                    matAssigned = true;
            }
            else
            {
                var img = GetComponent<Graphic>();
                if (img != null)
                {
                    if (img.material.name.Contains("Default"))
                        MakeNewMaterial(true);
                    else
                        matAssigned = true;
                }
            }
        }
#endif

        #region normalMapCreator

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            EditorApplication.update += OnEditorUpdate;
#endif
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.update -= OnEditorUpdate;
#endif
        }

        private bool needToWait;
        private int waitingCycles;
        private int timesWeWaited;

        protected virtual void OnEditorUpdate()
        {
            if (computingNormal)
            {
                if (needToWait)
                {
                    waitingCycles++;
                    if (waitingCycles > 5)
                    {
                        needToWait = false;
                        timesWeWaited++;
                    }
                }
                else
                {
                    if (timesWeWaited == 1)
                        SetNewNormalTexture2();
                    if (timesWeWaited == 2)
                        SetNewNormalTexture3();
                    if (timesWeWaited == 3)
                        SetNewNormalTexture4();
                    needToWait = true;
                }
            }
        }

        private SpriteRenderer normalMapSr;
        private Renderer normalMapRenderer;
        private bool isSpriteRenderer;

        public void CreateAndAssignNormalMap()
        {
#if UNITY_EDITOR
            if (GetComponent<TilemapRenderer>() != null)
            {
                EditorUtility.DisplayDialog("This is a tilemap", "This feature isn't supported on Tilemap Renderers." +
                                                                 " Add a secondary normal map texture instead (you can create a Normal Map in the asset Window)", "Ok");
                return;
            }

            normalMapSr = GetComponent<SpriteRenderer>();
            normalMapRenderer = GetComponent<Renderer>();
            if (normalMapSr != null)
            {
                isSpriteRenderer = true;
                SetNewNormalTexture();
            }
            else if (normalMapRenderer != null)
            {
                isSpriteRenderer = false;
                SetNewNormalTexture();
            }
            else
            {
                if (GetComponent<Graphic>() != null)
                    EditorUtility.DisplayDialog("This is a UI element", "This GameObject (" +
                                                                        gameObject.name + ") is a UI element. UI elements probably shouldn't have a normal map. Why are you using the light shader variant?", "Ok");
                else
                    MissingRenderer();
            }
#endif
        }

        private string path;

        private void SetNewNormalTexture()
        {
#if UNITY_EDITOR
            path = AllIn1ShaderWindow.normalMapSavesPath;
            if (PlayerPrefs.HasKey("All1ShaderNormals"))
                path = PlayerPrefs.GetString("All1ShaderNormals");
            else
                PlayerPrefs.SetString("All1ShaderNormals", AllIn1ShaderWindow.normalMapSavesPath);
            path += "/";
            if (!Directory.Exists(path))
            {
                EditorUtility.DisplayDialog("The desired folder doesn't exist",
                    "Go to Window -> AllIn1ShaderWindow and set a valid folder", "Ok");
                return;
            }
#else
        computingNormal = false;
        return;
#endif

            computingNormal = true;
            needToWait = true;
            waitingCycles = 0;
            timesWeWaited = 0;
        }

#if UNITY_EDITOR
        private TextureImporter importer;
        private Texture2D mainTex2D;
#endif
        private void SetNewNormalTexture2()
        {
#if UNITY_EDITOR
            if (!isSpriteRenderer)
            {
                mainTex2D = (Texture2D)normalMapRenderer.sharedMaterial.GetTexture("_MainTex");
                importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(mainTex2D)) as TextureImporter;
            }
            else
            {
                importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(normalMapSr.sprite)) as TextureImporter;
            }

            importer.isReadable = true;
            importer.SaveAndReimport();
#endif
        }

        private string subPath;

        private void SetNewNormalTexture3()
        {
#if UNITY_EDITOR
            Texture2D normalM = null;
            if (isSpriteRenderer)
                normalM = CreateNormalMap(normalMapSr.sprite.texture, normalStrength, normalSmoothing);
            else
                normalM = CreateNormalMap(mainTex2D, normalStrength, normalSmoothing);

            var bytes = normalM.EncodeToPNG();

            path += gameObject.name;
            subPath = path + ".png";
            var dataPath = Application.dataPath;
            dataPath = dataPath.Replace("/Assets", "/");
            var fullPath = dataPath + subPath;

            File.WriteAllBytes(fullPath, bytes);
            AssetDatabase.ImportAsset(subPath);
            AssetDatabase.Refresh();
            DestroyImmediate(normalM);
#endif
        }

        private void SetNewNormalTexture4()
        {
#if UNITY_EDITOR
            importer = AssetImporter.GetAtPath(subPath) as TextureImporter;
            importer.filterMode = FilterMode.Bilinear;
            importer.textureType = TextureImporterType.NormalMap;
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.SaveAndReimport();

            if (currMaterial == null)
            {
                FindCurrMaterial();
                if (currMaterial == null)
                {
                    MissingRenderer();
                    return;
                }
            }

            var normalTex = (Texture2D)AssetDatabase.LoadAssetAtPath(subPath, typeof(Texture2D));
            currMaterial.SetTexture("_NormalMap", normalTex);

            Debug.Log("Normal texture saved to: " + subPath);
            EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(subPath, typeof(Texture)));

            computingNormal = false;
#endif
        }

        private Texture2D CreateNormalMap (Texture2D t, float normalMult = 5f, int normalSmooth = 0)
        {
            var pixels = new Color[t.width * t.height];
            var texNormal = new Texture2D(t.width, t.height, TextureFormat.RGB24, false, false);
            var vScale = new Vector3(0.3333f, 0.3333f, 0.3333f);

            for (var y = 0; y < t.height; y++)
            for (var x = 0; x < t.width; x++)
            {
                var tc = t.GetPixel(x - 1, y - 1);
                var cSampleNegXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x, y - 1);
                var cSampleZerXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y - 1);
                var cSamplePosXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 1, y);
                var cSampleNegXZerY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y);
                var cSamplePosXZerY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 1, y + 1);
                var cSampleNegXPosY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x, y + 1);
                var cSampleZerXPosY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y + 1);
                var cSamplePosXPosY = new Vector3(tc.r, tc.g, tc.g);
                var fSampleNegXNegY = Vector3.Dot(cSampleNegXNegY, vScale);
                var fSampleZerXNegY = Vector3.Dot(cSampleZerXNegY, vScale);
                var fSamplePosXNegY = Vector3.Dot(cSamplePosXNegY, vScale);
                var fSampleNegXZerY = Vector3.Dot(cSampleNegXZerY, vScale);
                var fSamplePosXZerY = Vector3.Dot(cSamplePosXZerY, vScale);
                var fSampleNegXPosY = Vector3.Dot(cSampleNegXPosY, vScale);
                var fSampleZerXPosY = Vector3.Dot(cSampleZerXPosY, vScale);
                var fSamplePosXPosY = Vector3.Dot(cSamplePosXPosY, vScale);
                var edgeX = (fSampleNegXNegY - fSamplePosXNegY) * 0.25f + (fSampleNegXZerY - fSamplePosXZerY) * 0.5f + (fSampleNegXPosY - fSamplePosXPosY) * 0.25f;
                var edgeY = (fSampleNegXNegY - fSampleNegXPosY) * 0.25f + (fSampleZerXNegY - fSampleZerXPosY) * 0.5f + (fSamplePosXNegY - fSamplePosXPosY) * 0.25f;
                var vEdge = new Vector2(edgeX, edgeY) * normalMult;
                var norm = new Vector3(vEdge.x, vEdge.y, 1.0f).normalized;
                var c = new Color(norm.x * 0.5f + 0.5f, norm.y * 0.5f + 0.5f, norm.z * 0.5f + 0.5f, 1);
                pixels[x + y * t.width] = c;
            }

            if (normalSmooth > 0f)
            {
                var step = 0.00390625f * normalSmooth;
                for (var y = 0; y < t.height; y++)
                for (var x = 0; x < t.width; x++)
                {
                    var pixelsToAverage = 0.0f;
                    var c = pixels[x + 0 + (y + 0) * t.width];
                    pixelsToAverage++;
                    if (x - normalSmooth > 0)
                    {
                        if (y - normalSmooth > 0)
                        {
                            c += pixels[x - normalSmooth + (y - normalSmooth) * t.width];
                            pixelsToAverage++;
                        }

                        c += pixels[x - normalSmooth + (y + 0) * t.width];
                        pixelsToAverage++;
                        if (y + normalSmooth < t.height)
                        {
                            c += pixels[x - normalSmooth + (y + normalSmooth) * t.width];
                            pixelsToAverage++;
                        }
                    }

                    if (y - normalSmooth > 0)
                    {
                        c += pixels[x + 0 + (y - normalSmooth) * t.width];
                        pixelsToAverage++;
                    }

                    if (y + normalSmooth < t.height)
                    {
                        c += pixels[x + 0 + (y + normalSmooth) * t.width];
                        pixelsToAverage++;
                    }

                    if (x + normalSmooth < t.width)
                    {
                        if (y - normalSmooth > 0)
                        {
                            c += pixels[x + normalSmooth + (y - normalSmooth) * t.width];
                            pixelsToAverage++;
                        }

                        c += pixels[x + normalSmooth + (y + 0) * t.width];
                        pixelsToAverage++;
                        if (y + normalSmooth < t.height)
                        {
                            c += pixels[x + normalSmooth + (y + normalSmooth) * t.width];
                            pixelsToAverage++;
                        }
                    }

                    pixels[x + y * t.width] = c / pixelsToAverage;
                }
            }

            texNormal.SetPixels(pixels);
            texNormal.Apply();
            return texNormal;
        }

        #endregion
    }
}