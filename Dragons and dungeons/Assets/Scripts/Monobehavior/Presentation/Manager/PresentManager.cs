using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class PresentManager : MonoBehaviour
{
    [Title("Control")]
    [SerializeField]
    private TextMeshProUGUI tmpNames;

    [SerializeField]
    private TextMeshProUGUI tmpPresents;

    [SerializeField]
    private TextMeshProUGUI tmpGameName;
    
    [Space]
    [SerializeField]
    private AssetReference cityScene;
    
    // private string names = "TsuDohNimh and Nefisto";
    // private string presentLabel = "presents...";
    // private string gameName = "Noblebright";
    
    private IEnumerator Start()
    {
        ClearTexts();
        yield return new WaitForSeconds(1f);

        yield return WriteRoutine(tmpNames, "TsuDohNimh and Nefisto", 2f);
        yield return WriteRoutine(tmpPresents, "presents...");
        yield return WriteRoutine(tmpGameName, "Noblebright");

        yield return FadeAll();

        yield return cityScene.LoadSceneAsync();
    }

    private void ClearTexts()
    {
        tmpNames.text = "";
        tmpPresents.text = "";
        tmpGameName.text = "";
    }

    private IEnumerator WriteRoutine (TextMeshProUGUI tmp, string phrase, float duration = 1f)
    {
        var timeForEachLetter = duration / phrase.Length;
        
        tmp.text = "";
        foreach (var letter in phrase)
        {
            tmp.text += letter;
            yield return new WaitForSeconds(timeForEachLetter);
        }
    }

    private IEnumerator FadeAll()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(tmpNames.DOFade(0f, .5f));
        sequence.Append(tmpPresents.DOFade(0f, .5f));
        sequence.Append(tmpGameName.DOFade(0f, .5f));

        yield return sequence.WaitForCompletion();
    }
}