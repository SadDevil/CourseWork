using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace warships
{
    public static class Utils
    {
        /// <summary>
        /// поверне чи рядок є числом
        /// </summary>
        public static bool IsNum(string message)
        {
            int num;
            return int.TryParse(message, out num);
        }
    }
}
