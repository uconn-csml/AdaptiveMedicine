using System;

namespace AdaptiveMedicine.Common.Statechart.Attributes {
   [AttributeUsage(
         AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
   public class TransitionAttribute: Attribute {

      public string EventType { get; }
      public string StateType { get; }

      public TransitionAttribute(object eventType, object stateType = null) {
         EventType = eventType?.ToString();
         StateType = stateType?.ToString();
      }
   }
}
