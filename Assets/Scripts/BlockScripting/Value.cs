using System;

public class Value
{
    public object value;

    public Value(object value)
    {
        this.value = value;
    }

    public bool TryGetValue<T>(out T value)
    {
        if (this.value is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }

    public static bool operator >(Value a, Value b)
    {
        var valueA = a.value;
        var valueB = b.value;

        if (valueA.GetType() == valueB.GetType() && valueA is IComparable ac)
        {
            return ac.CompareTo(b) > 0;
        }

        return false;
    }

    public static bool operator <(Value a, Value b)
    {
        var valueA = a.value;
        var valueB = b.value;

        if (valueA.GetType() == valueB.GetType() && valueA is IComparable ac)
        {
            return ac.CompareTo(b) < 0;
        }

        return false;
    }

    public static bool operator >=(Value a, Value b)
    {
        var valueA = a.value;
        var valueB = b.value;

        if (valueA.GetType() == valueB.GetType() && valueA is IComparable ac)
        {
            return ac.CompareTo(b) >= 0;
        }

        return false;
    }

    public static bool operator <=(Value a, Value b)
    {
        var valueA = a.value;
        var valueB = b.value;

        if (valueA.GetType() == valueB.GetType() && valueA is IComparable ac)
        {
            return ac.CompareTo(b) <= 0;
        }

        return false;
    }

    public static bool operator ==(Value a, Value b)
    {
        var valueA = a.value;
        var valueB = b.value;

        if (valueA.GetType() == valueB.GetType() && valueA is IComparable ac)
        {
            return ac.CompareTo(b) == 0;
        }

        return false;
    }

    public static bool operator !=(Value a, Value b)
    {
        var valueA = a.value;
        var valueB = b.value;

        if (valueA.GetType() == valueB.GetType() && valueA is IComparable ac)
        {
            return ac.CompareTo(b) != 0;
        }

        return false;
    }
}
