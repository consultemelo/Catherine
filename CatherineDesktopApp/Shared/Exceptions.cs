using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatherineDesktopApp.Shared.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message) : base(message) { }
    }

    public class ResourceKeyNotFoundException : Exception
    {
        public ResourceKeyNotFoundException(string message) : base(message) { }
    }
}
