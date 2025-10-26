using System;

namespace NewKris.FiniteStateMachine {
    public class Transition<T> {
        public event Action<State<T>> OnTransitionTriggered; 
        
        public delegate bool TransitionCondition(T dependency);

        private readonly bool _hasTrigger;
        private readonly bool _hasCondition;
        private readonly Trigger<T> _trigger;
        private readonly State<T> _toState;
        private readonly TransitionCondition _condition;

        public State<T> ToState => _toState;

        internal Transition(
            Trigger<T> trigger,
            State<T> toState,
            TransitionCondition condition
        ) {
            _hasTrigger = trigger != null;
            _hasCondition = condition != null;
            
            _trigger = trigger;
            _toState = toState;
            _condition = condition;
        }

        public static TransitionBuilder<T> GetBuilder(State<T> toState) {
            return new TransitionBuilder<T>(toState) {
                
            };
        }
        
        public bool Evaluate(T dependency) {
            if (_hasTrigger || !_hasCondition) {
                return false;
            }
            
            return _condition.Invoke(dependency);
        }

        public void Activate() {
            if (!_hasTrigger) {
                return;
            }
            
            _trigger.OnTriggered += OnTriggered;
        }

        public void DeActivate() {
            if (!_hasTrigger) {
                return;
            }
            
            _trigger.OnTriggered -= OnTriggered;
        }

        private void OnTriggered(T dependency) {
            if (!_hasCondition || _condition.Invoke(dependency)) {
                OnTransitionTriggered?.Invoke(_toState);
            }
        }
    }
}