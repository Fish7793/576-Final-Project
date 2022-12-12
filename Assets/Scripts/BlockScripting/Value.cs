using System;
using System.Linq;
using UnityEngine;

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
        var valueA = a?.value;
        var valueB = b?.value;

        if (valueA?.GetType() == valueB?.GetType() && valueA is IComparable ac && valueB is IComparable bc)
        {
            return ac.CompareTo(bc) > 0;
        }

        return false;
    }

    public static bool operator <(Value a, Value b)
    {
        var valueA = a?.value;
        var valueB = b?.value;

        if (valueA?.GetType() == valueB?.GetType() && valueA is IComparable ac && valueB is IComparable bc)
        {
            return ac.CompareTo(bc) < 0;
        }

        return false;
    }

    public static bool operator >=(Value a, Value b)
    {
        var valueA = a?.value;
        var valueB = b?.value;

        if (valueA?.GetType() == valueB?.GetType() && valueA is IComparable ac && valueB is IComparable bc)
        {
            return ac.CompareTo(bc) >= 0;
        }

        return false;
    }

    public static bool operator <=(Value a, Value b)
    {
        var valueA = a?.value;
        var valueB = b?.value;

        if (valueA?.GetType() == valueB?.GetType() && valueA is IComparable ac && valueB is IComparable bc)
        {
            return ac.CompareTo(bc) <= 0;
        }

        return false;
    }

    public static bool operator ==(Value a, Value b)
    {
        var valueA = a?.value;
        var valueB = b?.value;
        if (valueA?.GetType() == valueB?.GetType() && valueA is IComparable ac && valueB is IComparable bc)
        {
            return ac.CompareTo(bc) == 0;
        }

        if (valueA is PropType[] pla && valueB is PropType pb)
        {
            return pla.Contains(pb);
        }

        if (valueB is PropType[] plb && valueA is PropType pa)
        {
            return plb.Contains(pa);
        }

        return false;
    }

    public static bool operator !=(Value a, Value b)
    {
        var valueA = a?.value;
        var valueB = b?.value;

        if (valueA?.GetType() == valueB?.GetType() && valueA is IComparable ac && valueB is IComparable bc)
        {
            return ac.CompareTo(bc) != 0;
        }

        if (valueA is PropType[] pla && valueB is PropType pb)
        {
            return !pla.Contains(pb);
        }

        if (valueB is PropType[] plb && valueA is PropType pa)
        {
            return !plb.Contains(pa);
        }

        return false;
    }
}
