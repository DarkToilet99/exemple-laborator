using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Carutz.Domain
{
    [Serializable]
    class WrongCode : Exception
    {
        public WrongCode()
        {
        }

        public WrongCode(string? message) : base(message)
        {
        }

        public WrongCode(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected WrongCode(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
