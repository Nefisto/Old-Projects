using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
///     Represent each "position" in a room
/// </summary>
[Serializable]
public class BlockData
{
    public Vector2Int Position { get; set; }

    [ShowInInspector]
    [InlineEditor]
    public UnitData UnitData { get; set; }

    /// <summary>
    ///     The representation of this block at runtime
    /// </summary>
    public RuntimeBlock RuntimeBlock { get; set; }

    public bool IsEmpty => UnitData == null;
    public bool HasUnitOnIt => UnitData != null;


    /// <summary>
    ///  Used when generating room data making this room starts wiht a unit
    /// </summary>
    public UnitData InitialUnitOnThisBlock { get; set; }

    public bool HasGoblinUnit => HasUnitOnIt && UnitData.UnitSide == UnitSide.Goblin;
    public bool HasDwarfUnit => HasUnitOnIt && UnitData.UnitSide == UnitSide.Dwarf;

    public void SetUnit (UnitData unit)
    {
        UnitData = unit;
        UnitData.OnDie += RemoveUnit;
        UnitData.BlockData = this;

        UnitData.RuntimeUnit.transform.parent = RuntimeBlock.PiecePosition;
        UnitData.RuntimeUnit.transform.localPosition = Vector3.zero;
    }

    public void RemoveUnit()
    {
        UnitData = null;
    }

    public IEnumerator ApplyDamage (ApplyDamageSettings settings)
    {
        // TODO: Why not adding and remove into unit OnDie when set remove unit?
        settings.onDieCallback += RemoveUnit;
        yield return UnitData.TakeDamage(settings);
    }

    public IEnumerator ApplyHealing (ApplyHealingSettings settings)
    {
        yield return UnitData.GetHealed(settings);
    }

    public IEnumerator ApplyModifier (ModifierSettings settings)
    {
        yield return UnitData.ApplyModifier(settings);
    }

    public class ApplyHealingSettings
    {
        public int healing;
    }

    public class ApplyDamageSettings
    {
        public int damage;
        public Color damageColor = Color.red;

        public Action onDieCallback;
        public bool showDamageTest = true;
    }
}