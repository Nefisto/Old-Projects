public abstract partial class Modifier
{
    public bool Equals (Modifier other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Kind == other.Kind;
    }

    public override bool Equals (object obj)
    {
        if (obj is null)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((Modifier)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (int)Kind;
        }
    }

    public static bool operator == (Modifier left, Modifier right) => Equals(left, right);

    public static bool operator != (Modifier left, Modifier right) => !Equals(left, right);
}