using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class ScriptableObjectFactory<T> : SerializedScriptableObject
{
    [HideLabel]
    [InlineProperty]
    [SerializeField]
    public T serializedField;

    public virtual T GetInstance() => (T)SerializationUtility.CreateCopy(serializedField);
}