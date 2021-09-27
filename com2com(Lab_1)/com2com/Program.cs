using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.IO.Ports;
using System.Threading;

namespace com2com
{
    static class Program
    {
        class MyApplicationContext : ApplicationContext
        {
            private int formCount;
            public com2com form1;
            public com2com form2;

            private MyApplicationContext()
            {
                formCount = 0;

                // Create both application forms and handle the Closed event 
                // to know when both forms are closed.
                form1 = new com2com();
                form1.Closed += new EventHandler(OnFormClosed);
                formCount++;

                form2 = new com2com();
                form2.Closed += new EventHandler(OnFormClosed);
                formCount++;

                // Show both forms.
                form1.Show();
                form2.Show();

                Thread thread1 = new Thread(StartThread);
                Thread thread2 = new Thread(StartThread);

                thread1.Start();
                thread2.Start();
            }

            private void OnFormClosed(object sender, EventArgs e)
            {
                // When a form is closed, decrement the count of open forms. 

                // When the count gets to 0, exit the app by calling 
                // ExitThread().
                formCount--;
                if (formCount == 0)
                    ExitThread();
            }
            private void StartThread()
            {
                MyApplicationContext context = new MyApplicationContext();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new com2com());

                Application.Exit();
            }
            /// <summary>
            /// Главная точка входа для приложения.
            /// </summary>
            [STAThread]
            static void Main()
            {
                // MyApplicationContext context = new MyApplicationContext();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new com2com());

                //Application.Run();
                //Application.Exit();
            }
        }
    }
}
