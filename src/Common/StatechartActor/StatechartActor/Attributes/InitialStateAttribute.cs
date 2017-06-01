using System;

namespace AdaptiveMedicine.Common.Statechart.Attributes {
   [AttributeUsage(
         AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
   public class InitialStateAttribute: StateAttribute {

      public InitialStateAttribute(object stateType)
         : base(stateType) {
      }
   }
}
