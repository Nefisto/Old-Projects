using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Loot
{
    public partial class Bag
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Loot this [int i] => Loot[i];

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerator<Loot> GetEnumerator()
            => Loot.GetEnumerator();

        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool Equals (Bag other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (EntryCount != other.EntryCount)
                return false;

            using (var enumerator = GetEnumerator())
            {
                using (var otherEnumerator = other.GetEnumerator())
                {
                    while (enumerator.MoveNext() && otherEnumerator.MoveNext())
                    {
                        var a = enumerator.Current;
                        var b = otherEnumerator.Current;

                        if (a != b)
                            return false;
                    }
                }
            }

            return true;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals (object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Bag)obj);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            var hash = 0;
            if (Loot != null)
            {
                hash = 17 + EntryCount;
                foreach (var loot in Loot)
                {
                    hash *= 17;
                    if (loot != null)
                        hash += loot.GetHashCode();
                }
            }

            return hash;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool operator == (Bag left, Bag right)
            => Equals(left, right);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static bool operator != (Bag left, Bag right)
            => !Equals(left, right);
    }
}