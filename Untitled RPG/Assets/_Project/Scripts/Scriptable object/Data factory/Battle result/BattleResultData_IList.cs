using System.Collections;
using System.Collections.Generic;

public partial class BattleResultData
{
    public IEnumerator<OnDieContext> GetEnumerator()
        => BattleResult.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => ((IEnumerable)BattleResult).GetEnumerator();

    public void Add (OnDieContext item)
        => BattleResult.Add(item);

    public void Clear()
        => BattleResult.Clear();

    public bool Contains (OnDieContext item)
        => BattleResult.Contains(item);

    public void CopyTo (OnDieContext[] array, int arrayIndex)
        => BattleResult.CopyTo(array: array, arrayIndex: arrayIndex);

    public bool Remove (OnDieContext item)
        => BattleResult.Remove(item);

    public int Count => BattleResult.Count;

    public bool IsReadOnly => ((ICollection<OnDieContext>)BattleResult).IsReadOnly;

    public int IndexOf (OnDieContext item)
        => BattleResult.IndexOf(item);

    public void Insert (int index, OnDieContext item)
        => BattleResult.Insert(index: index, item: item);

    public void RemoveAt (int index)
        => BattleResult.RemoveAt(index);

    public OnDieContext this [int index]
    {
        get => BattleResult[index];
        set => BattleResult[index] = value;
    }
}