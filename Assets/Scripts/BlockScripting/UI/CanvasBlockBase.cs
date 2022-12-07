
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public abstract class CanvasBlockBase : MonoBehaviour
{
    public Block Block { get; set; }
    public List<Transform> input_nodes;
    public List<Transform> output_nodes;

    public Func<Vector3, CanvasBlockBase> queryTile;

    CanvasBlockBase[] InputBlock
    {
        get
        {
            if (input_nodes.Count > 0)
                return input_nodes.Select(x => queryTile(x.transform.position)).ToArray();
            return new CanvasBlockBase[0];
        }
    }
    CanvasBlockBase[] OutputBlock
    {
        get
        {
            if (output_nodes.Count > 0)
                return output_nodes.Select(x => queryTile(x.transform.position)).ToArray();
            return new CanvasBlockBase[0];
        }
    }

    Block[] Inputs
    {
        get
        {
            var tmp = InputBlock;
            if (tmp.Length > 0)
                return tmp.Select(x => x != null ? x.Block : null).ToArray();
            return new Block[0];
        }
    }
    Block[] Outputs
    {
        get
        {
            var tmp = OutputBlock;
            if (tmp.Length > 0)
                return tmp.Select(x => x != null ? x.Block : null).ToArray();
            return new Block[0];
        }

    }

    public abstract void Begin(CanvasGraph cg);

    public void RefreshIO()
    {
        Block.inputs = Inputs.ToList();
        switch (Block)
        {
            case ActionBlock action:
                action.next = Outputs.FirstOrDefault();
                break;
            case BranchBlock branch:
                branch.ifTrue = Outputs.Count() > 0 ? Outputs[0] : null;
                branch.ifFalse = Outputs.Count() > 1 ? Outputs[1] : null;
                break;
            case InputBlock input:
                input.next = Outputs.FirstOrDefault();
                break;
            case PredicateBlock predicate:
                predicate.next = Outputs.FirstOrDefault();
                break;
            case StartBlock start:
                start.next = Outputs.FirstOrDefault();
                break;
        }
    }
}