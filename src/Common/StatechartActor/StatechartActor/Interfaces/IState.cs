using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors.Runtime;

namespace AdaptiveMedicine.Common.Statechart.Interfaces {
   public interface IState {
      string Type { get; }
      Task<bool> EntryActionAsync(Actor actor);
      Task<bool> ExitActionAsync(Actor actor);
      ITransition GetActivatedTransition(IEvent anEvent);
   }
}
