﻿using System;

namespace AdaptiveMedicine.Actors.Base.Statechart.Interfaces {
   public interface IEvent {
      DateTime Id { get; }
      string Type { get; }
      object Input { get; }
   }
}
