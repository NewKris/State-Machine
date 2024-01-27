namespace VirtualDeviants.CommonStateMachine
{
    public readonly struct StateDependencies
    {
        public readonly Transition[] transitions;

        public StateDependencies(Transition[] transitions)
        {
            this.transitions = transitions;
        }
    }
}