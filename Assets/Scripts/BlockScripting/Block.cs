using System.Collections.Generic;

public abstract class Block
{
    public List<Block> inputs;
    public Block next;
    public Value value;

    public abstract bool Evaluate();

    public abstract void Reset();
}
