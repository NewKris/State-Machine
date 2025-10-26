namespace NewKris.FiniteStateMachine {
    public class TransitionBuilder<T> {
        private readonly State<T> _toState;
        private Trigger<T> _trigger;
        private Transition<T>.TransitionCondition _condition;

        internal TransitionBuilder(State<T> toState) {
            _toState = toState;
        }

        public Transition<T> Build() {
            return new Transition<T>(_trigger, _toState, _condition);
        }

        public TransitionBuilder<T> SetTrigger(Trigger<T> trigger) {
            _trigger = trigger;
            return this;
        }

        public TransitionBuilder<T> Condition(Transition<T>.TransitionCondition condition) {
            _condition = condition;
            return this;
        }
    }
}