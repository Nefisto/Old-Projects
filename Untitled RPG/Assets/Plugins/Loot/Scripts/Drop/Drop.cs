using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Loot
{
    /// <summary>
    ///     Represent the entry in your drop table
    /// </summary>
    [Serializable]
    public partial class Drop : ICloneable
    {
        [SerializeField]
        private ScriptableObject entry;

        [SerializeField]
        private Vector2Int amountRange = new Vector2Int(1, 1);

        [SerializeField]
        [Min(0)]
        private int weight = 1;

        [FormerlySerializedAs("percentage")]
        [SerializeField]
        private float odds;

        [Tooltip("Should this drop be treated as an extension drop?\n\n*The entry MUST be a Drop table AND have the same" +
                 " percentage calculation method otherwise it will be disabled")]
        [SerializeField]
        private bool isExtension;

        [SerializeField]
        private bool isGuaranteed;

        [FormerlySerializedAs("isHidden")]
        [SerializeField]
        private bool isDisabled;

        /// <summary>
        ///     Weighted drops must know weight of all items to know their percentage, and for this they to know information
        ///     about a layer above.
        ///     OBS: This only exist to be able to know the percentage from the drop instead of from the table.
        /// </summary>
        [SerializeField]
        internal DropTable ownerTable;

        [SerializeField]
        // ReSharper disable once NotAccessedField.Global
        internal float weightPercentage = -1f;

        // Support for amount range drawer
        [SerializeField]
        internal Vector2Int amountLimit = new Vector2Int(0, 10);

        // To avoid break
        [SerializeField]
        internal bool hasAlreadyUpdatedLimit;

        private List<Predicate<Drop>> filters = new List<Predicate<Drop>>();

        internal Drop OriginalDrop;

        internal bool InternalIsExtensionDrop => entry is DropTable && IsExtensionDrop;
    }
}