using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FuncCompiler
{
    [Serializable()]
    public class BindNotFoundException : System.Exception
    {
        public BindNotFoundException() : base() { }
        public BindNotFoundException(string message) : base(message) { }
        public BindNotFoundException(string message, System.Exception inner) : base(message, inner) { }

        protected BindNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

}
