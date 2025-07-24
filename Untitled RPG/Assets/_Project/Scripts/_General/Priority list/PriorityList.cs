using System;
using System.Collections;
using System.Collections.Generic;
using NTools;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
///     As I'm more worried about being able to send a message FIRST then guarantee the actual order of the priority list,
///     this will be enough.
///     Priority will have no effect if the size of the list grows, but I'm consuming it every frame, so it should never
///     grow too much
/// </summary>
[Serializable]
public class PriorityList<T> : IList<T>
    where T : class
{
    private List<T> internalList = new();

    public int IndexOf (T item) => internalList.IndexOf(item);
    public void Insert (int index, T item) => internalList.Insert(index, item);
    public void RemoveAt (int index) => internalList.RemoveAt(index);

    public T this [int index]
    {
        get => internalList[index];
        set => internalList[index] = value;
    }

    public IEnumerator<T> GetEnumerator() => internalList.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)internalList).GetEnumerator();
    public void Add (T item) => Add(5, item);
    public void Clear() => internalList.Clear();
    public bool Contains (T item) => internalList.Contains(item);
    public void CopyTo (T[] array, int arrayIndex) => internalList.CopyTo(array, arrayIndex);
    public bool Remove (T item) => internalList.Remove(item);

    public int Count => internalList.Count;

    public bool IsReadOnly => ((ICollection<T>)internalList).IsReadOnly;

    public void Add (int priority, T item)
    {
        Assert.IsNotNull(item);

        if (internalList.IsEmpty() || priority > Count)
        {
            internalList.Add(item);
            return;
        }

        priority = Mathf.Clamp(priority, 0, Count);
        internalList.Insert(priority, item);
    }
}