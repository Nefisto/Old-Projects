using System.Collections;
using DG.Tweening;
using NTools;
using UnityEngine;

public partial class CoinAnimation
{
    public IEnumerator Animate (object settings = null)
    {
        var correctSettings = settings as Settings ?? new Settings();

        modelFolder.localScale = Vector3.zero;
        amountLabel.text = "";
        amountLabel.color = amountLabel.color.SetAlpha(0f);

        modelFolder
            .DORotate(new Vector3(0f, 360f, 0f), timeForAFullRotation, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1);

        yield return DOTween.Sequence()
            .Append(modelFolder.DOScale(Vector3.one, growingDuration))
            .AppendCallback(() => amountLabel.text = $"+ {correctSettings.coinAmount}")
            .Join(amountLabel.DOFade(1f, 1f))
            .AppendInterval(1.5f)
            .Append(amountLabel.DOFade(0f, 1f))
            .Append(modelFolder.DOScale(Vector3.zero, growingDuration))
            .WaitForCompletion();

        DOTween.Kill(modelFolder);
    }

    public class Settings
    {
        public int coinAmount = 1;
    }
}