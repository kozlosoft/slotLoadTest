using System.Threading;
using System.Windows.Forms;

namespace Slot_LoadTesting
{
    public static class SmartThreadSleep
    {
        public static void Sleep(int mls)
        {
            for (int i = 0; i < mls / 10; i++)
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
        }
    }
}