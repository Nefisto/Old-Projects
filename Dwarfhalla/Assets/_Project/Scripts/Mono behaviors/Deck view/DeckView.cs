using System.Collections;
using System.Collections.Generic;
using NTools;
using QFSW.QC;
using Sirenix.OdinInspector;
using UnityEngine;

public class DeckView : MonoBehaviour
{
    [TitleGroup("Settings")]
    [SerializeField]
    private UnitSide deckSide = UnitSide.Dwarf;

    [TitleGroup("References")]
    [SerializeField]
    private Transform unitFolder;

    [TitleGroup("Debug")]
    [ShowInInspector]
    public NDictionary<UnitKind, List<GameObject>> kindToUnits = new();

    private void Awake() => GameEntryPoints.OnSetupScene += _ => SetupDeck();

    [Command(CommandConstants.BASE_PATH + "Deck.Refresh", "Refresh both decks", MonoTargetType.All)]
    private void SetupDeck()
    {
        unitFolder.DestroyChildren();

        var deck = deckSide == UnitSide.Dwarf
            ? ServiceLocator.GameContext.PlayerData.GetAllSummonCards
            : ServiceLocator.GameContext.EnemyData.GetAllSummonCards;

        RegisterListeners(deck);

        var position = Vector3.zero;
        var zDirection = deckSide == UnitSide.Dwarf ? 1 : -1;
        foreach (var card in deck)
        {
            var model = ServiceLocator.Database.GetModelOf(card.UnitData.UnitKind);
            var instance = Instantiate(model, unitFolder);
            instance.transform.localPosition = new Vector3(position.x % 5, position.y, position.z);
            instance.transform.rotation = Quaternion.Euler(0f, -135f, 0f);

            position.x += 1;
            position.z = ((int)position.x / 5) * zDirection;

            kindToUnits.TryAdd(card.UnitData.UnitKind, new List<GameObject>());
            kindToUnits[card.UnitData.UnitKind].Add(instance);
        }
    }

    private void RegisterListeners (List<SummonCard> deck)
    {
        foreach (var summonCard in deck)
        {
            summonCard.OnSummoningFromCard -= SummoningHandle;
            summonCard.OnSummoningFromCard += SummoningHandle;
        }
    }

    private IEnumerator SummoningHandle (object arg)
    {
        var card = arg as SummonCard;
        card.OnSummoningFromCard -= SummoningHandle;

        var foundUnit = kindToUnits[card.UnitData.UnitKind].GetRandom();
        kindToUnits[card.UnitData.UnitKind].Remove(foundUnit);

        Destroy(foundUnit);
        yield return new WaitForSeconds(0.5f);
    }
}