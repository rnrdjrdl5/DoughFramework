using System;

public class Identity : IIdentifiable, IEquatable<Identity>
{
    public string Id { get; }

    public Identity()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    public override string ToString() => Id;
    
    public bool Equals(Identity other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        
        return string.Equals(Id, other.Id, StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
        return obj is Identity other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Id != null ? StringComparer.Ordinal.GetHashCode(Id) : 0;
    }

    public static bool operator ==(Identity left, Identity right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
        {
            return false;
        }
        
        return left.Equals(right);
    }

    public static bool operator !=(Identity left, Identity right) => !(left == right);
}
