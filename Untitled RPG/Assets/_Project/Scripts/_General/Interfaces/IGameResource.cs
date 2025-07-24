using System;

public interface IGameResource
{
    public int Current { get; set; }
    public event Action<int, int> OnUpdatedCurrent;
}