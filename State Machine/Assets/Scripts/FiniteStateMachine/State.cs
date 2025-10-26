using System;

namespace NewKris.FiniteStateMachine {
    public class State<T> {
        public event Action<State<T>> OnExitState;

        private readonly Action<T> _onEnter;
        private readonly Action<T> _onExit;
        private readonly Action<T> _onTick;

        private bool _enabled;
        private bool _hasTransitions;
        private Transition<T>[] _transitions;

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
            
            _enabled = false;
            _hasTransitions = false;
        }

        public static StateBuilder<T> GetBuilder() {
            return new StateBuilder<T>();
        }

        public void SetTransitions(params Transition<T>[] transitions) {
            _transitions = transitions;
            _hasTransitions = transitions.Length > 0;
        }

        internal void Enter(T dependency) {
            _enabled = true;
            
            EnableTransitions(_transitions);
            _onEnter?.Invoke(dependency);
        }
        
        internal void Exit(T dependency) {
            _enabled = false;

            DisableTransitions(_transitions);
            _onExit?.Invoke(dependency);
        }

        internal void Tick(T dependency) {
            _onTick?.Invoke(dependency);
        }

        internal void TryTransition(T dependency) {
            if (!_hasTransitions || !_enabled) {
                return;
            }
            
            foreach (Transition<T> transition in _transitions) {
                if (transition.Evaluate(dependency)) {
                    ExitState(transition.ToState);
                    break;
                }
            }
        }
        
        private void EnableTransitions(Transition<T>[] transitions) {
            if (transitions == null) {
                return;
            }

            foreach (Transition<T> transition in transitions) {
                transition.OnTransitionTriggered += ExitState;
                transition.Activate();
            }
        }

        private void DisableTransitions(Transition<T>[] transitions) {
            if (transitions == null) {
                return;
            }
            
            foreach (Transition<T> transition in transitions) {
                transition.OnTransitionTriggered -= ExitState;
                transition.DeActivate();
            }
        }
        
        private void ExitState(State<T> toState) {
            if (!_enabled) {
                return;
            }
            
            OnExitState?.Invoke(toState);
        }
    }
}