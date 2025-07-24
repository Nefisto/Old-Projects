using OldSample.Utilities;
using Sample.Utilities;
using UnityEngine;

public class SetupScene : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private string sampleTitle = "";

    [SerializeField]
    [Multiline(5)]
    private string sampleInfo = "";

    [SerializeField]
    private bool shouldShowGlobalPanel;

    [SerializeField]
    private bool shouldShowTreatTableAsDrop;

    [Space]
    [Header("Control")]
    [SerializeField]
    private CommonGUIController guiController;

    private void Start()
        => guiController.Setup(new GUISetupContext
        {
            SampleTitle = sampleTitle,
            SampleInfo = sampleInfo,
            ShouldShowGlobalPanel = shouldShowGlobalPanel,
            ShouldShownTreatTableAsDropOption = shouldShowTreatTableAsDrop
        });

    public void ResetSampleInfo()
        => guiController.UpdateSampleInfo(sampleInfo);
}