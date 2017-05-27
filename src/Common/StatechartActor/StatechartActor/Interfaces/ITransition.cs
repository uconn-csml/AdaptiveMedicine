using System.Reflection;

namespace AdaptiveMedicine.Actors.Base.Statechart.Interfaces {
   public interface ITransition {
      string EventTrigger { get; }
      string TargetState { get; }
      MethodInfo Action { get; }
   }
}
