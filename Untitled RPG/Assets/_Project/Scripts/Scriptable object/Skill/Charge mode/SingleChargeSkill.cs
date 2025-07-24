using System.Collections;
using System.Linq;

public abstract class SingleChargeSkill : Skill, IHoldableSkill
{
    public override IEnumerator HoldBegin()
    {
        yield return ServiceLocator.ChargeBar.Setup(ChargeModeEnum.SingleCharge,
            new SingleChargeMode.Settings
            {
                chargeAbilities = ChargePointsSettings.SelectMany(s => s.chargeAbilities).ToList()
            });

        foreach (var chargeAbility in ChargePointsSettings.SelectMany(s => s.chargeAbilities)
                     .Cast<SingleChargeAbility>())
            yield return chargeAbility.RegisterAbility();
    }

    public override IEnumerator HoldFinish (HoldFinishSettings settings)
    {
        foreach (var chargeAbility in ChargePointsSettings.SelectMany(s => s.chargeAbilities)
                     .Cast<SingleChargeAbility>())
            yield return chargeAbility.RemoveAbility();

        ServiceLocator.ChargeBar.Close();
    }
}