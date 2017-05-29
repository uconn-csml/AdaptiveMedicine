using System.Reflection;
using AdaptiveMedicine.Common.Statechart.Attributes;
using AdaptiveMedicine.Common.Statechart.Interfaces;

namespace AdaptiveMedicine.Common.Actors {
   public class StatechartTransition: ITransition {
      private TransitionAttribute _Transition { get; }

      public string EventTrigger { get { return _Transition.EventType; } }
      public string TargetState { get { return _Transition.StateType; } }
      public MethodInfo Action { get; }

      public StatechartTransition(TransitionAttribute transition, MethodInfo method) {
         _Transition = transition;
         Action = method;
      }
   }
}
