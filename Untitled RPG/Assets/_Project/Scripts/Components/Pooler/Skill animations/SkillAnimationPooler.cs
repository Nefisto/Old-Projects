using System.Collections.Generic;
using _Project.Scripts._General.Enum;

public class SkillAnimationPooler : SerializedMonobehaviourPooler<SkillAnimator>
{
    private void Awake() => ServiceLocator.SkillVFX = this;

    public SkillAnimator GetSkillAnimator() => GetPooledObject();

    public List<SkillAnimator> GetMultipleSkillAnimator (int amountOfTargets)
        => GetMultiplePooledObjects(amountOfTargets);

    public void ReturnSkillAnimator (SkillAnimator skillAnimator)
        => skillAnimator.transform.SetParent(transform, false);

    [DisableInEditorButton]
    public void Play (SkillAnimationType animationTypeType)
    {
        var skillAnim = GetSkillAnimator();
        StartCoroutine(skillAnim.Play(animationTypeType.ToString()));
    }
}