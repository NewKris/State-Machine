using System;

namespace CoffeeBara.FiniteStateMachine {
    public class State<T> {
        public event Action<State<T>> OnExitState;

        private readonly Action<T> _onEnter;
        private readonly Action<T> _onExit;
        private readonly Action<T> _onTick;

        private Transition<T>[] _transitions;
        private TriggeredTransition<T>[] _triggeredTransitions;

        public string StateName { get; }

        internal State(
            string stateName,
            Action<T> onEnter,
            Action<T> onExit,
            Action<T> onTick
        ) {
            StateName = stateName;
            _onEnter = onEnter;
            _onExit = onExit;
            _onTick = onTick;
        }

        public static StateBuilder<T> GetBuilder() {
            return new StateBuilder<T>();
        }

        public void SetTransitions(params Transition<T>[] transitions) {
            _transitions = transitions;
        }

        public void SetTriggeredTransitions(params TriggeredTransition<T>[] triggeredTransitions) {
            _triggeredTransitions = triggeredTransitions;
            
            foreach (TriggeredTransition<T> triggeredTransition in _triggeredTransitions) {
                triggeredTransition.OnTransitionTriggered += ForceExit;
            }
        }

        public void Enter(T dependency) {
            ActivateTriggers(_triggeredTransitions);
            _onEnter?.Invoke(dependency);
        }
        
        public void Exit(T dependency) {
            DeActivateTriggers(_triggeredTransitions);
            _onExit?.Invoke(dependency);
        }

        public void Tick(T dependency) {
            _onTick?.Invoke(dependency);
        }

        public void TryTransition(T dependency) {
            if (_transitions == null) return;
            
            foreach (Transition<T> transition in _transitions) {
                if (!transition.condition(dependency)) continue;
                
                OnExitState?.Invoke(transition.toState);
                break;
            }
        }

        private void ForceExit(State<T> toState) {
            OnExitState?.Invoke(toState);
        }

        private static void ActivateTriggers(TriggeredTransition<T>[] triggers) {
            if (triggers == null) return;
            
            foreach (TriggeredTransition<T> triggeredTransition in triggers) {
                triggeredTransition.Activate();
            }
        }

        private static void DeActivateTriggers(TriggeredTransition<T>[] triggers) {
            if (triggers == null) return;
            
            foreach (TriggeredTransition<T> triggeredTransition in triggers) {
                triggeredTransition.DeActivate();
            }
        }
    }
}