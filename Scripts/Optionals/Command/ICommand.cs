namespace GameKit.Optionals.Command
{
    public interface ICommand
    {
        void Execute();
        void UnDo();
    }
}