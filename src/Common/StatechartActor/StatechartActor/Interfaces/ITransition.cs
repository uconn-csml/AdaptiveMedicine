using System.Reflection;

namespace AdaptiveMedicine.Common.Statechart.Interfaces {
   public interface ITransition {
      string EventTrigger { get; }
      string TargetState { get; }
      MethodInfo Action { get; }
   }
}
