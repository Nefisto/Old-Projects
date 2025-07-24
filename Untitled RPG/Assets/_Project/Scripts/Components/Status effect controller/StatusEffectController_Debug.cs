using System.Collections.Generic;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
public partial class StatusEffectController
{
    [Button]
    [DisableInEditorMode]
    private void Test_AddStatusEffect (StatusEffectData effectToApply)
        => StartCoroutine(ApplyStatusEffect(new List<StatusEffectData> { effectToApply }));
}
#endif