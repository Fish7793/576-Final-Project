using System.Collections.Generic;
using UnityEngine;

public class PredicateBlock : Block
{
    readonly Dictionary<Comparison, System.Func<Value, Value, bool>> comparisons = new()
    {
        { Comparison.Greater,           (a, b) => a > b },
        { Comparison.GreaterOrEqual,    (a, b) => a >= b },
        { Comparison.Equal,             (a, b) => a == b },
        { Comparison.LessOrEqual,       (a, b) => a <= b },
        { Comparison.Less,              (a, b) => a < b },
        { Comparison.NotEqual,          (a, b) => a != b },
    };

    public Block a;
    public Block b;
    public Comparison comparison;

    public override bool Evaluate()
    {
        value = new Value(comparisons[comparison](a != null ? a.value : new Value(null), b != null ? b.value : new Value(null)));
        return false;
    }

    public override void Reset()
    {
        value = new Value(false);
    }
}