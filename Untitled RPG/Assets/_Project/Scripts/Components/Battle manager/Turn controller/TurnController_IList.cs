using System.Collections;
using System.Collections.Generic;

public partial class TurnController
{
    public IEnumerator<BattleActionContext> GetEnumerator() => waitingActors.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)waitingActors).GetEnumerator();

    public void Add (BattleActionContext item) => waitingActors.Add(item);

    public void Clear() => waitingActors.Clear();

    public bool Contains (BattleActionContext item) => waitingActors.Contains(item);

    public void CopyTo (BattleActionContext[] array, int arrayIndex) => waitingActors.CopyTo(array, arrayIndex);

    public bool Remove (BattleActionContext item) => waitingActors.Remove(item);

    public int Count => waitingActors.Count;
    public bool IsReadOnly => ((ICollection<BattleActionContext>)waitingActors).IsReadOnly;

    public int IndexOf (BattleActionContext item) => waitingActors.IndexOf(item);

    public void Insert (int index, BattleActionContext item) => waitingActors.Insert(index, item);

    public void RemoveAt (int index) => waitingActors.RemoveAt(index);

    public BattleActionContext this [int index]
    {
        get => waitingActors[index];
        set => waitingActors[index] = value;
    }
}