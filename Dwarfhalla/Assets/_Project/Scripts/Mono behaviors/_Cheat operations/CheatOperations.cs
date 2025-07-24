using Sirenix.OdinInspector;
using UnityEngine;

public class CheatOperations : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Transform goblinSummonCards;

    [TitleGroup("References")]
    [SerializeField]
    private Transform dwarfSummonCards;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            goblinSummonCards.gameObject.SetActive(!goblinSummonCards.gameObject.activeSelf);
            dwarfSummonCards.gameObject.SetActive(!dwarfSummonCards.gameObject.activeSelf);
        }
    }
}