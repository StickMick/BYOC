using BYOC.Data.Objects;

namespace BYOC.Data.StateMachines;
public class UnitStateMachine : IStateMachine
{
    private readonly Unit _unit;
    private State _state;
    
    public UnitStateMachine(Unit unit)
    {
        _unit = unit;
        
        State moveState = new State
        {
            Name = "Move",
            OnEnter = () =>
            {
                Console.WriteLine("Moving");
            },
            OnExit = () =>
            {
                Console.WriteLine("Stopped Moving");
            },
            OnTick = () =>
            {
                _unit.Move();
                Console.WriteLine("Moved to position: {0},{1}", _unit.Position.X, unit.Position.Y);
            }
        };

        State idleState = new State
        {
            Name = "Idle",
            OnEnter = () =>
            {
                Console.WriteLine("Idle");
            },
            OnExit = () =>
            {
                Console.WriteLine("Stopped being Idle");
            },
            OnTick = () =>
            {
                Console.WriteLine("Unit {0} is in Idle State", _unit.Id);
            }
        };
        
        moveState.Transitions.Add(idleState, () =>
        {
            return !_unit.Path.Any();
        });
        idleState.Transitions.Add(moveState, () =>
        {
            return _unit.Path.Any();
        });

        _state = idleState;
    }
    
    public void Tick()
    {
        _state.Tick(this);
    }

    public void SetCurrentState(State state)
    {
        _state.Exit(this);
 
        _state = state;
 
        _state.Enter(this);
        
        _state.Tick(this);
    }

}