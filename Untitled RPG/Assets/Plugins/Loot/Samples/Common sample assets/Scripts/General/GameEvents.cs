using System;
using Loot;

namespace Sample
{
    public static class GameEvents
    {
        public static Action<UpdateScrollViewArguments> OnUpdateScrollView;
        public static Action<UpdateHeaderArguments> OnUpdateHeader;

        public static event Action<DropTable> OnUpdateDropTableDrawer;
        public static event Action<string> OnUpdateSampleInfo;

        public static event Action<LogEntryContext> OnUpdateLog;

        public static void RaiseUpdateDropTableDrawer (DropTable table)
            => OnUpdateDropTableDrawer?.Invoke(table);

        public static void RaiseUpdateSampleInfo (string obj)
            => OnUpdateSampleInfo?.Invoke(obj);

        public static void RaiseUpdateLog (LogEntryContext obj)
            => OnUpdateLog?.Invoke(obj);
    }
}