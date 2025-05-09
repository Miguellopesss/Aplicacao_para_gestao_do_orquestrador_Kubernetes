using System;
using System.Windows.Forms;
using kubernets;

namespace K8sDashboard
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Login());
        }
    }
}
