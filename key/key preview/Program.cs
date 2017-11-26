using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace key_preview {

	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]

		static void Main() {
            //������
            bool onlyInstance;
            Mutex mtx = new Mutex(true, "Clipboard", out onlyInstance);

            if (onlyInstance)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            else
            {
                MessageBox.Show("���������� ��� ��������", "���������", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

            //

            
		}
	}
}