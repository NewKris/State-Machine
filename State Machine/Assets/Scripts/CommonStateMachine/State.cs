namespace VirtualDeviants.CommonStateMachine
{
    public abstract class State
    {
        public readonly Transition[] transitions;

        protected State(StateDependencies dependencies)
        {
            transitions = dependencies.transitions;
        }
        
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void Tick(float dt);
    }
}