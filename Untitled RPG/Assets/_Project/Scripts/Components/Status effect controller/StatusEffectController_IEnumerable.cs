using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class StatusEffectController
{
    public IEnumerator<StatusEffectData> GetEnumerator()
        => StatusEffectKindToTask
            .Values
            .Select(s => s.instance)
            .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}