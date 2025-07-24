using System;
using UnityEngine;

namespace Loot.NTools
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new Exception("An instance of this singleton already exists.");
            }

            Instance = this as T;
        }
    }
}