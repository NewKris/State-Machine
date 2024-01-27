using System;

namespace VirtualDeviants.CommonStateMachine
{
    public readonly struct Transition
    {
        public readonly State toState;
        public readonly Func<bool> condition;

        public Transition(State toState, Func<bool> condition)
        {
            this.toState = toState;
            this.condition = condition;
        }
    }
}