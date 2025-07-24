﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

/// <typeparam name="T">Factory type</typeparam>
/// <typeparam name="K">Data type</typeparam>
public class FactoryDataLoader<T, K> : DataLoader<T>
    where T : ScriptableObjectFactory<K>
{
    public FactoryDataLoader (string collectionKey, bool hasFallback = false) : base(collectionKey, hasFallback) { }

    /// <summary>
    /// Data created from loaded factories
    /// </summary>
    public List<K> InstanceData { get; protected set; } = new();

    /// <summary>
    ///     Fallback instance from a Factory
    /// </summary>
    public K InstanceFallback { get; protected set; }

    public K GetInstanceDataThatMatch (Predicate<K> condition)
        => InstanceData.FirstOrDefault(data => condition(data)) ?? InstanceFallback;

    protected override void AsyncCallback (T addressable)
    {
        base.AsyncCallback(addressable);

        if (Fallback == addressable)
            return;

        InstanceData.Add(addressable.GetInstance());
    }

    protected override void StoreFallback()
    {
        base.StoreFallback();

        InstanceFallback = Fallback.GetInstance();
    }
}

public class DataLoader<T> where T : Object
{
    private AsyncOperationHandle handle;

    private bool hasFallback;

    public DataLoader (string collectionKey, bool hasFallback)
    {
        CollectionKey = collectionKey;
        this.hasFallback = hasFallback;
    }

    public T Fallback { get; protected set; }
    protected string CollectionKey { get; }

    public List<T> Data { get; protected set; } = new();

    public bool IsLoaded => handle.IsValid() && handle.IsDone;
    public float Percentage => handle.PercentComplete;

    public virtual IEnumerator LoadData (Action<List<T>> OnCompleteLoad = null)
    {
        if (IsLoaded)
            yield break;

        if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
        {
            handle = Addressables.LoadAssetsAsync<GameObject>(CollectionKey,
                AsyncCallback,
                releaseDependenciesOnFailure: false);
        }
        else
        {
            handle = Addressables.LoadAssetsAsync<T>(CollectionKey,
                AsyncCallback,
                releaseDependenciesOnFailure: false);
        }

        yield return handle;

        OnCompleteLoad?.Invoke(Data);

        if (!hasFallback)
            yield break;

        StoreFallback();
    }

    public T GetDataThatMatch (Predicate<T> condition) => Data.FirstOrDefault(data => condition(data)) ?? Fallback;

    protected virtual void AsyncCallback (GameObject addressable) => Data.Add(addressable.GetComponent<T>());

    protected virtual void AsyncCallback (T addressable) => Data.Add(addressable);

    protected virtual void StoreFallback() => Fallback = Data.First(d => d.name.Contains("Default"));

    public virtual void UnloadData()
    {
        Data.Clear();
        Addressables.Release(handle);
    }
}