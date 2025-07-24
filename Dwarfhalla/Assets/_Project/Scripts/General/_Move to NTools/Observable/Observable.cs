using System;
using UnityEngine;

[Serializable]
public class Observable<T>
{
    // A delegate just to allow parameters to be named on the listener
    public delegate void ChangeValueDelegate (T old, T current);

    [SerializeField]
    private T value;

    public T Value
    {
        get => value;
        set
        {
            if (Equals(this.value, value))
                return;

            var oldValue = this.value;
            this.value = value;
            OnValueChanged?.Invoke(oldValue, value);
        }
    }

    public event ChangeValueDelegate OnValueChanged;
}