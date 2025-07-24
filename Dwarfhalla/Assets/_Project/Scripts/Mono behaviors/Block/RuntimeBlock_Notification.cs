using System.Collections;
using UnityEngine;

public partial class RuntimeBlock
{
    private NotificationType cachedNotification;
    private NotificationType currentNotification;

    private IEnumerator InternalNotify (NotificationType notificationType)
    {
        ShowNotifier();

        var notificationSettings = notifyTypeToColorGradient[notificationType];

        notifierRenderer.material.mainTexture = notificationSettings.texture;

        var gradient = notificationSettings.gradient;
        var counter = 0f;
        var isGrowing = true;
        while (true)
        {
            counter += Time.deltaTime * notificationSettings.blinkingSpeed * (isGrowing ? 1f : -1f);

            switch (counter)
            {
                case > 1f:
                    counter = 1f;
                    isGrowing = false;
                    break;

                case < 0f:
                    counter = 0f;
                    isGrowing = true;
                    break;
            }

            notifierRenderer.material.color = gradient.Evaluate(counter);
            yield return null;
        }
    }

    private void HideNotifier() => notifierRenderer.gameObject.SetActive(false);
    private void ShowNotifier() => notifierRenderer.gameObject.SetActive(true);
}