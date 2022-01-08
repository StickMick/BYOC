using BYOC.Data.Objects;

namespace BYOC.Data;

public class Node
{
    public Node(Position position, bool isWalkable)
    {
        Position = position;
        IsWalkable = isWalkable;
    }
    
    public Position Position { get; private set; }
    public bool IsWalkable { get; set; }
    public float G { get; set; }
    public float H { get; set; }
    public float F => G + H;
    public NodeState State { get; set; }
    public Node? ParentNode { get; set; }
    public BasicList<IInteractable> Objects { get; set; } = new();
}

public enum NodeState
{
    NotTested,
    Closed,
    Open
}