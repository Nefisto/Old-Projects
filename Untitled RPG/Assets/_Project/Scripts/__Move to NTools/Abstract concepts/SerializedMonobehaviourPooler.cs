using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class SerializedMonobehaviourPooler<T> : SerializedMonoBehaviour
    where T : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    protected T prefab;

    [TitleGroup("References")]
    [SerializeField]
    protected List<T> pool;

    public T GetPooledObject (bool activeState = false)
    {
        var pooledObject = GetMultiplePooledObjects(1).First();

        pooledObject.gameObject.SetActive(activeState);

        return pooledObject;
    }

    public List<T> GetMultiplePooledObjects (int amount)
    {
        var pooledObjects = InternalGetter(amount);

        if (pooledObjects.Count() < amount)
            IncreasePool();

        pooledObjects = InternalGetter(amount);

        return pooledObjects;
    }

    public void ReturnPooledObject (T objectToReturn)
    {
        objectToReturn.transform.SetParent(transform, false);
        objectToReturn.gameObject.SetActive(false);
    }

    private List<T> InternalGetter (int amount)
    {
        return pool
            .Where(t => t.gameObject.activeInHierarchy == false)
            .Take(amount)
            .ToList();
    }

    private void IncreasePool()
    {
        var amountToIncrease = Mathf.Max((int)(pool.Count * .5f), 10);
        for (var i = 0; i < amountToIncrease; i++)
        {
            var instance = Instantiate(prefab, transform, false);
            instance.gameObject.SetActive(false);

            pool.Add(instance);
        }
    }
}