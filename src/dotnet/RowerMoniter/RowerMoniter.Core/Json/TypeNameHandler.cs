using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Core.Json
{
    public class SerializationBinder : ISerializationBinder
    {

        private Lazy<IDictionary<string, Type>> _typeLookup = new Lazy<IDictionary<string, Type>>(() => InitializeTypeLookup());

        internal IDictionary<string, Type> TypeLookup => _typeLookup.Value;

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            throw new NotImplementedException();
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            var defaultBinder = new DefaultSerializationBinder();

            if (assemblyName == "RowerMoniter")
            {
                var type = typeName.Split('.').LastOrNone().Map(GetTypeByName).IfNone(typeof(object));
                return type;
            }
            else 
            {
                return defaultBinder.BindToType(assemblyName, typeName);
            }

        }


        private Type GetTypeByName(string name)
        {
            if (TypeLookup.TryGetValue(name, out var type)) 
                return type;
            
            throw new InvalidOperationException($"Can't find type '{name}'");
        }

        private static IDictionary<string, Type> InitializeTypeLookup() 
        {
            var types = Assembly.GetExecutingAssembly().GetExportedTypes();
            
            var filtered = types.Where(t => t.Namespace == "RowerMoniter.Contracts" && t.IsSerializable);

            return types.ToDictionary(t => t.Name, t => t);
        }
    }
}
