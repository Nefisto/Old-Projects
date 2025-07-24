using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Bali/Card", order = 0)]
public class CardData : SerializedScriptableObject
{
    public Action<int> OnUpdateLife;
    
    [Title("Settings")]
    [OdinSerialize]
    public string Name { get; private set; }

    [OdinSerialize]
    public CardClass Class { get; private set; }
    
    [OdinSerialize]
    public Sprite Portrait { get; private set; }

    [OdinSerialize]
    public int BaseDamage { get; private set; }

    [OdinSerialize]
    public int BaseHealth { get; private set; }

    [OdinSerialize]
    public Magicka Magicka { get; private set; }

    public void TakeDamage (int damage)
    {
        BaseHealth -= damage;
        OnUpdateLife?.Invoke(BaseHealth);
    }

    public bool IsAlive()
        => BaseHealth > 0;
}