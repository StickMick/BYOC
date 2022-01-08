using BYOC.Data.StateMachines;
namespace BYOC.Data.Objects;

public class Unit : IInteractable
{
    public Unit(int x, int y)
    {
        _stateMachine = new(this);
        Position = new Position(x, y);
    }

    private readonly UnitStateMachine _stateMachine;

    public Position? Position { get; private set; }
    public Position? MoveTarget { get; private set; }

    public BasicList<Position> Path { get; set; } = new(); //so its already newed up.
    public BasicList<EnumActions> Actions => new()
    {
        EnumActions.Attack
    };

    public void Tick()
    {
        _stateMachine.Tick();
    }
    
    public void SetMoveTarget(int x, int y)
    {
        MoveTarget!.X = x;
        MoveTarget.Y = y;
    }
    
    public void SetMoveTarget(Position position)
    {
        MoveTarget = position;
    }

    public void Interact(IInteractable item)
    {
        foreach (var action in Actions)
        {
            Console.WriteLine($"I can {action} this {item.GetType().Name}");
        }
        //Array possibleActions = Enum.GetValues(typeof(Actions));
        
        //foreach(Actions action in possibleActions) {  
        //    if (item.Actions.HasFlag(action))
        //        Console.WriteLine($"I can [{action}] this [{item.GetType().Name}]");  
        //}
    }

    public void Move()
    {
        if (Path.Any()) 
            Position = Path.First();
    }

    //public EnumActions Actions => EnumActions.Attack;
}