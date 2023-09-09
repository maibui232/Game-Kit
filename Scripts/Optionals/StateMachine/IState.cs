namespace GameKit.Optionals.StateMachine
{
    using Cysharp.Threading.Tasks;

    public interface IState
    {
        UniTask Enter();
        UniTask Exit();
    }
}