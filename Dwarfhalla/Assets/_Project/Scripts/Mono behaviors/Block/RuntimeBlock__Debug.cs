using NTools;
using Sirenix.OdinInspector;

public partial class RuntimeBlock
{
    [TitleGroup("Operations")]
    [Button]
    private void Test_Notify (NotificationType notificationType, NotificationSettings notificationSettings = null)
    {
        notifyRoutine?.Stop();

        notifyRoutine = new NTask(InternalNotify(notificationType));
    }
}