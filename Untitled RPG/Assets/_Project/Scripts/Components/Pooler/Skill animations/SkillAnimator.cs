using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer),
    typeof(Animator))]
public class SkillAnimator : MonoBehaviour
{
    [TitleGroup("References")]
    [SerializeField]
    private Animator animator;

    [TitleGroup("References")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public IEnumerator Play (string animationName, PlayAnimationSettings settings = null)
    {
        gameObject.SetActive(true);

        settings ??= new PlayAnimationSettings();

        var animatorTransform = transform;
        spriteRenderer.flipX = settings.flipX;
        spriteRenderer.flipY = settings.shouldFlipYForPlayer;
        animator.speed = settings.animationSpeed;
        animatorTransform.position += new Vector3(settings.offsetX, settings.offsetY, 1f);
        animatorTransform.localScale = new Vector3(settings.size, settings.size, 1f);

        animator.Play(animationName, 0, 0f);
        // When I do a normalization of 1, it runs the first sprite of the animation sometimes, so making it stop
        //  a little early solved the issue
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .95f)
            yield return null;

        gameObject.SetActive(false);
    }
}