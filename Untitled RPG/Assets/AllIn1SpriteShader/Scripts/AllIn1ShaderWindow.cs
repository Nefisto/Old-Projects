#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AllIn1SpriteShader
{
    public class AllIn1ShaderWindow : EditorWindow
    {
        private const string versionString = "3.4";
        private const int bigFontSize = 16;

        public static readonly string materialsSavesPath = "Assets/AllIn1SpriteShader/Materials";
        public static readonly string renderImagesSavesPath = "Assets/AllIn1SpriteShader/Textures";
        public static readonly string normalMapSavesPath = "Assets/AllIn1SpriteShader/Textures/NormalMaps";
        public static readonly string gradientSavesPath = "Assets/AllIn1SpriteShader/Textures/GradientTextures";

        public Vector2 scrollPosition = Vector2.zero;

        [SerializeField]
        private Gradient gradient = new();

        private readonly GUIStyle titleStyle = new();

        private FilterMode gradientFiltering = FilterMode.Bilinear;

        private ImageType imageType;
        private int isComputingNormals;

        private DefaultAsset materialTargetFolder;
        private int normalSmoothing = 1;
        private float normalStrength = 5f;

        private ShaderTypes shaderTypes = ShaderTypes.Default;
        private bool showUrpWarning;
        private GUIStyle style, bigLabel = new();

        private Texture2D targetNormalImage;

        private TextureSizes textureSizes = TextureSizes._128;
        private double warningTime;

        private void OnGUI()
        {
            style = new GUIStyle(EditorStyles.helpBox);
            style.margin = new RectOffset(0, 0, 0, 0);
            bigLabel = new GUIStyle(EditorStyles.boldLabel);
            bigLabel.fontSize = bigFontSize;
            titleStyle.alignment = TextAnchor.MiddleLeft;

            using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height)))
            {
                scrollPosition = scrollView.scrollPosition;

                ShowImageAndSetImageEditorPref();

                ShowAssetImageOptionsToggle();

                DefaultAssetShader();

                DrawLine(Color.grey, 1, 3);
                GUILayout.Label("Material Save Path", bigLabel);
                GUILayout.Space(20);
                GUILayout.Label("Select the folder where new Materials will be saved when the Save Material To Folder button of the asset component is pressed", EditorStyles.boldLabel);
                HandleSaveFolderEditorPref("All1ShaderMaterials", materialsSavesPath, "Material");

                DrawLine(Color.grey, 1, 3);
                GUILayout.Label("Render Material to Image Save Path", bigLabel);
                GUILayout.Space(20);
                EditorGUILayout.BeginHorizontal();
                {
                    float scaleSlider = 1;
                    if (PlayerPrefs.HasKey("All1ShaderRenderImagesScale"))
                        scaleSlider = PlayerPrefs.GetFloat("All1ShaderRenderImagesScale");
                    GUILayout.Label("Rendered Image Texture Scale", GUILayout.MaxWidth(190));
                    scaleSlider = EditorGUILayout.Slider(scaleSlider, 0.2f, 5f, GUILayout.MaxWidth(200));
                    if (GUILayout.Button("Default Value", GUILayout.MaxWidth(100)))
                        PlayerPrefs.SetFloat("All1ShaderRenderImagesScale", 1f);
                    else
                        PlayerPrefs.SetFloat("All1ShaderRenderImagesScale", scaleSlider);
                }
                EditorGUILayout.EndVertical();
                GUILayout.Label("Select the folder where new Images will be saved when the Render Material To Image button of the asset component is pressed", EditorStyles.boldLabel);
                HandleSaveFolderEditorPref("All1ShaderRenderImages", renderImagesSavesPath, "Images");

                DrawLine(Color.grey, 1, 3);
                NormalMapCreator();

                DrawLine(Color.grey, 1, 3);
                GradientCreator();

                GUILayout.Space(10);
                DrawLine(Color.grey, 1, 3);
                GUILayout.Label("Current asset version is " + versionString, EditorStyles.boldLabel);
            }
        }

        [MenuItem("Window/AllIn1ShaderWindow")]
        public static void ShowAllIn1ShaderWindowWindow()
            => GetWindow<AllIn1ShaderWindow>("All In 1 Shader Window");

        private void ShowImageAndSetImageEditorPref()
        {
            if (!EditorPrefs.HasKey("allIn1ImageConfig"))
                EditorPrefs.SetInt("allIn1ImageConfig", (int)ImageType.ShowImage);

            imageType = (ImageType)EditorPrefs.GetInt("allIn1ImageConfig");
            if (imageType == ImageType.HideEverywhere)
                return;
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

            if (imageInspector)
            {
                //Label title image to the right
                var rect = EditorGUILayout.GetControlRect(false, 5, titleStyle);
                GUILayout.Label(imageInspector, titleStyle, GUILayout.Height(50));

                //Centered title image
                //Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(50));
                //GUI.DrawTexture(rect, imageInspector, ScaleMode.ScaleToFit, true);
            }

            DrawLine(Color.grey, 1, 3);
        }

        private void ShowAssetImageOptionsToggle()
        {
            GUILayout.Label("Asset Image Display Options", bigLabel);
            GUILayout.Space(20);

            var previousImageType = (int)imageType;
            imageType = (ImageType)EditorGUILayout.EnumPopup(imageType, GUILayout.MaxWidth(200));
            if ((int)imageType != previousImageType)
                EditorPrefs.SetInt("allIn1ImageConfig", (int)imageType);

            DrawLine(Color.grey, 1, 3);
        }

        private void DefaultAssetShader()
        {
            GUILayout.Label("Default Asset Shader", bigLabel);
            GUILayout.Space(20);
            GUILayout.Label("This is the shader variant that will be assinged by default to Sprites and UI Images when the asset component is added", EditorStyles.boldLabel);

            var isUrp = false;
            var temp = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
            if (temp != null)
                isUrp = true;

            shaderTypes = (ShaderTypes)PlayerPrefs.GetInt("allIn1DefaultShader");
            var previousShaderType = (int)shaderTypes;
            shaderTypes = (ShaderTypes)EditorGUILayout.EnumPopup(shaderTypes, GUILayout.MaxWidth(200));

            if (previousShaderType != (int)shaderTypes)
            {
                if (!isUrp && shaderTypes == ShaderTypes.Urp2dRenderer)
                {
                    showUrpWarning = true;
                    warningTime = EditorApplication.timeSinceStartup + 5;
                }
                else
                {
                    PlayerPrefs.SetInt("allIn1DefaultShader", (int)shaderTypes);
                    showUrpWarning = false;
                }
            }

            if (warningTime < EditorApplication.timeSinceStartup)
                showUrpWarning = false;
            if (isUrp)
                showUrpWarning = false;
            if (!isUrp && !showUrpWarning && shaderTypes == ShaderTypes.Urp2dRenderer)
            {
                showUrpWarning = true;
                warningTime = EditorApplication.timeSinceStartup + 5;
                shaderTypes = ShaderTypes.Default;
                PlayerPrefs.SetInt("allIn1DefaultShader", (int)shaderTypes);
            }

            if (showUrpWarning)
                EditorGUILayout.HelpBox(
                    "You can't set the URP 2D Renderer variant since you didn't import the URP package available in the asset root folder (SEE DOCUMENTATION)",
                    MessageType.Error,
                    true);
        }

        private void NormalMapCreator()
        {
            GUILayout.Label("Normal Map Creator", bigLabel);

            GUILayout.Space(20);
            GUILayout.Label("Select the folder where new Normal Maps will be saved when the Create Normal Map button of the asset component is pressed (URP only)", EditorStyles.boldLabel);
            HandleSaveFolderEditorPref("All1ShaderNormals", normalMapSavesPath, "Normal Maps");

            GUILayout.Space(20);
            GUILayout.Label("Assign a sprite you want to create a normal map from. Choose the normal map settings and press the 'Create And Save Normal Map' button", EditorStyles.boldLabel);
            targetNormalImage = (Texture2D)EditorGUILayout.ObjectField("Target Image", targetNormalImage, typeof(Texture2D), false, GUILayout.MaxWidth(225));

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Normal Strength:", GUILayout.MaxWidth(150));
                normalStrength = EditorGUILayout.Slider(normalStrength, 1f, 20f, GUILayout.MaxWidth(400));
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Normal Smoothing:", GUILayout.MaxWidth(150));
                normalSmoothing = EditorGUILayout.IntSlider(normalSmoothing, 0, 3, GUILayout.MaxWidth(400));
            }
            EditorGUILayout.EndHorizontal();

            if (isComputingNormals == 0)
            {
                if (targetNormalImage != null)
                {
                    if (GUILayout.Button("Create And Save Normal Map"))
                    {
                        isComputingNormals = 1;
                        return;
                    }
                }
                else
                {
                    GUILayout.Label("Add a Target Image to use this feature", EditorStyles.boldLabel);
                }
            }
            else
            {
                GUILayout.Label("Normal Map is currently being created, be patient", EditorStyles.boldLabel, GUILayout.Height(40));
                Repaint();
                isComputingNormals++;
                if (isComputingNormals > 5)
                {
                    var assetPath = AssetDatabase.GetAssetPath(targetNormalImage);
                    var tImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                    if (tImporter != null)
                    {
                        tImporter.isReadable = true;
                        tImporter.SaveAndReimport();
                    }

                    var normalToSave = CreateNormalMap(targetNormalImage, normalStrength, normalSmoothing);

                    var prefSavedPath = PlayerPrefs.GetString("All1ShaderNormals") + "/";
                    var path = prefSavedPath + "NormalMap.png";
                    if (File.Exists(path))
                        path = GetNewValidPath(path);
                    var texName = path.Replace(prefSavedPath, "");

                    path = EditorUtility.SaveFilePanel("Save texture as PNG", prefSavedPath, texName, "png");
                    if (path.Length != 0)
                    {
                        var pngData = normalToSave.EncodeToPNG();
                        if (pngData != null)
                            File.WriteAllBytes(path, pngData);
                        AssetDatabase.Refresh();

                        if (path.IndexOf("Assets/") >= 0)
                        {
                            var subPath = path.Substring(path.IndexOf("Assets/"));
                            var importer = AssetImporter.GetAtPath(subPath) as TextureImporter;
                            if (importer != null)
                            {
                                Debug.Log("Normal Map saved inside the project: " + subPath);
                                importer.filterMode = FilterMode.Bilinear;
                                importer.textureType = TextureImporterType.NormalMap;
                                importer.wrapMode = TextureWrapMode.Repeat;
                                importer.SaveAndReimport();
                                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(subPath, typeof(Texture)));
                            }
                        }
                        else
                        {
                            Debug.Log("Normal Map saved outside the project: " + path);
                        }
                    }

                    isComputingNormals = 0;
                }
            }

            GUILayout.Label("*This process will freeze the editor for some seconds, larger images will take longer", EditorStyles.boldLabel);
        }

        private void HandleSaveFolderEditorPref (string keyName, string defaultPath, string logsFeatureName)
        {
            if (!PlayerPrefs.HasKey(keyName))
                PlayerPrefs.SetString(keyName, defaultPath);
            materialTargetFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath(PlayerPrefs.GetString(keyName), typeof(DefaultAsset));
            if (materialTargetFolder == null)
            {
                PlayerPrefs.SetString(keyName, defaultPath);
                materialTargetFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath(PlayerPrefs.GetString(keyName), typeof(DefaultAsset));
                if (materialTargetFolder == null)
                {
                    materialTargetFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath("Assets/", typeof(DefaultAsset));
                    if (materialTargetFolder == null)
                        Debug.LogError("The desired save folder doesn't exist. Go to Window -> AllIn1ShaderWindow and set a valid folder");
                    else
                        PlayerPrefs.SetString("Assets/", defaultPath);
                }
            }

            materialTargetFolder = (DefaultAsset)EditorGUILayout.ObjectField("New " + logsFeatureName + " Folder", materialTargetFolder, typeof(DefaultAsset), false);

            if (materialTargetFolder != null && IsAssetAFolder(materialTargetFolder))
            {
                var path = AssetDatabase.GetAssetPath(materialTargetFolder);
                PlayerPrefs.SetString(keyName, path);
                EditorGUILayout.HelpBox("Valid folder! " + logsFeatureName + " save path: " + path, MessageType.Info, true);
            }
            else
            {
                EditorGUILayout.HelpBox("Select the new " + logsFeatureName + " Folder", MessageType.Warning, true);
            }
        }

        private void GradientCreator()
        {
            GUILayout.Label("Gradient Creator", bigLabel);
            GUILayout.Space(20);
            GUILayout.Label("This feature can be used to create textures for the Color Ramp Effect", EditorStyles.boldLabel);

            EditorGUILayout.GradientField("Gradient", gradient, GUILayout.Height(25));

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label("Texture Size:", GUILayout.MaxWidth(145));
                textureSizes = (TextureSizes)EditorGUILayout.EnumPopup(textureSizes, GUILayout.MaxWidth(200));
            }
            EditorGUILayout.EndHorizontal();

            var textureSize = (int)textureSizes;
            var gradTex = new Texture2D(textureSize, 1, TextureFormat.RGBA32, false);
            for (var i = 0; i < textureSize; i++)
                gradTex.SetPixel(i, 0, gradient.Evaluate(i / (float)textureSize));
            gradTex.Apply();

            GUILayout.Space(20);
            GUILayout.Label("Select the folder where new Gradient Textures will be saved", EditorStyles.boldLabel);
            HandleSaveFolderEditorPref("All1ShaderGradients", gradientSavesPath, "Gradient");

            var prefSavedPath = PlayerPrefs.GetString("All1ShaderGradients") + "/";
            if (Directory.Exists(prefSavedPath))
            {
                EditorGUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Gradient Texture Filtering: ", GUILayout.MaxWidth(170));
                    gradientFiltering = (FilterMode)EditorGUILayout.EnumPopup(gradientFiltering, GUILayout.MaxWidth(200));
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Save Gradient Texture"))
                {
                    var path = prefSavedPath + "ColorGradient.png";
                    if (File.Exists(path))
                        path = GetNewValidPath(path);
                    var texName = path.Replace(prefSavedPath, "");

                    path = EditorUtility.SaveFilePanel("Save texture as PNG", prefSavedPath, texName, "png");
                    if (path.Length != 0)
                    {
                        var pngData = gradTex.EncodeToPNG();
                        if (pngData != null)
                            File.WriteAllBytes(path, pngData);
                        AssetDatabase.Refresh();

                        if (path.IndexOf("Assets/") >= 0)
                        {
                            var subPath = path.Substring(path.IndexOf("Assets/"));
                            var importer = AssetImporter.GetAtPath(subPath) as TextureImporter;
                            if (importer != null)
                            {
                                Debug.Log("Gradient saved inside the project: " + subPath);
                                importer.filterMode = gradientFiltering;
                                importer.SaveAndReimport();
                                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath(subPath, typeof(Texture)));
                            }
                        }
                        else
                        {
                            Debug.Log("Gradient saved outside the project: " + path);
                        }
                    }
                }
            }
        }

        private static bool IsAssetAFolder (Object obj)
        {
            var path = "";

            if (obj == null)
                return false;

            path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

            if (path.Length > 0)
            {
                if (Directory.Exists(path))
                    return true;
                return false;
            }

            return false;
        }

        private string GetNewValidPath (string path, int i = 1)
        {
            var number = i;
            path = path.Replace(".png", "");
            var newPath = path + "_" + number;
            var fullPath = newPath + ".png";
            if (File.Exists(fullPath))
            {
                number++;
                fullPath = GetNewValidPath(path, number);
            }

            return fullPath;
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

        private enum ShaderTypes
        {
            Default,
            ScaledTime,
            MaskedUI,
            Urp2dRenderer
        }

        private enum TextureSizes
        {
            _2 = 2,
            _4 = 4,
            _8 = 8,
            _16 = 16,
            _32 = 32,
            _64 = 64,
            _128 = 128,
            _256 = 256,
            _512 = 512,
            _1024 = 1024,
            _2048 = 2048
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