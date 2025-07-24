using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Base class for all our skills
/// </summary>
public abstract class Skill : ScriptableObject
{
    [Title("Common settings")]
    [Tooltip("Image that will appear in UI")]
    [PreviewField]
    [Required]
    public Sprite icon;

    [Tooltip("Which actor(s) should this skill target?")]
    public SkillGroupTarget groupTarget = SkillGroupTarget.Single;

    [Tooltip("Amount of targets that this skill require")]
    [ShowIf("@groupTarget == SkillGroupTarget.Selectable")]
    [MinValue(1), MaxValue(8)]
    public int maxAmountOfTargets = 1;
    
    [Tooltip("Modifier that this skill will apply")]
    public ModifierSO modifierToApply;

    [Title("Cost")]
    [ProgressBar(0, 10, Segmented = true, ColorGetter = "GetRedManaColorBar")]
    [SerializeField]
    private short red;

    [ProgressBar(0, 10, Segmented = true, ColorGetter = "GetGreenManaColorBar")]
    [SerializeField]
    private short green;

    [ProgressBar(0, 10, Segmented = true, ColorGetter = "GetBlueManaColorBar")]
    [SerializeField]
    private short blue;
    
    /// <summary>
    /// This will vary based on each kind of skills
    /// </summary>
    /// <param name="context"></param>
    public virtual IEnumerator Act (BattleActionContext context)
    {
        modifierToApply?.Act(context);

        yield return null;
    }

    protected IEnumerator SkillDelay (float amount)
    {
        yield return new WaitForSeconds(amount);
    }

    public ManaPool GetSkillCost()
        => new ManaPool()
        {
            red = red,
            green = green,
            blue = blue
        };

    private Color GetRedManaColorBar()
        => Color.red;
    
    private Color GetGreenManaColorBar()
        => Color.green;

    private Color GetBlueManaColorBar()
        => Color.blue;
}