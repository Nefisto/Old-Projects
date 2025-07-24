using UnityEngine;

public interface ICard : IAction
{
    public string Name { get; }

    public Sprite Icon { get; }
}