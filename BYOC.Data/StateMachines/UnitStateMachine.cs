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
                Console.WriteLine("Enter State 1");
                test = 0;
            },
            OnExit = () =>
            {
                Console.WriteLine("Exit State 1");
                test = 0;
            },
            OnTick = () =>
            {
                Console.WriteLine("Tick on State 1 {0}", DateTime.Now.ToLocalTime());
                Console.WriteLine("Currently Idle at: {0},{1}", _unit.Position.X, _unit.Position.Y);
                test++;
            }
        };

        State idleState2 = new State
        {
            Name = "Idle2",
            OnEnter = () =>
            {
                Console.WriteLine("Enter State 2"); 
                test = 0;
            },
            OnExit = () =>
            {
                Console.WriteLine("Exit State 2");
                test = 0;
            },
            OnTick = () =>
            {
                Console.WriteLine("Tick on State 2 {0}", DateTime.Now.ToLocalTime());
                test++;
            }
        };
        
        idleState1.Transitions.Add(idleState2, () =>
            {
                Console.WriteLine("Evaluating {0} == 5", test);
                return test == 5;
            });
        idleState2.Transitions.Add(idleState1, () =>
            {
                Console.WriteLine("Evaluating {0} == 2", test);
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