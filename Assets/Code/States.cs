using System.Collections.Generic;

namespace Code
{
    public class States
    {
        public delegate void StateChangedHandler(string state);

        public HashSet<string> currentStates = new HashSet<string>();
        
        public event StateChangedHandler onEnterState;
        public event StateChangedHandler onExitState;

        public bool HasState(string stateName)
        {
            return currentStates.Contains(stateName);
        }

        public void EnterState(string state)
        {
            currentStates.Add(state);
            OnStatesEnter(state);
        }

        public void ExitState(string state)
        {
            currentStates.Remove(state);
            OnStatesExit(state);
        }

        public void ClearCallbacks()
        {
            onEnterState = null;
            onExitState = null;
        }

        public void OnStatesEnter(string state)
        {
            onEnterState?.Invoke(state);
        }

        public void OnStatesExit(string state)
        {
            onExitState?.Invoke(state);
        }
    }
}