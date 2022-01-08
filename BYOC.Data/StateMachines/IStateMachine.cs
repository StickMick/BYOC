namespace BYOC.Data.StateMachines;

public interface IStateMachine
{
    void Tick();
    void SetCurrentState(State state);
}