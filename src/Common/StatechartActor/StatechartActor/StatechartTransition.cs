using System.Reflection;
using AdaptiveMedicine.Actors.Base.Statechart.Attributes;
using AdaptiveMedicine.Actors.Base.Statechart.Interfaces;

namespace AdaptiveMedicine.Actors.Base.Statechart {
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
