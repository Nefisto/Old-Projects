using System.Collections;
using System.Linq;

public abstract partial class Skill
{
    public bool IsHoldableSkill => ChargePointsSettings.Any();

    public virtual IEnumerator HoldBegin()
    {
        yield break;
    }

    public virtual void HoldStay (HoldSettings settings) { }

    public virtual IEnumerator HoldFinish (HoldFinishSettings settings)
    {
        yield break;
    }
}