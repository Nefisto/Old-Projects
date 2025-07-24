using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Loot.Context;
using Loot.Enum;
using Loot.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable EventNeverSubscribedTo.Global

// ReSharper disable UnusedMember.Global

namespace Loot
{
    public sealed partial class DropTable
    {
        public static Action<ModifyContext> OnGlobalModify;
        public static Action<ModifiedContext> OnGlobalModified;
        public Action<ModifiedContext> OnLocalModified;
        public Action<ModifyContext> OnLocalModify;

        /// <summary>
        ///     Name that appear on debug if this table is a cloned one
        /// </summary>
        public string DebugName { get; set; } = "";

        /// <summary>
        ///     Provide an icon to the table.
        ///     <para></para>
        ///     <para>It`s normally used when you want to list the table as a drop in some list.</para>
        /// </summary>
        public Sprite Icon
        {
            get => tableIcon;
            set => tableIcon = value;
        }

        /// <summary>
        ///     Table description
        /// </summary>
        public string Description
        {
            get => tableDescription;
            set => tableDescription = value;
        }

        /// <summary>
        ///     <para>This decides the method used to calculate the drops percentage.</para>
        ///     <para></para>
        ///     <para>You can get more info about different calculations methods on <see cref="PercentageCalculation" /></para>
        /// </summary>
        public PercentageCalculation PercentageCalculation
        {
            get => percentageCalculation;
            set => percentageCalculation = value;
        }

        /// <summary>
        ///     False returns will make the drop to not appear
        /// </summary>
        public event Predicate<Drop> OnTableFilter
        {
            add => filters.Add(value);
            remove => filters.Remove(value);
        }

        /// <summary>
        /// Called before each drop request
        /// </summary>
        public event Action<DroppingContext> OnDropping;
        
        /// <summary>
        /// Called after each drop request
        /// </summary>
        public event Action<DroppedContext> OnDropped;


        public void AddDrop (Drop drop)
        {
            drops.Add(drop);
            drop.ownerTable = this;
        }

        public void RemoveDrop (Drop drop)
        {
            CheckForDestructiveOperations();

            drops.Remove(drop);
        }

        public void RemoveAllDrops (Predicate<Drop> predicate)
        {
            CheckForDestructiveOperations();

            drops.RemoveAll(predicate);
        }

        /// <summary>
        ///     Request a drop
        /// </summary>
        /// <param name="n">Number of requests</param>
        /// <returns>Bag with result drops</returns>
        public Bag Drop (int n = 1)
            => CustomDrop(droppingCallback: OnDropping, droppedCallback: OnDropped, numberOfDrops: n);

        /// <summary>
        ///     Request a drop and reroll all tables until the resulting bag has no tables in it
        /// </summary>
        /// <param name="n">Number of requests</param>
        /// <returns>Bag with result drops</returns>
        public Bag DropAndRerollTables (int n = 1)
            => CustomDrop(droppingCallback: OnDropping, droppedCallback: OnDropped, numberOfDrops: n)
                .RerollAllTables();

        /// <summary>
        ///     Request a drop using custom behaviors (passing no parameter will give the same result from Drop)
        /// </summary>
        /// <param name="numberOfDrops">Number of requests</param>
        /// <param name="customDropBehaviour">Manually control what will be dropped</param>
        /// <param name="droppingCallback">Called before each drop requested</param>
        /// <param name="droppedCallback">Called after each drop requested</param>
        /// <returns>Bag with result drops</returns>
        public Bag CustomDrop (
            int numberOfDrops = 1,
            Func<List<Drop>, Bag> customDropBehaviour = null,
            Action<DroppingContext> droppingCallback = null,
            Action<DroppedContext> droppedCallback = null)
        {
            var resultBag = new Bag();

            for (var i = 0; i < numberOfDrops; i++)
            {
                var dropList = this.ToList();

                droppingCallback?.Invoke(new DroppingContext(this, dropList));

                // Drop
                var temporaryBag = customDropBehaviour?.Invoke(dropList) ??
                                   (percentageCalculation == PercentageCalculation.Simple
                                       ? new Bag(SimpleDrop(dropList))
                                       : new Bag(WeightedDrop(dropList)));

                droppedCallback?.Invoke(new DroppedContext(this, temporaryBag));

                resultBag.Add(temporaryBag);
            }

            return resultBag;
        }

        /// <summary>
        /// Clone current table drops, modifiers, filter, callbacks and set a custom debug name to it
        /// </summary>
        /// <param name="debugName">Name that will appear on debug window</param>
        /// <returns>The instance of the cloned table</returns>
        public DropTable Clone (string debugName = "")
        {
            var instance = Instantiate(this);

            // Callbacks
            instance.OnDropping = OnDropping;
            instance.OnDropped = OnDropped;

            instance.OnLocalModify = OnLocalModify;
            instance.OnLocalModified = OnLocalModified;
            instance.filters = new List<Predicate<Drop>>(filters);

            instance.isClone = true;
            instance.DebugName = debugName;

            clones.Add(instance);

            return instance;
        }

        /// <summary>
        ///     This will open extensions, remove hidden, remove duplications apply modifiers and filters and THEN
        ///     give to you the sum of weights
        /// </summary>
        /// <returns>The sum of drop's weight</returns>
        public int SumOfWeights()
            => this
                .Where(IsValidToSumWeight)
                .Sum(drop => drop.Weight);

        /// <summary>
        ///     Clean filters from this table
        /// </summary>
        public void RemoveAllTableFilters()
            => filters.Clear();

        public void AddFilterToDrops (Predicate<Drop> dropsGroup, Predicate<Drop> filter)
        {
            foreach (var drop in drops.Where(new Func<Drop, bool>(dropsGroup)))
                drop.AddFilter(filter);
        }

        private static List<Drop> SimpleDrop (List<Drop> drops)
        {
            // Add guaranteed drops
            var resultList = drops
                .Where(drop => drop.IsGuaranteed)
                .ToList();

            foreach (var drop in drops)
            {
                if (drop.IsGuaranteed || drop.IsDisabled)
                    continue;

                var rand = Random.value * 100;

                if (rand <= drop.Percentage)
                    resultList.Add(drop);
            }

            return resultList;
        }

        private List<Drop> WeightedDrop (List<Drop> drops)
        {
            // Add guaranteed drops
            var resultList = drops
                .Where(drop => drop.IsGuaranteed)
                .ToList();

            // Sum weights
            var weightSum = SumOfWeights();

            // Case all drops are guaranteed or locked
            if (weightSum == 0)
                return resultList;

            var rand = Random.Range(0, weightSum);
            foreach (var drop in drops.Where(drop => !drop.IsGuaranteed && !drop.IsDisabled))
            {
                if (rand < drop.Weight)
                {
                    resultList.Add(drop);
                    return resultList;
                }

                rand -= drop.Weight;
            }

            throw new WarningException("Weighted drop with integer range should never reach this point");
        }

        private void CheckForDestructiveOperations()
        {
            if (!isClone && !LootSettings.EnableWorkOnOriginal)
                throw new Exception(Messages.DestructiveOperationsOnOriginalAssetsWarning);
        }
    }
}