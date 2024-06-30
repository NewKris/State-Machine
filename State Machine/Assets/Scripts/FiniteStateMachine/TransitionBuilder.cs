namespace CoffeeBara.FiniteStateMachine {
    public class TransitionBuilder<T> {
        private Trigger<T> _trigger;
        private State<T> _toState;
        private Transition<T>.TransitionCondition _condition;
        
        internal TransitionBuilder() {}

        public Transition<T> Build() {
            return new Transition<T>(_trigger, _toState, _condition);
        }

        public TransitionBuilder<T> SetTrigger(Trigger<T> trigger) {
            _trigger = trigger;
            return this;
        }

        public TransitionBuilder<T> ToState(State<T> toState) {
            _toState = toState;
            return this;
        }

        public TransitionBuilder<T> Condition(Transition<T>.TransitionCondition condition) {
            _condition = condition;
            return this;
        }
    }
}