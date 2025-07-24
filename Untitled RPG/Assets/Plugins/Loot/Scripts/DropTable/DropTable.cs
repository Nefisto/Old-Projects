using System;
using System.Collections.Generic;
using Loot.Enum;
using UnityEngine;

// ReSharper disable UnusedMember.Global

namespace Loot
{
    /// <summary>
    ///     Container for your drops and configurations of how to drop things
    /// </summary>
    [CreateAssetMenu(fileName = EditorConstants.DropTableName, menuName = EditorConstants.DropTableMenu)]
    public sealed partial class DropTable : ScriptableObject, IEnumerable<Drop>
    {
        [Tooltip("Used in cases that you want to show the table as a drop in some HUD")]
        [SerializeField]
        private Sprite tableIcon;

        [Multiline]
        [Delayed]
        [SerializeField]
        private string tableDescription = string.Empty;

        [Tooltip("This decides the method used to calculate the drops percentage.")]
        [SerializeField]
        private PercentageCalculation percentageCalculation;

        [SerializeField]
        private List<Drop> drops = new List<Drop>();

        [SerializeField]
        internal List<DropTable> clones = new List<DropTable>();

        /// <summary>
        ///     As we can't rely on a Func-bool- because we can't get what return this will give,
        ///     we need to control it though a list
        /// </summary>
        private List<Predicate<Drop>> filters = new List<Predicate<Drop>>();

        // To verify when you're working with a clone table and when not
        private bool isClone;

        private static bool IsValidToSumWeight (Drop drop)
            => !drop.IsGuaranteed && !drop.IsDisabled && !drop.InternalIsExtensionDrop;
    }
}