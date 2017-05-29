using System;

namespace AdaptiveMedicine.Common.Statechart.Interfaces {
   public interface IEvent {
      DateTime Id { get; }
      string Type { get; }
      object Input { get; }
   }
}
