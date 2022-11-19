using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph
{
    public List<Node> nodes;
    public List<Node> ordered;
    public Graph(List<Node> nodes)
    {
        this.nodes = nodes;
        ordered = GetExecOrder();
    }

    public List<Node> GetExecOrder()
    {
        Queue<Node> front = new();
        HashSet<Node> done = new();
        List<Node> result = new();
        int maxAttempts = 10000;
        int attempt = 0;

        foreach (var node in nodes)
            front.Enqueue(node);

        while(front.Count > 0 && attempt < maxAttempts)
        {
            var cur = front.Dequeue();
            if (done.Contains(cur))
                continue;

            var inputs = cur.inputs;
            if (inputs.Count == 0 || inputs.All(x => done.Contains(x.Value)))
            {
                done.Add(cur);
                result.Add(cur);
            } 
            else
            {
                front.Enqueue(cur);
            }

            attempt++;
        }

        return result;
    }

    public void Evaluate()
    {
        foreach (var node in ordered)
        {
            node.Evaluate();
        }
    }
}

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

public abstract class Node
{
    public Value value;

    public Dictionary<string, Node> inputs = new();
    public abstract void Evaluate();
}

public class Branch : Node
{
    public Branch()
    {
        inputs.Add("predicate", null);
        inputs.Add("A", null);
        inputs.Add("B", null);
    }

    public Predicate Predicate { get => (Predicate)inputs["predicate"]; set => inputs["predicate"] = value; }
    public Node IfTrue { get => inputs["A"]; set => inputs["A"] = value; }
    public Node IfFalse { get => inputs["B"]; set => inputs["B"] = value; }

    public override void Evaluate()
    {
        if (Predicate != null && Predicate.value.TryGetValue(out bool b))
        {
            value = b ? IfTrue.value : IfFalse.value;
        } 
        else
        {
            value = IfFalse != null ? IfFalse.value : IfTrue != null ? IfTrue.value : new Value(null);
        }
        
    }
}

public enum Comparison
{
    Greater, GreaterOrEqual, Equal, LessOrEqual, Less, NotEqual
}

public class Predicate : Node
{
    readonly Dictionary<Comparison, Func<Value, Value, bool>> comparisons = new() 
    { 
        { Comparison.Greater,           (a, b) => a > b },
        { Comparison.GreaterOrEqual,    (a, b) => a >= b },
        { Comparison.Equal,             (a, b) => a == b },
        { Comparison.LessOrEqual,       (a, b) => a <= b },
        { Comparison.Less,              (a, b) => a < b },
        { Comparison.NotEqual,          (a, b) => a != b },
    };

    public Predicate()
    {
        inputs.Add("A", null);
        inputs.Add("B", null);
    }

    public Node A { get => inputs["A"]; set => inputs["A"] = value; }
    public Node B { get => inputs["B"]; set => inputs["B"] = value; }
    public Comparison comparison;

    public override void Evaluate()
    {
        value = new Value(comparisons[comparison](A.value, B.value));
    }
}