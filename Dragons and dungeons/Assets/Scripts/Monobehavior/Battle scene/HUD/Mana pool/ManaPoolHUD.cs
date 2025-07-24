using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class ManaPoolHUD : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private BattleActor battleActor;

    [Space]
    [SerializeField]
    private TextMeshProUGUI redManaValueLabel;

    [SerializeField]
    private TextMeshProUGUI greenManaValueLabel;

    [SerializeField]
    private TextMeshProUGUI blueManaValueLabel;
    
    private void Awake()
    {
        battleActor.OnUpdateMana += UpdateManaHUD;
    }

    private void UpdateManaHUD (ManaPool manaPool)
    {
        redManaValueLabel.text = manaPool.red.ToString();
        greenManaValueLabel.text = manaPool.green.ToString();
        blueManaValueLabel.text = manaPool.blue.ToString();
    }
}