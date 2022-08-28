using BYOC.Data.Events;
using BYOC.Data.StateMachines;
using Serilog;

namespace BYOC.Data.Objects;

public class Unit : IInteractable
{
    public Unit(ILogger logger, int x, int y)
    {
        _stateMachine = new(logger, this);
        Position = new Position(x, y);
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    
    private readonly UnitStateMachine _stateMachine;
    public Position Position { get; private set; }
    public Position? MoveTarget { get; private set; }
    public Player Player { get; set; }
    
    public EventHandler<UnitMovedEventArgs>? OnMove { get; set; }

    public BasicList<Position> Path { get; set; } = new(); //so its already newed up.
    
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

    private void NotifyMoveEvent()
    {
        EventHandler<UnitMovedEventArgs>? handler = OnMove;
        handler?.Invoke(this, new UnitMovedEventArgs
        {
            Id = Id,
            Position = Position
        });
    }

    public void Move()
    {
        if (Path.Any())
        {
            Position = Path.Last();
            Path.RemoveSpecificItem(Position);
            NotifyMoveEvent();
        }  
    }

    //public EnumActions Actions => EnumActions.Attack;
}

