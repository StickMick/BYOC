namespace BYOC.Data;

public class Node
{
    public Position Location { get; private set; }
    public bool IsWalkable { get; set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F { get { return this.G + this.H; } }
    public NodeState State { get; set; }
    public Node ParentNode { get; set; }
}

public enum NodeState
{
    NotTested,
    Closed,
    Open
}