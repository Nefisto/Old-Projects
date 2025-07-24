﻿using System.Collections.Generic;

namespace Loot.NTools
{
    /// <summary>
    ///     If observers change the table size while this is running things can get really bad
    /// </summary>
    public static partial class EventHandler
    {
        private static readonly Dictionary<string, List<InvokableActionBase>> globalEventTable =
            new Dictionary<string, List<InvokableActionBase>>();

        private static readonly Dictionary<object, Dictionary<string, List<InvokableActionBase>>> eventTable =
            new Dictionary<object, Dictionary<string, List<InvokableActionBase>>>();
    }
}