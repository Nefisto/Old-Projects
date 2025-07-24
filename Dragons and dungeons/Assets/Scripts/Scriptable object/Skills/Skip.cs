using System.Collections;

// Used to skip turn
public class Skip : Skill
{
    private static Skip instance;

    public static Skip Instance
    {
        get
        {
            if (instance == null)
                instance = CreateInstance<Skip>();

            return instance;
        }
    }
    
    public override IEnumerator Act (BattleActionContext context)
    {
        yield return null;
    }
}