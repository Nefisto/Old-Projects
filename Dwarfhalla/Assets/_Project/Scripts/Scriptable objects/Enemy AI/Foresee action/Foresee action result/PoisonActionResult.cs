public class PoisonActionResult : ForeseeActionResult
{
    public bool willPoison = true;
    public override int Priority => willPoison ? 2 : 1;
}