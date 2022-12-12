using System.Linq;
using UnityEngine;

public class SenseBlock : Block
{
    public System.Func<Vector3Int, PropType[]> sense;

    public override bool Evaluate()
    {
        var i = inputs.FirstOrDefault();
        if (i != null && i.value.TryGetValue(out Vector3 v))
        {
            value = new Value(sense?.Invoke(v.ToVector3Int()));
        }
        return false;
    }

    public override void Reset() 
    {
        value = new Value(null);
    }
}