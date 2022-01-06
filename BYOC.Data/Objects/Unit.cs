namespace BYOC.Data.Objects;

public class Unit : IInteractable
{
    public Unit(int x, int y)
    {
        Position.X = x;
        Position.Y = y;
    }

    public Position Position { get; private set; } = new Position();
    public Position MoveTarget { get; private set; } = new Position();

    
    
    public void SetMoveTarget(int x, int y)
    {
        MoveTarget.X = x;
        MoveTarget.Y = y;
    }

    public void Interact(IInteractable item)
    {
        Array possibleActions = Enum.GetValues(typeof(Actions));
        
        foreach(Actions action in possibleActions) {  
            if (item.Actions.HasFlag(action))
                Console.WriteLine($"I can [{action}] this [{item.GetType().Name}]");  
        }
    }

    public void Move(int x, int y)
    {
        Position.X = x;
        Position.Y = y;
    }

    public Actions Actions => Actions.Attack;
}