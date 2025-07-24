public static class Blackboard
{
    public static GameInfo GameInfo { get; set; } = new();

    public static Skill CurrentChargingSkill { get; set; }
}