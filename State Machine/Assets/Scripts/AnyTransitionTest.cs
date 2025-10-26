using System;
using NewKris.FiniteStateMachine;
using UnityEngine;

namespace NewKris {
    public class AnyTransitionTest : MonoBehaviour {
        private StateMachine<AnyTransitionTest> _stateMachine;
        private Trigger<AnyTransitionTest> _toS2Trigger;
        private Trigger<AnyTransitionTest> _toS1Trigger;

        public bool HoldingSpace { get; set; }
        public string CurrentState => _stateMachine?.CurrentStateName ?? "State Machine not initialized";
        
        private void Awake() {
            State<AnyTransitionTest> s1 = State<AnyTransitionTest>.GetBuilder()
                .StateName("S1")
                .Build();
            
            State<AnyTransitionTest> s2 = State<AnyTransitionTest>.GetBuilder()
                .StateName("S2")
                .Build();

            _toS2Trigger = new Trigger<AnyTransitionTest>(this);
            _toS1Trigger = new Trigger<AnyTransitionTest>(this);
            
            Transition<AnyTransitionTest> toS2 = Transition<AnyTransitionTest>.GetBuilder(s2)
                .Condition(x => x.HoldingSpace)
                .SetTrigger(_toS2Trigger)
                .Build();
            
            Transition<AnyTransitionTest> toS1 = Transition<AnyTransitionTest>.GetBuilder(s1)
                .Condition(x => !x.HoldingSpace)
                .SetTrigger(_toS1Trigger)
                .Build();
            
            Transition<AnyTransitionTest>[] anyTransitions = { toS1, toS2 };
            
            _stateMachine = new StateMachine<AnyTransitionTest>(s1, this, anyTransitions);
            _stateMachine.DebugMode = true;
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(10, 10, 500, 500));
            
            GUILayout.Label($"Holding Space: {HoldingSpace}");
            GUILayout.Label($"Current State: {CurrentState}");
            
            GUILayout.EndArea();
        }

        private void Update() {
            _stateMachine.Tick();

            HoldingSpace = Input.GetKey(KeyCode.Space);

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                _toS1Trigger.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                _toS2Trigger.Invoke();
            }
        }
    }
}