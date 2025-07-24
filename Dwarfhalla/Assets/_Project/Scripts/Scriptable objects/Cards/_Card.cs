using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Card : ScriptableObject, ICard
{
    [field: TitleGroup("Settings")]
    [field: SerializeField]
    public int Price { get; set; } = 1;

    protected TurnContext TurnContext => ServiceLocator.GameContext.TurnContext;

    public ICard GetInstance => Instantiate(this);

    [field: TitleGroup("Settings")]
    [field: Range(1, 5)]
    [field: SerializeField]
    public int Cost { get; set; } = 1;

    [field: TitleGroup("Settings")]
    [field: PreviewField]
    [field: SerializeField]
    public Sprite Icon { get; set; }

    public abstract string Name { get; }
    public abstract bool CanBePerformed();
    public abstract void PreviewExecution();

    public abstract IEnumerator Perform (object context);
}