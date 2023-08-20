namespace GameKit.Services.ApplicationManager
{
    public class ApplicationStateChangeMessage
    {
        public ApplicationState state;
        public ApplicationStateChangeMessage(ApplicationState state) { this.state = state; }
    }

    public enum ApplicationState
    {
        Run,
        Pause,
        Quit
    }
}