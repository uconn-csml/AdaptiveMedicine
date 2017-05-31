using System;

namespace AdaptiveMedicine.Experiments.AlgorithmActor.Attributes {
   [AttributeUsage(
         AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
   public class ServiceNameAttribute: Attribute {

      public string Name { get; }

      public ServiceNameAttribute(string name) {
         Name = name;
      }
   }
}
