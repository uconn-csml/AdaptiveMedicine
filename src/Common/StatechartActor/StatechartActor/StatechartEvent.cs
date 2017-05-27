using System;
using AdaptiveMedicine.Actors.Base.Statechart.Interfaces;

namespace AdaptiveMedicine.Actors.Base.Statechart {
   public class StatechartEvent: IEvent {
      public DateTime Id { get; }
      public string Type { get; }
      public object Input { get; }

      public StatechartEvent(DateTime id, string type, object input = null) {
         Id = id;
         Type = type;
         Input = input;
      }

      public StatechartEvent(DateTime id, Enum type, object input = null)
         : this(id, type.ToString(), input) {
      }
   }
}
