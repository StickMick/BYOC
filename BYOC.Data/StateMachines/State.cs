using System.Collections;

namespace BYOC.Data.StateMachines;

public class State
{
    public string Name { get; set; } = "";
    public Dictionary<State, Func<bool>> Transitions { get; set; } = new();
    public Action? OnTick { get; set; }
    public Action? OnEnter { get; set; }
    public Action? OnExit { get; set; }

    public void Enter(IStateMachine stateMachine)
    {
        OnEnter?.Invoke();
    }

    public void Exit(IStateMachine stateMachine)
    {
        OnExit?.Invoke();
    }

    public void Tick(IStateMachine stateMachine)
    {
        foreach (var transition in Transitions)
        {
            if (transition.Value.Invoke())
            {
                stateMachine.SetCurrentState(transition.Key);
                return;
            }
        }
        OnTick?.Invoke();
    }

}