using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract partial class BattleActor
{
    public event Action<ManaPool> OnUpdateMana;
    
    [TabGroup("Mana pool")]
    [Title("Settings")]
    [LabelWidth(200)]
    [SerializeField]
    private int manaRandomizedPerTurn = 3;

    [TabGroup("Mana pool")]
    [Title("Debug")]
    [HideLabel]
    [ReadOnly]
    [SerializeField]
    private ManaPool manaPool;

    public ManaPool Mana
    {
        get => manaPool;
        set
        {
            manaPool = value;
            OnUpdateMana?.Invoke(manaPool);
        }
    }

    private void ResetMana()
        => Mana = new ManaPool();

    private IEnumerator RandomizeMana()
    {
        var manaGainedThisTurn = new ManaPool();
        for (var i = 0; i < manaRandomizedPerTurn; i++)
        {
            var randomizedValue = Random.value;

            var mana = ConvertRandomValueToMana(randomizedValue);
            
            manaGainedThisTurn.Add(mana, 1);
        }
        
        GameEvents.Battle.RaiseCombatLogAction(new ManaGainedLog(this, manaGainedThisTurn));

        Mana += manaGainedThisTurn;
        
        yield return new WaitForSeconds(1f);
    }

    private ManaName ConvertRandomValueToMana (float randomizedValue)
    {
        if (randomizedValue <= .33f)
            return ManaName.Red;

        if (randomizedValue <= .66f)
            return ManaName.Green;

        return ManaName.Blue;
    }
}