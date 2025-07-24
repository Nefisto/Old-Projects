#if UNITY_EDITOR && UNITY_2020_1_OR_NEWER
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Loot.Editor
{
    public partial class LootDebugger : EditorWindow
    {
        private static DebuggerRefresher refreshRoutineRuntime;

        // Hot reload
        internal static float RefreshRate = .5f;
        public VisualTreeAsset defaultReferenceTree;
        public VisualTreeAsset defaultReferenceDropEntry;
        public Texture2D defaultReferenceNoneIcon;

        [SerializeField]
        private int cachedOriginalSelectedTableIndex = -1;

        [SerializeField]
        private int cachedCurrentSelectedTableIndex = -1;

        private DropTable currentOriginalSelectedTable;
        private DropTable currentSelectedTable;
        private Button refreshRateButton;

        private VisualElement refreshRateContainer;
        private VisualElement refreshRateDragger;
        private Label refreshRatePrefix;
        private Slider refreshRateSlider;
        private Label refreshRateSuffix;


        // Cached fields
        private Toggle showExtensions;
        private Toggle showRepetition;
        private Toggle showColorTendency;

        private VisualElement tableInformation;
        private VisualTreeAsset whenPlayingDropEntry;
        private Texture2D whenPlayingNoneIcon;

        // Default references work in editor only, so when opening the debug while playing, above refs would be null
        private VisualTreeAsset whenPlayingTree;

        private VisualTreeAsset Tree => EditorApplication.isPlaying ? whenPlayingTree : defaultReferenceTree;
        private VisualTreeAsset DropEntry => EditorApplication.isPlaying || !defaultReferenceDropEntry ? whenPlayingDropEntry : defaultReferenceDropEntry;
        private Texture2D NoneIcon => EditorApplication.isPlaying || !defaultReferenceNoneIcon ? whenPlayingNoneIcon : defaultReferenceNoneIcon;

        private void OnDestroy()
        {
            if (refreshRoutineRuntime != null)
                refreshRoutineRuntime.OnRefresh -= RefreshTableInformation;

            refreshRateButton.clicked -= RefreshTableInformation;
        }

        private void CreateGUI()
        {
            if (Application.isPlaying)
            {
                LoadAssets();
                CreateRefresher();

                refreshRoutineRuntime.OnRefresh += RefreshTableInformation;
            }

            // For some reason when changing the layout, unity is calling this script editor even though the window
            //      is not open
            if (Tree == null)
                return;

            Tree.CloneTree(rootVisualElement);

            showExtensions = rootVisualElement.Q<Toggle>("table-information--options--show-hierarchy");
            showRepetition = rootVisualElement.Q<Toggle>("table-information--options--show-repetition");
            showColorTendency = rootVisualElement.Q<Toggle>("table-information--options--show-color-changes");

            tableInformation = rootVisualElement.Q<VisualElement>("table-information");

            refreshRateContainer = rootVisualElement.Q<VisualElement>("table-information--options--entry--refresh-rate");
            refreshRateSlider = refreshRateContainer.Q<Slider>("table-information--options--refresh--slider");
            refreshRateSuffix = refreshRateContainer.Q<Label>("table-information--options--refresh-suffix");
            refreshRateDragger = refreshRateSlider.Q<VisualElement>("unity-dragger");
            refreshRatePrefix = refreshRateSlider.Q<Label>();
            refreshRateButton = refreshRateContainer.Q<Button>();

            OriginalTableDrawer();
            RuntimeTableDrawer();

            OnSelectionChangeInOriginalListView();
            OnSelectionChangeInRuntimeListView();
            OnChangeRefreshRate();

            // Hot reload
            originalListView.selectedIndex = cachedOriginalSelectedTableIndex;
            runtimeListView.selectedIndex = cachedCurrentSelectedTableIndex;
            refreshRateSlider.value = RefreshRate;

            RefreshRefreshRate(refreshRateSlider.value);
            refreshRateButton.clicked += RefreshTableInformation;

            rootVisualElement
                .Q<Button>("table-information--options--show-hierarchy--button")
                .clicked += () => showExtensions.value = !showExtensions.value;
            rootVisualElement
                .Q<Button>("table-information--options--show-repetition--button")
                .clicked += () =>
            {
                if (!showRepetition.enabledSelf)
                    return;

                showRepetition.value = !showRepetition.value;
            };
            rootVisualElement
                .Q<Button>("table-information--options--show-color-changes--button")
                .clicked += () => showColorTendency.value = !showColorTendency.value;

            showExtensions.RegisterValueChangedCallback(ctx =>
            {
                if (!ctx.newValue)
                    showRepetition.value = false;

                showRepetition.SetEnabled(ctx.newValue);
                TableInformationDrawer();
            });
            showRepetition.RegisterValueChangedCallback(_ => TableInformationDrawer());
        }

        public static void CreateRefresher()
        {
            if (refreshRoutineRuntime != null)
                return;

            refreshRoutineRuntime = new GameObject("Debug refresher")
                .AddComponent<DebuggerRefresher>();

            DontDestroyOnLoad(refreshRoutineRuntime);
        }

        public void RefreshTableInformation()
        {
            if (currentSelectedTable == null)
                return;

            var cachedSum = currentSelectedTable.SumOfWeights();
            UpdateTableDrops(cachedSum);
            UpdatePercentageCalculation(cachedSum);
        }

        private void OnChangeRefreshRate()
            => refreshRateSlider.RegisterValueChangedCallback(valueChanged =>
            {
                RefreshRate = valueChanged.newValue;

                RefreshRefreshRate(valueChanged.newValue);

                if (!Application.isPlaying)
                    return;

                if (valueChanged.newValue == 0f)
                    refreshRoutineRuntime.StopRefresh();

                if (valueChanged.previousValue == 0f && valueChanged.newValue != 0f)
                    refreshRoutineRuntime.StartRefresh();
            });

        private void RefreshRefreshRate (float newValue)
        {
            refreshRateSuffix.text = $"{newValue:F3} s";
            refreshRateSuffix.EnableInClassList("unity-base-slider__label--disabled", newValue == 0f);
            refreshRatePrefix.EnableInClassList("unity-base-slider__label--disabled", newValue == 0f);
            refreshRateDragger.EnableInClassList("unity-slider-dragger--enabled", newValue != 0f);
        }

        private void LoadAssets()
        {
            whenPlayingTree = Resources.Load<VisualTreeAsset>("Loot-debugger");
            whenPlayingDropEntry = Resources.Load<VisualTreeAsset>("Drop-entry");
            whenPlayingNoneIcon = Resources.Load<Texture2D>("Icons/no-icon");
        }

        [MenuItem(EditorConstants.DebuggerWindow, priority = 0)]
        private static void ShowWindow()
        {
            var window = GetWindow<LootDebugger>();
            window.titleContent = new GUIContent("Loot debugger");

            window.minSize = new Vector2(1100f, 650f);

            window.Show();
        }
    }
}
#endif