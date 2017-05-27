using System;

namespace AdaptiveMedicine.Actors.Base.Statechart.Attributes {
   [AttributeUsage(
         AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
   public class StateAttribute: Attribute {

      public string Type { get; }

      public StateAttribute(object stateType) {
         Type = stateType.ToString();
      }
   }
}
