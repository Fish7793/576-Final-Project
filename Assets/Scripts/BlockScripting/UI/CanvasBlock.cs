using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanvasBlock : MonoBehaviour
{
    public BlockType blockType;
    public ActionType actionType;
    public Block Block { get; set; }
    public List<Transform> input_nodes;
    public List<Transform> output_nodes;

    public Func<Vector3, CanvasBlock> queryTile;

    CanvasBlock[] InputBlock 
    { 
        get 
        { 
            if (input_nodes.Count > 0)
                return  input_nodes.Select(x => queryTile(x.transform.position)).ToArray();
            return new CanvasBlock[0];
        } 
    }
    CanvasBlock[] OutputBlock 
    {
        get
        {
            if (output_nodes.Count > 0)
                return output_nodes.Select(x => queryTile(x.transform.position)).ToArray();
            return new CanvasBlock[0];
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

    public void Begin(CanvasGraph cg)
    {
        switch (blockType)
        {
            case BlockType.ActionBlock:
                var action = new ActionBlock();
                switch (actionType)
                {
                    case ActionType.None:
                        break;
                    case ActionType.MoveForward:
                        action.action = () => cg.Agent.Move(); 
                        break;
                    case ActionType.RotateLeft:
                        action.action = () => cg.Agent.Rotate(-90);
                        break;
                    case ActionType.RotateRight:
                        action.action = () => cg.Agent.Rotate(90);
                        break;
                    case ActionType.Jump:
                        action.action = () => cg.Agent.Jump();
                        break;
                    case ActionType.Attack:
                        action.action = () => cg.Agent.Attack();
                        break;
                }
                Block = action;
                break;
            case BlockType.ActionBlockContainer:
                Block = new ActionBlockContainer();
                break;
            case BlockType.BranchBlock:
                Block = new BranchBlock();
                break;
            case BlockType.InputBlock:
                Block = new InputBlock();
                break;
            case BlockType.PredicateBlock:
                Block = new PredicateBlock();
                break;
            case BlockType.StartBlock: 
                Block = new StartBlock();
                break;
            case BlockType.SenseBlock:
                var b = new SenseBlock();
                b.sense = (v) => cg.Agent.Sense(v).FirstOrDefault();
                Block = b;
                break;
        }
        print("Refreshing " + Block);
    }

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

public enum BlockType
{
    StartBlock, ActionBlock, ActionBlockContainer, BranchBlock, InputBlock, PredicateBlock, SenseBlock
}

public enum ActionType
{
    None, MoveForward, RotateLeft, RotateRight, Jump, Attack
}