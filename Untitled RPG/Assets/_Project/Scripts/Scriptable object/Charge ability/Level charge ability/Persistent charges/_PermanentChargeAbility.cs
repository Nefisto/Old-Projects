using Sirenix.OdinInspector;
using UnityEngine;

public abstract class PersistentChargeAbility : LevelChargeAbility
{
    [TitleGroup("Settings")]
    [SerializeField]
    protected StatusEffectData statusEffect;
}