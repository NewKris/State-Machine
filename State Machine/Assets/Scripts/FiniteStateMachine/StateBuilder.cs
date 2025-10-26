using System;

namespace NewKris.FiniteStateMachine {
    public class StateBuilder<T> {
        private string _stateName = "State";
        private Action<T> _onEnter;
        private Action<T> _onExit;
        private Action<T> _onTick;

        internal StateBuilder() {}
        
        public State<T> Build() {
            return new State<T>(
                _stateName,
                _onEnter,
                _onExit,
                _onTick
            );
        }

        public StateBuilder<T> StateName(string stateName) {
            _stateName = stateName;
            return this;
        }

        public StateBuilder<T> OnEnter(Action<T> onEnter) {
            _onEnter = onEnter;
            return this;
        }

        public StateBuilder<T> OnExit(Action<T> onExit) {
            _onExit = onExit;
            return this;
        }

        public StateBuilder<T> OnTick(Action<T> onTick) {
            _onTick = onTick;
            return this;
        }
    }
}