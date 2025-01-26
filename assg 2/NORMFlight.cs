using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assg
{
    class NORMFlight : Flight
    {
        public NORMFlight(string fn, string o, string des, DateTime et, string s) : base(fn, o, des, et, s) { }

        public override double CalculateFees()
        {
            return 500.0; // Base fee for normal flights
        }

        public override string ToString()
        {
            return $"NORMFlight - {base.ToString()}";
        }
    }
}
