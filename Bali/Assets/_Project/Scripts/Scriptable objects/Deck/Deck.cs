using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "Deck", menuName = "Bali/Deck", order = 0)]
public class Deck : SerializedScriptableObject, IList<CardData>
{
    [OdinSerialize]
    public List<CardData> InternalDeck { get; private set; } = new();

    public int MagickaScore => 3;

    public IEnumerator<CardData> GetEnumerator()
        => InternalDeck.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)InternalDeck).GetEnumerator();

    public void Add (CardData item)
        => InternalDeck.Add(item);

    public void Clear()
        => InternalDeck.Clear();

    public bool Contains (CardData item)
        => InternalDeck.Contains(item);

    public void CopyTo (CardData[] array, int arrayIndex)
        => InternalDeck.CopyTo(array, arrayIndex);

    public bool Remove (CardData item)
        => InternalDeck.Remove(item);

    public int Count => InternalDeck.Count;

    public bool IsReadOnly => ((ICollection<CardData>)InternalDeck).IsReadOnly;

    public int IndexOf (CardData item)
        => InternalDeck.IndexOf(item);

    public void Insert (int index, CardData item)
        => InternalDeck.Insert(index, item);

    public void RemoveAt (int index)
        => InternalDeck.RemoveAt(index);

    public CardData this [int index]
    {
        get => InternalDeck[index];
        set => InternalDeck[index] = value;
    }

    public void Setup (IEnumerable<CardData> cards)
        => InternalDeck = cards
            .Select(Instantiate)
            .ToList();
}