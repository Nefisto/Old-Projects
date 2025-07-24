using System.Collections;
using System.Collections.Generic;

public partial class Deck
{
    public IEnumerator<ICard> GetEnumerator()
    {
        return cards.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}