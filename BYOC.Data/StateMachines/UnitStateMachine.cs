using BYOC.Data.Objects;

namespace BYOC.Data.StateMachines;
public class UnitStateMachine : IStateMachine
{
    private readonly Unit _unit;
    private State _state;
    
    private int test = 0;

    public UnitStateMachine(Unit unit)
    {
        _unit = unit;
        
        State idleState1 = new State
        {
            Name = "Idle1",
            OnEnter = () =>
            {
                test = 0;
            },
            OnExit = () =>
            {
                test = 0;
            },
            OnTick = () =>
            {
                Console.WriteLine("Unit is in Idle State 1");
                test++;
            }
        };

        State idleState2 = new State
        {
            Name = "Idle2",
            OnEnter = () =>
            {
                test = 0;
            },
            OnExit = () =>
            {
                test = 0;
            },
            OnTick = () =>
            {
                Console.WriteLine("Unit is in Idle State 2");
                test++;
            }
        };
        
        idleState1.Transitions.Add(idleState2, () =>
            {
                return test == 5;
            });
        idleState2.Transitions.Add(idleState1, () =>
            {
                return test == 2;
            });

        _state = idleState1;
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