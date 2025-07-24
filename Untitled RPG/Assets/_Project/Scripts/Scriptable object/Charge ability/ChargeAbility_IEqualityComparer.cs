using System.Collections.Generic;

public abstract partial class ChargeAbility
{
    public class ChargeAbilityComparer : IEqualityComparer<ChargeAbility>
    {
        public bool Equals (ChargeAbility x, ChargeAbility y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null))
                return false;
            if (ReferenceEquals(y, null))
                return false;
            if (x.GetType() != y.GetType())
                return false;
            return x.IconKind == y.IconKind;
        }

        public int GetHashCode (ChargeAbility obj) => (int)obj.IconKind;
    }
}