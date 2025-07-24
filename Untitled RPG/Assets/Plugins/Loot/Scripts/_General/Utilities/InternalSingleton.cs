using UnityEngine;

namespace Loot.Utilities
{
    // A singleton for being referenced inside assembly only, it was used to create an better accessible LootSettings
    public abstract class InternalSingleton<T> where T : class, new()
    {
        protected static T _instance;

        internal static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = new T();
                if (_instance == null)
                    Debug.LogErrorFormat("No instance of {0} has been created for some reason", typeof(T));

                return _instance;
            }
        }
    }
}