using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LevelUpIconAnimation : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Image iconPrefab;

    [TitleGroup("References")]
    [ReadOnly]
    [SerializeField]
    private List<Image> cachedIcons;

    [TitleGroup("Debug")]
    [PreviewField]
    [ReadOnly]
    [SerializeField]
    private Sprite loadedLevelUpIcon;

    private void Awake() => GameEvents.onFinishedLoadingData += CacheChildren;

    [DisableInEditorButton]
    public void PlayTween()
    {
        var icon = GetIcon();

        var moveTween = ((RectTransform)icon.transform)
            .DOLocalMoveY(500f, 2f)
            .SetRelative(true);
        var fadeTween = icon
            .DOFade(0f, 1f);

        var mySequence = DOTween.Sequence();
        mySequence.Append(moveTween);
        mySequence.Join(fadeTween);
        mySequence.OnComplete(() => icon.gameObject.SetActive(false));

        mySequence.Play();
    }

    private Image GetIcon()
    {
        var validIcon = cachedIcons.FirstOrDefault(image => image.gameObject.activeInHierarchy == false);

        if (validIcon == null)
            ExpandCachedIcons();

        validIcon = cachedIcons.FirstOrDefault(image => image.gameObject.activeInHierarchy == false);
        Assert.IsNotNull(validIcon);

        validIcon.gameObject.SetActive(true);
        validIcon.sprite = loadedLevelUpIcon;

        ((RectTransform)validIcon.transform).anchoredPosition = Vector2.zero;
        validIcon.color = new Color
        {
            r = validIcon.color.r,
            g = validIcon.color.g,
            b = validIcon.color.b,
            a = 1
        };

        return validIcon;
    }

    private void ExpandCachedIcons()
    {
        for (var i = 0; i < 10; i++)
        {
            var instance = Instantiate(iconPrefab, transform, false);

            cachedIcons.Add(instance);
            instance.gameObject.SetActive(false);
        }
    }

    private void CacheChildren()
    {
        loadedLevelUpIcon = Database.GameIcons.GameplayIcons.GetIconOfKind(GameplayIcons.LevelUp);

        cachedIcons.Clear();
        foreach (Transform child in transform)
        {
            var instance = child.GetComponent<Image>();

            cachedIcons.Add(instance);
            instance.gameObject.SetActive(false);
            instance.sprite = loadedLevelUpIcon;
        }
    }
}