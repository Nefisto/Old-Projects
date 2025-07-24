using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = Nomenclature.EnemyStatusName, menuName = Nomenclature.EnemyStatusMenu, order = -10)]
public partial class EnemyData : ActorData
{
    [VerticalGroup("Basic/split/data")]
    [MinValue(1), MaxValue(20)]
    public int level = 1;

    public override int Level => level;
    
    [TabGroup("Data tab", "Data", true)]
    [Title("Grown curve")]
    [SerializeField]
    protected AnimationCurve health = new AnimationCurve();
    
    [TabGroup("Data tab", "Data")]
    [SerializeField]
    protected AnimationCurve strength = new AnimationCurve();
    
    [TabGroup("Data tab", "Data")]
    [SerializeField]
    protected AnimationCurve dexterity = new AnimationCurve();

    [TabGroup("Data tab", "Data")]
    [SerializeField]
    protected AnimationCurve intelligence = new AnimationCurve();

    [TabGroup("Data tab", "Data")]
    [Title("Status")]
    [ShowInInspector]
    private int MaxHealth => (int)health.Evaluate(Level);

    [TabGroup("Data tab", "Data")]
    [ShowInInspector]
    private int Strength => (int)strength.Evaluate(Level);

    [TabGroup("Data tab", "Data")]
    [ShowInInspector]
    private int Dexterity => (int)dexterity.Evaluate(Level);

    [TabGroup("Data tab", "Data")]
    [ShowInInspector]
    private int Intelligence => (int)intelligence.Evaluate(Level);

    public override Status GetBaseStatus()
        => Status.ConstructForEnemies(level, Strength, Dexterity, Intelligence);
    
    protected override ActorData CreateNewInstance => CreateInstance<EnemyData>();

    public override object Clone()
    {
        var enemyClone = (EnemyData) base.Clone();

        enemyClone.possibleMessages = possibleMessages.ToList();
        enemyClone.skills = skills.ToList();

        enemyClone.level = level;
        enemyClone.health = health;
        enemyClone.strength = strength;
        enemyClone.dexterity = dexterity;
        enemyClone.intelligence = intelligence;

        return enemyClone;
    }
}