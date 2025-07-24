public abstract partial class StatusEffectData
{
    public bool Equals (StatusEffectData other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return Kind == other.Kind;
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj.GetType() != GetType())
            return false;
        return Equals((StatusEffectData)obj);
    }

    public override int GetHashCode()
        => Kind.GetHashCode();

    public static bool operator == (StatusEffectData left, StatusEffectData right)
        => Equals(left, right);

    public static bool operator != (StatusEffectData left, StatusEffectData right)
        => !Equals(left, right);
}