using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RowerMoniter.Core.Json
{
    public class SerializationBinder : ISerializationBinder
    {
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            throw new NotImplementedException();
        }

        public Type BindToType(string assemblyName, string typeName)
        {

            var type = typeName.Split('.').LastOrNone().Match(s => GetTypeByName(s), (Type)null);
        }


        private Type GetTypeByName(string name)
        {
        }
    }
}
