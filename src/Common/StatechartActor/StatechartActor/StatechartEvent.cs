using System;
using AdaptiveMedicine.Common.Statechart.Interfaces;

namespace AdaptiveMedicine.Common.Actors {
   public class StatechartEvent: IEvent {
      public DateTime Id { get; }
      public string Type { get; }
      public object Input { get; }

      public StatechartEvent(string type, DateTime id, object input = null) {
         Id = id;
         Type = type;
         Input = input;
      }

      public StatechartEvent(Enum type, DateTime id, object input = null)
         : this(type.ToString(), id, input) {
      }

      public StatechartEvent(string type)
         : this(type, DateTime.Now, null) {
      }

      public StatechartEvent(Enum type)
         : this(type.ToString()) {
      }
   }
}
