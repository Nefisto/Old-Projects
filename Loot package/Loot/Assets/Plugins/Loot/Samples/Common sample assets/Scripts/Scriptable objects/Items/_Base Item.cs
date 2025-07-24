using System;
using Sample;
using UnityEngine;

namespace Sample
{
    public enum Quality
    {
        Common,
        Uncommon,
        Rare,
        Epic
    }

    public abstract class Item : ScriptableObject
    {
        public string id;

        public bool cursed;

        [Header("Info")]
        public Quality quality;

        public Sprite icon;

        [Multiline]
        public string description;

        private void Awake()
            => id = Guid.NewGuid().ToString();

        public void Print (int amount)
            => GameEvents.RaiseUpdateLog(new LogEntryContext
            {
                Icon = icon,
                Label = name,
                Amount = amount
            });
    }
}