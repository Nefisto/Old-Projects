using System;
using System.Collections;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using EventHandler = NTools.EventHandler;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [Title("Settings")]
    [SerializeField]
    private Deck deck;

    [SerializeField]
    private BackRowCards backRowCards;

    [SerializeField]
    private LeaderCardSlot leaderSlot;

    [Title("HUD references")]
    [SerializeField]
    private TMP_Text playerNameLabel;

    [SerializeField]
    private EventTrigger attackButton;

    [Title("Debug")]
    [HideLabel]
    [SerializeField]
    [ReadOnly]
    private MagickaResource magickaResource;

    [SerializeField]
    [ReadOnly]
    private int playerHealth;

    public Action<int> OnUpdatedLife;

    public Action<MagickaResource> OnUpdatedMagickaResource;

    private Deck runtimeDeck;

    public CardData CurrentLeader => leaderSlot.CardData;
    public string PlayerName => playerNameLabel.text;

    public MagickaResource MagickaResource
    {
        get => magickaResource;
        set
        {
            magickaResource = value;
            OnUpdatedMagickaResource?.Invoke(magickaResource);
        }
    }

    public int PlayerHealth
    {
        get => playerHealth;
        set
        {
            playerHealth = value;
            OnUpdatedLife?.Invoke(playerHealth);
        }
    }

    public void Shuffle()
        => runtimeDeck.Setup(deck.Shuffle());

    public IEnumerator DrawCardsIntoBackRowField()
    {
        yield return backRowCards.Setup(runtimeDeck.Take(5).ToArray());
    }

    public IEnumerator SelectLeader (string message = "Select a leader!")
    {
        EventHandler.RaiseEvent(this, GameEvents.CHANGE_FIELD_MESSAGE, message);
        CardSlot selectedSlot = null;
        yield return backRowCards.SelectLeader(ctx => selectedSlot = ctx.SelectedSlot);
        selectedSlot.Change(leaderSlot);
        Utilities.CreateLog("Leader selected", $"{PlayerName} has chosen {leaderSlot.CardName}!");
        EventHandler.RaiseEvent(this, GameEvents.CHANGE_FIELD_MESSAGE, string.Empty);
    }

    public IEnumerator SetupPlayerSide (string playerName)
    {
        yield return backRowCards.HideSlots();
        leaderSlot.ClearSlot();
        leaderSlot.HideCardHUD();
        MagickaResource = new MagickaResource();
        PlayerHealth = Random.Range(90, 120);
        playerNameLabel.text = playerName;
        EventHandler.RaiseEvent(this, GameEvents.CHANGE_FIELD_MESSAGE, string.Empty);
        HideAttackButton();
        runtimeDeck = ScriptableObject.CreateInstance<Deck>();
    }

    public bool HasDied()
    {
        if (playerHealth <= 0)
        {
            Utilities.CreateLog("End game!", $"{PlayerName} does not have more life to keep fighting.");
            return true;
        }

        if (!IsCurrentLeaderValidOrAlive() && backRowCards.NumberOfAliveCards == 0)
        {
            Utilities.CreateLog("End game!", $"{PlayerName} does not have more cards to fight.");
            return true;
        }

        return false;
    }

    private bool IsCurrentLeaderValidOrAlive()
        => CurrentLeader != null && CurrentLeader.IsAlive();

    // Turn actions
    public IEnumerator Act (ActionResult actionResult)
    {
        ShowAttackButton();
        EventHandler.RaiseEvent(this, GameEvents.CHANGE_FIELD_MESSAGE, "Chose a new leader OR attack.");

        var backRowAndAttackButtons = backRowCards.GetBackRowEventTriggers().ToList();
        backRowAndAttackButtons.Add(attackButton);

        var waitForUI = new WaitForUIEventTrigger(backRowAndAttackButtons);
        do
        {
            yield return waitForUI.Reset();

            if (waitForUI.PressedButton.TryGetComponent<BackRowCardSlot>(out var slot))
            {
                slot.Change(leaderSlot);
                EventHandler.RaiseEvent(this, GameEvents.CHANGE_FIELD_MESSAGE, "Attack.");
                Utilities.CreateLog("Leader changed!", $"{PlayerName} has selected a new leader, {leaderSlot.CardName}");
                waitForUI = new WaitForUIEventTrigger(attackButton);
            }
        } while (waitForUI.PressedButton == null);

        EventHandler.RaiseEvent(this, GameEvents.CHANGE_FIELD_MESSAGE, string.Empty);
        HideAttackButton();

        actionResult.Damage = CurrentLeader.BaseDamage;
        var attackMessage = $"{PlayerName} has used a basic attacked with {actionResult.Damage} power";
        if (CanAffordSkill())
        {
            MagickaResource = magickaResource.Reduce(CurrentLeader.Magicka);
            actionResult.Damage += CurrentLeader.Magicka.BonusDamage;
            attackMessage = $"{PlayerName} has used the {CurrentLeader.Magicka.Name} magicka and sharpened his attack to {actionResult.Damage}";
        }

        Utilities.CreateLog("Attack!", attackMessage);

        yield return new WaitForSeconds(1f);
        GenerateMagickaResource();
    }

    private bool CanAffordSkill()
        => magickaResource.CanAfford(CurrentLeader.Magicka);

    private void GenerateMagickaResource()
    {
        MagickaResource = MagickaResource.Generate(deck.MagickaScore, out var generatedAmount);
        Utilities.CreateLog("Generate magicka resource!", $"{PlayerName} has generated [{generatedAmount[0]}-{generatedAmount[1]}-{generatedAmount[2]}] magicka.", 4f);
    }

    private void ShowAttackButton()
        => attackButton.gameObject.SetActive(true);

    private void HideAttackButton()
        => attackButton.gameObject.SetActive(false);

    public void TakeDamage (int damage)
    {
        var overkillDamage = Mathf.Clamp(damage - CurrentLeader.BaseHealth, 0, int.MaxValue);
        CurrentLeader.TakeDamage(damage);
        Utilities.CreateLog("Take damage", $"{leaderSlot.CardName} has defended {damage - overkillDamage}");

        PlayerHealth -= overkillDamage;
        if (overkillDamage != 0)
            Utilities.CreateLog("Direct damage", $"{PlayerName} has taken {overkillDamage} direct damage!");
    }
}