using System.Collections.Generic;
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
        Queue<Node> front = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        List<Node> result = new List<Node>();

        while(front.Count > 0)
        {
            var cur = front.Dequeue();
        }

        return result;
    }

    public void Evaluate()
    {
        
    }
}

public enum NodeType
{
    Node, Conditional, Predicate
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
}

public abstract class Node
{
    public Value value;

    public Dictionary<string, Node> inputs = new Dictionary<string, Node>();
    public abstract void Evaluate();
}

public class Conditional : Node
{
    public Conditional()
    {
        inputs.Add("predicate", null);
        inputs.Add("A", null);
        inputs.Add("B", null);
    }

    public Predicate Predicate { get => (Predicate)inputs["predicate"];  }
    public Node IfTrue { get => inputs["A"]; }
    public Node IfFalse { get => inputs["B"]; }

    public override void Evaluate()
    {
        if (Predicate.value.TryGetValue(out bool b))
        {
            value = b ? IfTrue.value : IfFalse.value;
        } 
        else
        {
            value = IfFalse.value;
        }
        
    }
}

public class Predicate : Node
{
    public Predicate()
    {
        inputs.Add("A", null);
        inputs.Add("B", null);
    }

    public Node A { get => inputs["A"]; }
    public Node B { get => inputs["B"]; }
    public System.Func<Value, Value, bool> comparison;

    public override void Evaluate()
    {
        value = new Value(comparison(A.value, B.value));
    }
}