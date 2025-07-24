using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class RoomData
{
    public IEnumerator<BlockData> GetEnumerator() => Grid.Cast<BlockData>().GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}