using UnityEngine;

public interface IFloatText
{
    public void AddCustomFloatText (FloatTextSettings floatTextSettings, int priority = 5);

    public void DamageText (string message, Transform position)
        => AddCustomFloatText(new FloatTextSettings(message, position, textColor: Color.red));

    public void PoisonText (string message, Transform position)
        => AddCustomFloatText(new FloatTextSettings(message, position, textColor: Color.green));

    public void BleedText (string message, Transform position)
        => AddCustomFloatText(new FloatTextSettings(message, position, textColor: Color.magenta));

    public void MissText (Transform position)
        => AddCustomFloatText(new FloatTextSettings("miss", position, textColor: Color.gray));

    public void CriticalText (Transform position)
        => AddCustomFloatText(new FloatTextSettings("crit!", position, textColor: new Color()));
}