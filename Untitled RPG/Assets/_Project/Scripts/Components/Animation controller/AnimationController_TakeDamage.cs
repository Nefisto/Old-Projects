using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public partial class AnimationController
{
    private static readonly int HitEffectBlend = Shader.PropertyToID("_HitEffectBlend");

    [TitleGroup("Settings", "Take damage")]
    [SerializeField]
    private float hitEffectBlend = .05f;

    [TitleGroup("Settings", "Take damage")]
    [LabelText("Effect duration")]
    [SerializeField]
    private float takeDamageEffectDuration = .2f;

    private float takeDamageCounter;
    private IEnumerator takeDamageRoutine;

    public IEnumerator TakeDamageAnimation()
    {
        if (takeDamageRoutine != null)
        {
            SpriteRenderer.material.SetColor("_HitEffectColor", Color.red);
            takeDamageCounter = 0;
            yield return takeDamageRoutine;
        }

        takeDamageRoutine = _TakeDamageAnimation();

        StartCoroutine(takeDamageRoutine);

        IEnumerator _TakeDamageAnimation()
        {
            SpriteRenderer.material.SetColor("_HitEffectColor", Color.red);

            SpriteRenderer.material.SetFloat(HitEffectBlend, hitEffectBlend);
            while (takeDamageCounter < takeDamageEffectDuration)
            {
                takeDamageCounter += Time.deltaTime;
                yield return null;
            }

            SpriteRenderer.material.SetFloat(HitEffectBlend, 0f);
        }
    }
}