using System;

namespace CoffeeBara.FiniteStateMachine {
    public class Trigger<T> {
        public event Action<T> OnTriggered;

        private readonly T _dependency;

        public Trigger(T dependency) {
            _dependency = dependency;
        }

        public void Invoke() {
            OnTriggered?.Invoke(_dependency);
        }
    }
}