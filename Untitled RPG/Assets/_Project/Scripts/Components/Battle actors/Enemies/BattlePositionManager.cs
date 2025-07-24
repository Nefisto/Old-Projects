using System;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

public enum BattleLinePosition
{
    Front,
    Back
}

public class BattlePositionManager : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private NDictionary<BattleLinePosition, BattlePositionSettings> battleLineToSettings;

    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer actorSprite;

    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer ground;

    public void SetupBattleLinePosition (BattleLinePosition linePosition)
    {
        var settings = battleLineToSettings[linePosition];

        transform.localScale = new Vector3(settings.spriteSize, settings.spriteSize, 1f);
        actorSprite.sortingOrder = settings.sortingOrder;

        var c = ground.color;
        c.a = settings.groundAlpha;
        ground.color = c;
    }

    [Serializable]
    public class BattlePositionSettings
    {
        [Range(.5f, 1.5f)]
        public float spriteSize = 1f;

        [Range(0f, 1f)]
        public float groundAlpha = 1f;

        public int sortingOrder;
    }
}