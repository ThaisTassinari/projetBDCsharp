using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    class Enrollments
    {
        internal static int UpdateFinalGrade(string[] a, string ev)
        {
            Nullable<int> fg;
            int temp;

            if (ev == "")
            {
                fg = null;
            }
            else if (int.TryParse(ev, out temp) && (0 <= temp && temp <= 100))
            {
                fg = temp;
            }
            else
            {
                TP2___Thais.Form1.BLLMessage(
                          "Final Grade must be an integer between 0 and 100"
                          );
                return -1;
            }

            return Data.Enrollments.UpdateFinalGrade(a, fg);
        }
    }
}
