using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace NewKris.FiniteStateMachine {
    public class StateMachine<T> {
        [NotNull]
        private State<T> _currentState;
        
        private readonly T _stateDependency;
        private readonly State<T> _defaultState;
        private readonly Transition<T>[] _anyTransitions;
        private readonly Queue<State<T>> _queuedStates;

        public string CurrentStateName => _currentState.StateName;
        public bool DebugMode { get; set; }
        
        public StateMachine(
            State<T> defaultState,
            T stateDependency,
            Transition<T>[] anyTransitions
        ) {
            _queuedStates = new Queue<State<T>>();
            
            _currentState = defaultState;
            _defaultState = defaultState;
            _stateDependency = stateDependency;
            _anyTransitions = anyTransitions;
            
            _currentState.OnExitState += OnStateExited;
            _currentState.Enter(_stateDependency);
            
            EnableAnyTransitions();
        }

        ~StateMachine() {
            _currentState.OnExitState -= OnStateExited;
            DisableAnyTransitions();
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

            if (DebugMode) {
                Debug.Log($"Entered state {_currentState.StateName}");
            }
        }

        private void OnStateExited(State<T> nextState) {
            _queuedStates.Enqueue(nextState);
        }

        private void EnableAnyTransitions() {
            if (_anyTransitions == null) {
                return;
            }
            
            foreach (Transition<T> anyTransition in _anyTransitions) {
                anyTransition.OnTransitionTriggered += OnStateExited;
                anyTransition.Activate();
            }
        }

        private void DisableAnyTransitions() {
            if (_anyTransitions == null) {
                return;
            }
            
            foreach (Transition<T> anyTransition in _anyTransitions) {
                anyTransition.OnTransitionTriggered -= OnStateExited;
                anyTransition.DeActivate();
            }
        }

        private void TryAnyTransition() {
            if (_anyTransitions == null) {
                return;
            }
            
            foreach (Transition<T> anyTransition in _anyTransitions) {
                if (anyTransition.Evaluate(_stateDependency) && _currentState != anyTransition.ToState) {
                    OnStateExited(anyTransition.ToState);
                    break;
                }
            }
        }
    }
}
