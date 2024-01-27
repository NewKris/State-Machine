using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VirtualDeviants.CommonStateMachine
{
    public class StateMachine
    {
        private State _currentState;
        private Transition[] _anyTransitions;

        public void Tick(float dt)
        {
            _currentState.Tick(dt);

            TryTransition(_anyTransitions.Concat(_currentState.transitions));
        }

        public void ForceTransition(State state)
        {
            TransitionToState(state);
        }

        private void TryTransition(IEnumerable<Transition> transitions)
        {
            foreach (Transition transition in transitions)
            {
                if (!transition.condition()) continue;
                
                TransitionToState(transition.toState);
                return;
            }
        }

        private void TransitionToState(State toState)
        {
            _currentState.OnExit();
            _currentState = toState;
            _currentState.OnEnter();
        }
    }
}
