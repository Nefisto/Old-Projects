public class GameContext
{
    public int CurrentTurn { get; set; } = 1;
    public PlayerData PlayerData { get; set; }
    public EnemyData EnemyData { get; set; }
    public TurnContext TurnContext { get; set; } = new();
    public LevelData LevelData { get; set; }
}