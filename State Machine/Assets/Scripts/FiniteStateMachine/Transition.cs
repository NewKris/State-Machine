namespace CoffeeBara.FiniteStateMachine {
    public readonly struct Transition<T> {
        public delegate bool TransitionCondition(T dependency);
        
        public readonly State<T> toState;
        public readonly TransitionCondition condition;

        public Transition(
            State<T> toState, 
            TransitionCondition condition
        ) {
            this.toState = toState;
            this.condition = condition;
        }
    }
}