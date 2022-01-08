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
    public float G { get; private set; }
    public float H { get; private set; }
    public float F { get { return this.G + this.H; } }
    public NodeState State { get; set; }
    public Node ParentNode { get; set; }
    public List<IInteractable> Objects { get; set; }
}

public enum NodeState
{
    NotTested,
    Closed,
    Open
}