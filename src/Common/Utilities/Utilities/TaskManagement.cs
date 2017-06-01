using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace AdaptiveMedicine.Common.Utilities {
   public static class TaskManagement {

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static void Forget(this Task task) {
         task.ConfigureAwait(false);
      }
   }
}