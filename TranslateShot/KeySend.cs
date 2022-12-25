using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslateShot
{
    internal class KeySend
    {
        public void AltPrintScreen()
        {
            SendKeys.SendWait("%{PRTSC}");
        }
        public void CtrlV()
        {
            SendKeys.SendWait("^v");
        }
    }
}
