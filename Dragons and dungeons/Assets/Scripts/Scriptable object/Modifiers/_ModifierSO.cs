using UnityEngine;

public abstract class ModifierSO : ScriptableObject
{
    public abstract void Act (BattleActionContext context);
}