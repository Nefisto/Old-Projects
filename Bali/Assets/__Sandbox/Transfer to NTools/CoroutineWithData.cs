using System.Collections;
using UnityEngine;

public class CoroutineWithData<T>
{
    public T result;
    private IEnumerator target;

    public CoroutineWithData (MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        Coroutine = owner.StartCoroutine(Run());
    }

    public Coroutine Coroutine { get; }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = (T)target.Current;
            yield return result;
        }
    }
}