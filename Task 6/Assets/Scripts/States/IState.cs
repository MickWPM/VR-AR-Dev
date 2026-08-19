public interface IState
{
    public string StateName { get; }
    public void EnterState();
    public void UpdateState();
    public void ExitState();
}