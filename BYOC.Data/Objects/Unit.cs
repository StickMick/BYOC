namespace BYOC.Data.Objects;

public class Unit : IInteractable
{
    public Unit(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public int X { get; private set; }
    public int Y { get; private set; }

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
        X = x;
        Y = y;
    }

    public Actions Actions => Actions.Attack;
}