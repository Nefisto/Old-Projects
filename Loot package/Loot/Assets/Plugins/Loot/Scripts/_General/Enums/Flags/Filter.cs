using System;

namespace Loot.Enum
{
    /// <summary>
    ///     Use this to filter what you want to receive through <see cref="DropTable.CustomEnumerator" />
    ///     <para />
    ///     The default behavior is as follow:
    ///     <para />
    ///     - Include all drops that isn't hidden nor extensions
    ///     <para />
    ///     - Open all extension drops until we have none of them OR have reach at max depth, this maximum can be changed in
    ///     the Settings
    ///     <para />
    ///     - Remove all drops marked as hidden
    ///     <para />
    ///     - Clone all drops and
    ///     <para />
    ///     - Apply all the modifiers and filter
    ///     <para />
    ///     <para />
    ///     If you want to use the default behavior, you can just iterate over the table
    /// </summary>
    [Flags]
    public enum Filter
    {
        /// <summary>
        ///     All fields disabled
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Active: Plain list, as you see in the inspector
        ///     <para />
        ///     Default: Open all extensions and show then as drops
        ///     <para />
        ///     Note*: drops from extensions are always clone
        /// </summary>
        IncludeExtensions = 1 << 0,

        /// <summary>
        ///     Active: Open all extensions
        ///     Default: Skip repeated extensions tables (check repeated by reference by default)
        /// </summary>
        DontRemoveRepetitions = 1 << 1,

        /// <summary>
        ///     Active: Include drops marked as disabled
        ///     <para />
        ///     Default: Remove drops marked as disabled
        ///     <para />
        /// </summary>
        IncludeDisabledDrops = 1 << 2,

        /// <summary>
        ///     Active: Don't apply global modify on drops
        ///     <para />
        ///     Default: Apply global modify on drops
        ///     <para />
        ///     Note*: Valid only on runtime calls
        /// </summary>
        DontInvokeGlobalModify = 1 << 3,

        /// <summary>
        ///     Active: Don't apply global modified on drops
        ///     <para />
        ///     Default: Apply global modified on drops
        ///     <para />
        ///     Note*: Valid only on runtime calls
        /// </summary>
        DontInvokeGlobalModified = 1 << 4,

        /// <summary>
        ///     Active: Don't apply table specific modify on drops
        ///     <para />
        ///     Default: Apply table specific modify on drops
        ///     <para />
        ///     Note*: Valid only on runtime calls
        /// </summary>
        DontInvokeLocalModify = 1 << 5,

        /// <summary>
        ///     Active: Don't apply table specific modified on drops
        ///     <para />
        ///     Default: Apply table specific modified on drops
        ///     <para />
        ///     Note*: Valid only on runtime calls
        /// </summary>
        DontInvokeLocalModified = 1 << 6,

        /// <summary>
        ///     Active: Don't apply table specific filters on drops
        ///     <para />
        ///     Default: Apply table specific filters on drops
        ///     <para />
        ///     Note*: Valid only on runtime calls
        /// </summary>
        DontInvokeTableFilter = 1 << 7
    }
}