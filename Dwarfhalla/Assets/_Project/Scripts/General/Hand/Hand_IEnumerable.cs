using System.Collections;
using System.Collections.Generic;

public partial class Hand
{
    public IEnumerator<ICard> GetEnumerator() => cards.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}