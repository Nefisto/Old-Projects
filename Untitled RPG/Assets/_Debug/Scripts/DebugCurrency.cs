using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugCurrency : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private TMP_InputField inputForCurrencyAmount;

    [TitleGroup("References")]
    [SerializeField]
    private Button removeButton;

    [TitleGroup("References")]
    [SerializeField]
    private Button addButton;

    private void Start()
    {
        var currencyAmount = int.Parse(inputForCurrencyAmount.text);

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(()
            => ServiceLocator.SessionManager.PlayableCharacterData.RemoveCurrency(currencyAmount));

        addButton.onClick.RemoveAllListeners();
        addButton.onClick.AddListener(()
            => ServiceLocator.SessionManager.PlayableCharacterData.AddCurrency(currencyAmount));
    }
}