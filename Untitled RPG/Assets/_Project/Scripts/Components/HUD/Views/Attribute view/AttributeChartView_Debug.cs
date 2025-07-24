#if DEBUG_CODE
public partial class AttributeChartView
{
    [DisableInEditorButton]
    private void Setup (TraitChartFactory traitChartFactory) => StartCoroutine(Setup(traitChartFactory.GetInstance()));
}
#endif