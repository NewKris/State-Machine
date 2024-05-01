using System;

namespace CoffeeBara.FiniteStateMachine {
    public class Trigger {
        public event Action OnTriggered;

        public void Invoke() {
            OnTriggered?.Invoke();
        }
    }
}