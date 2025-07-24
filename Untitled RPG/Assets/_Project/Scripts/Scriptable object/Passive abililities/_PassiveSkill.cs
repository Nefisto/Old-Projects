using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class SetupPassiveSkillContext
{
    public BattleActor owner;
}

public abstract class PassiveSkill : ScriptableObject
{
    [TitleGroup("Settings")]
    [SerializeField]
    [Multiline]
    private string information;

    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public virtual string Name { get; set; }

    [TitleGroup("Debug")]
    [ReadOnly]
    [SerializeField]
    protected BattleActor owner;

    public virtual PassiveSkill GetInstance => Instantiate(this);

    public virtual void Setup (SetupPassiveSkillContext context)
    {
        owner = context.owner;
    }

    public abstract IEnumerator Register();
    public abstract IEnumerator Remove();

    public virtual IEnumerator Feedback()
    {
        yield return null;
    }
}