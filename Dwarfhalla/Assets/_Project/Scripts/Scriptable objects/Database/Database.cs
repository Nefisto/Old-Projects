using System;
using System.Collections.Generic;
using System.Linq;
using NTools;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Database", menuName = EditorConstants.BASE_PATH + "Database", order = 0)]
public class Database : ScriptableObject
{
    [field: TitleGroup("References")]
    [field: SerializeField]
    public List<Card> Cards { get; set; }
    
    [field: TitleGroup("References")]
    [field: SerializeField]
    public List<UnitData> Dwarfs { get; set; }
    
    [field: TitleGroup("References")]
    [field: SerializeField]
    public List<UnitData> Goblins { get; set; }

    [field: TitleGroup("References")]
    [field: SerializeField]
    public NDictionary<UnitKind, GameObject> UnitKindToModel { get; set; } = new();
    
    public ICard GetCardFromUnit (UnitData unit)
        => Cards
            .Where(c => c is SummonCard)
            .Cast<SummonCard>()
            .First(sc => sc.UnitData.Name == unit.Name);
    
    public GameObject GetModelOf (UnitKind unitKind) => UnitKindToModel[unitKind];
}