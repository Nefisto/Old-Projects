using System;

public partial class PoisonEffectData
{
    public Action OnUpdatedStackAmount { get; set; }
    public int StackAmount { get; set; }
}