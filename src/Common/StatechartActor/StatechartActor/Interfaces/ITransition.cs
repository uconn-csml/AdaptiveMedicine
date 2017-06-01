using static AdaptiveMedicine.Common.Actors.StatechartTransition;

namespace AdaptiveMedicine.Common.Statechart.Interfaces {
   public interface ITransition {
      string EventTrigger { get; }
      string TargetState { get; }
      TransitionAction Action { get; }
   }
}
