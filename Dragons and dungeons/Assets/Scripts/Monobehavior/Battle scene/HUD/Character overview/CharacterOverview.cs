using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterOverview : MonoBehaviour
{
    [Title("References")]
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI characterName;

    [SerializeField]
    private TextMeshProUGUI life;

    [SerializeField]
    private TextMeshProUGUI strength;

    [SerializeField]
    private TextMeshProUGUI dexterity;
    
    [SerializeField]
    private TextMeshProUGUI intelligence;

    private void Start()
        => BattleManager.Instance.OnSpawnActors += RegisterOnAlliesStartTurn;

    private void SetupPanel(BattleActor actor)
    {
        var actorInfo = actor.Data;
        var baseStatus = actorInfo.GetBaseStatus();
        
        icon.sprite = actorInfo.icon;
        characterName.text = actorInfo.name;

        life.text = $"{actor.CurrentLife} / {baseStatus.MaxHealth}";
        strength.text = $"{baseStatus.Strength}";
        dexterity.text = $"{baseStatus.Dexterity}";
        intelligence.text = $"{baseStatus.Intelligence}";
    }

    private void RegisterOnAlliesStartTurn()
    {
        var allies = BattleManager.Instance.GetPlayerCharacters();

        foreach (var battleActor in allies)
            battleActor.OnStartTurn += _ => SetupPanel(battleActor);
    }
}