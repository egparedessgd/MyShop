using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtensions
{
    public static class FunctionalExt
    {
        public struct Unit { }

        public static Unit ActionToFunc(this Action action)
        {
            action();
            return new Unit();
        }
    }

}
