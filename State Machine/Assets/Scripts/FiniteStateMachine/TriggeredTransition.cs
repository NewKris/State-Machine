using System;

namespace CoffeeBara.FiniteStateMachine {
    public class TriggeredTransition<T> {
        public event Action<State<T>> OnTransitionTriggered;
        
        private readonly T _dependency;
        private readonly Trigger _trigger;
        private readonly Transition<T> _transition;
        
        public TriggeredTransition(
            T dependency,
            Trigger trigger,
            State<T> toState, 
            Transition<T>.TransitionCondition otherConditions
        ) {
            _dependency = dependency;
            _trigger = trigger;
            _transition = new Transition<T>(toState, otherConditions);
        }

        public void Activate() {
            _trigger.OnTriggered += Evaluate;
        }

        public void DeActivate() {
            _trigger.OnTriggered -= Evaluate;
        }

        private void Evaluate() {
            if (_transition.condition == null || _transition.condition(_dependency)) {
                OnTransitionTriggered?.Invoke(_transition.toState);
            }
        }
    }
}