using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestModule
{
    public class TestCustomAttributes
    {
        [Obsolete("Test Message")]
        public int TestProperty { get; set; } = 0;
    }
}
