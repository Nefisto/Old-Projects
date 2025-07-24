using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class LevelData
{
    public int CurrentLevelIndex { get; set; }

    public RoomData CurrentRoom => GetRoomAtIndex(CurrentLevelIndex);

    [ShowInInspector]
    [HideReferenceObjectPicker]
    public List<RoomData> Rooms { get; set; }

    public RoomData GetRoomAtIndex (int i) => Rooms[i];
}