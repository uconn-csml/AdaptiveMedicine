using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AdaptiveMedicine.Common.Statechart.Attributes;
using AdaptiveMedicine.Common.Statechart.Interfaces;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Common.Actors {
   public class StatechartTransition: ITransition {
      public delegate Task<IEnumerable<IEvent>> TransitionAction(IEvent anEvent, Actor actor);
      private TransitionAttribute _Transition { get; }

      public string EventTrigger { get { return _Transition.EventType; } }
      public string TargetState { get { return _Transition.StateType; } }
      public TransitionAction Action { get; }

      public StatechartTransition(TransitionAttribute transition, MethodInfo method, IState context) {
         _Transition = transition;
         Action = (TransitionAction) Delegate.CreateDelegate(typeof(TransitionAction), context, method);
      }
   }
}
