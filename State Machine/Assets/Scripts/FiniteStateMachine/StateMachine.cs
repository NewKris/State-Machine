using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CoffeeBara.FiniteStateMachine {
    public class StateMachine<T> {
        [NotNull]
        private State<T> _currentState;
        
        private readonly T _stateDependency;
        private readonly Transition<T>[] _anyTransitions;
        private readonly Queue<State<T>> _queuedStates;

        public string CurrentStateName => _currentState.StateName;
        
        public StateMachine(
            State<T> defaultState,
            T stateDependency,
            params Transition<T>[] anyTransitions
        ) {
            _queuedStates = new Queue<State<T>>();
            
            _currentState = defaultState;
            _stateDependency = stateDependency;
            _anyTransitions = anyTransitions;
            
            _currentState.OnExitState += OnStateExited;
            _currentState.Enter(_stateDependency);
        }

        ~StateMachine() {
            _currentState.OnExitState -= OnStateExited;
        }
        
        public void Tick() {
            _currentState.Tick(_stateDependency);
            _currentState.TryTransition(_stateDependency);
            TryAnyTransition();
            
            TryGoToNextState();
        }

        private void TryGoToNextState() {
            if (_queuedStates.Count == 0) {
                return;
            }

            State<T> nextState = _queuedStates.Dequeue();
            
            _currentState.Exit(_stateDependency);
            _currentState.OnExitState -= OnStateExited;
            
            _currentState = nextState;

            _currentState.OnExitState += OnStateExited;
            _currentState.Enter(_stateDependency);
        }

        private void OnStateExited(State<T> nextState) {
            _queuedStates.Enqueue(nextState);
        }

        private void TryAnyTransition() {
            if (_anyTransitions == null) {
                return;
            }
            
            foreach (Transition<T> anyTransition in _anyTransitions) {
                if (!anyTransition.condition(_stateDependency)) continue;

                OnStateExited(anyTransition.toState);
                break;
            }
        }
    }
}