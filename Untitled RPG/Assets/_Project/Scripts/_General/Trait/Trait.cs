using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public partial class Trait : IEquatable<Trait>
{
    [HideInInspector]
    [SerializeField]
    private int grow = 1;

    [HideInInspector]
    [SerializeField]
    private int points;

    [HideInInspector]
    [SerializeField]
    private AttributeType attributeType;

    public Trait (AttributeType type) => AttributeType = type;

    [ShowInInspector]
    [PropertyOrder(1)]
    [ReadOnly]
    public AttributeType AttributeType
    {
        get => attributeType;
        private set => attributeType = value;
    }

    [PropertyOrder(-1)]
    [MinValue(1)]
    [ShowInInspector]
    [HorizontalGroup("Status")]
    public int Grow
    {
        get => grow;
        protected set
        {
            grow = value;

            OnUpdatedGrow?.Invoke(grow);
        }
    }

    [ShowInInspector]
    [ReadOnly]
    [HideReferenceObjectPicker]
    [HorizontalGroup("Status")]
    public int Points
    {
        get => points;
        protected set => points = value;
    }

    [ShowInInspector]
    [Indent]
    [PropertyOrder(0)]
    [LabelWidth(150)]
    public int CompletedPoints => Points / GameConstants.PARTIAL_ATTRIBUTE_POINTS_AMOUNT;

    [ShowInInspector]
    [Indent]
    [PropertyOrder(0)]
    [LabelWidth(150)]
    public int PartialPoints => Points % GameConstants.PARTIAL_ATTRIBUTE_POINTS_AMOUNT;

    public void SetPointsToLevel (int level, int sectorGrow) => Points = (Grow + sectorGrow) * level;

    public void IncreaseGrow()
    {
        Assert.IsTrue(Grow < 10, "Trying to increase a max level grow");

        Grow++;
    }

    public void DecreaseGrow()
    {
        Assert.IsTrue(Grow > 1, "Trying to decrease a minimum level grow");

        Grow--;
    }

    public static implicit operator int (Trait trait) => trait.CompletedPoints;

    public void MultiplyGrow (float multiplier)
    {
        Grow = Mathf.RoundToInt(Grow * multiplier);
    }

    public event Action<int> OnUpdatedGrow;
}