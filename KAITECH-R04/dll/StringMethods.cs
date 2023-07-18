using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL
{
    public class StringMethods
    {
        public static int IsIntNumber(string Number)
        {
            int NewNumber;
            if (int.TryParse(Number,out NewNumber))
            {
                return NewNumber;
            }
            else
            {
                return 0;
            }
        }
        public static double IsDoubleNumber(string Number)
        {
            double NewNumber;
            if (Number != string.Empty || Number != null)
            {
                if (double.TryParse(Number, out NewNumber))
                {
                    return NewNumber;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

    }
}
