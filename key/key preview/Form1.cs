using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Utilities;
using System.Collections.Generic;
using System.Threading;

using System.Diagnostics;
using System.IO;

namespace key_preview
{
    //�������� �� ���������� �����
    

    public partial class Form1 : Form
    {
        /*
        private static Mutex m_instance;
        private const string m_appName = "key preview";

        /// <summary>
        /// ������� ���������� Mutex
        /// </summary>
        /// <returns></returns>
        private static bool SetMutexFailed()
        {
            bool tryCreateNewApp;
            m_instance = new Mutex(true, m_appName, out tryCreateNewApp);
            return !tryCreateNewApp;
        }
        */

        globalKeyboardHook gkh = new globalKeyboardHook();

        bool C = false;
        bool V = false;
        bool D = false; 
        bool CTRL = false;
        bool RCTRL = false;
        bool Q = false;
        bool RALT = false;

        String[] clipboard = new String[10]; //������ � ���������� ������ ������
        int clipCount = 0; //���-�� ��������� � �������

        //bool onlyInstance;
        //private const string AppMutexName = "SingleApp"; //�������� ������� � �������   

        public Form1()
        {
            //
            

            //
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e) //��� �� ����������� �������
        {


            gkh.HookedKeys.Add(Keys.A);
            gkh.HookedKeys.Add(Keys.D);
            gkh.HookedKeys.Add(Keys.C); // - C
            gkh.HookedKeys.Add(Keys.V);
            gkh.HookedKeys.Add(Keys.Q);

            gkh.HookedKeys.Add(Keys.LControlKey); //����� Ctrl
            gkh.HookedKeys.Add(Keys.RControlKey); //������ Ctrl
            gkh.HookedKeys.Add(Keys.RMenu); //������ Alt



            //��� ����
            Keys[] k = new Keys[10];
            k[0] = Keys.D0;
            k[1] = Keys.D1;
            k[2] = Keys.D2;
            k[3] = Keys.D3;
            k[4] = Keys.D4;
            k[5] = Keys.D5;
            k[6] = Keys.D6;
            k[7] = Keys.D7;
            k[8] = Keys.D8;
            k[9] = Keys.D9;
            for (int i = 0; i < 10; i++) //��� �� ��� ����
            {
                gkh.HookedKeys.Add(k[i]);
            }

            //gkh.HookedKeys.Add(Keys.D1);

            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown); //�������
            gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
            Clipboard.Clear();

        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //����� ��������� ��� ������� ����������
            //textBox1.Text = e.ToString();
        }

        void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            lstLog.Items.Add("Down\t" + e.KeyCode.ToString()); //������ � ��������

            //������������� ����� ��� ������� 
            if (e.KeyCode == Keys.LControlKey)
            {
                CTRL = true;
            }
            if (e.KeyCode == Keys.RControlKey)
            {
                RCTRL = true;
            }
            else if (e.KeyCode == Keys.C)
            {
                C = true;
            }
            else if (e.KeyCode == Keys.V)
            {
                V = true;
            }
            else if (e.KeyCode == Keys.D)
            {
                D = true;
            }
            else if (e.KeyCode == Keys.Q)
            {
                Q = true;
            }
            else if (e.KeyCode == Keys.RMenu)
            {
                RALT = true;
            }


            //Keys[] k = new Keys[10];
            //k[0] = Keys.D0;
            //k[1] = Keys.D1;
            //k[2] = Keys.D2;
            //k[3] = Keys.D3;
            //k[4] = Keys.D4;
            //k[5] = Keys.D5;
            //k[6] = Keys.D6;
            //k[7] = Keys.D7;
            //k[8] = Keys.D8;
            //k[9] = Keys.D9;

            e.Handled = false; //���� false, �� ���������� ������� ������
        }

        void gkh_KeyUp(object sender, KeyEventArgs e)//������� �� key up
        {
            Keys[] k = new Keys[10];
                k[0] = Keys.D0;
                k[1] = Keys.D1;
                k[2] = Keys.D2;
                k[3] = Keys.D3;
                k[4] = Keys.D4;
                k[5] = Keys.D5;
                k[6] = Keys.D6;
                k[7] = Keys.D7;
                k[8] = Keys.D8;
                k[9] = Keys.D9;

            //���������� � ������ ��� �����������
            if (CTRL && C)
            {
                textBox1.Text += "Ctrl+C pressed" + Environment.NewLine;
                String lastClipboar = Clipboard.GetText(); //�������� �������� ������ � ����������
                Console.Out.WriteLine("\"" + lastClipboar + "\"");
                if (clipCount < 10) //���� ���� ����� � ������ - �������� � ����� ������� �����������
                {
                    textBox1.Text += "clpcount = " + clipCount + " - ����� �� �����" + Environment.NewLine;
                    clipboard[clipCount] = lastClipboar;
                    //Console.Out.WriteLine("�������� � " + clipCount + " ������� ������� " + clipboard[clipCount]);
                    clipCount++;

                }
                else //����� �������
                {
                    textBox1.Text += "clpcount = " + clipCount + " - ����� �������" + Environment.NewLine;

                    for (int i = 0; i < 9; i++)
                    {
                        clipboard[i] = clipboard[i + 1];
                    }
                    clipboard[9] = lastClipboar;
                }
            }

            //������ ������� �������� � ����� ������
            try
            {
                // 0 
                if (CTRL && D && e.KeyCode == k[0] || RCTRL && e.KeyCode == k[0] || RALT && e.KeyCode == k[0])
                {
                    try
                    {
                        Console.WriteLine("������� ������� ������� - " + clipboard[0]);
                        Clipboard.Clear();
                        //Thread.Sleep(500);
                        Clipboard.SetText(clipboard[0]); // ������� ������� �������� � ����� ������
                        //Thread.Sleep(1000);
                        SendKeys.Send("^V");
                    }catch(Exception ex) {
                        Console.WriteLine("Exception ");
                        SendKeys.Send("^V"); 
                    }
                }
                // 1
                if ((CTRL && D && e.KeyCode == k[1]) || RCTRL && e.KeyCode == k[1] || RALT && e.KeyCode == k[1])
                {
                    //Console.WriteLine(clipboard[1]);
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[1]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^(V)");
                }
                // 2
                if (CTRL && D && e.KeyCode == k[2] || RCTRL && e.KeyCode == k[2] || RALT && e.KeyCode == k[2])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[2]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^" + "V");
                }
                // 3
                if (CTRL && D && e.KeyCode == k[3] || RCTRL && e.KeyCode == k[3] || RALT && e.KeyCode == k[3])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[3]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");
                }
                // 4
                if (CTRL && D && e.KeyCode == k[4] || RCTRL && e.KeyCode == k[4] || RALT && e.KeyCode == k[4])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[4]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");
                }
                // 5
                if (CTRL && D && e.KeyCode == k[5] || RCTRL && e.KeyCode == k[5])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[5]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");
                }
                // 6
                if (CTRL && D && e.KeyCode == k[6] || RCTRL && e.KeyCode == k[6])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[6]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");

                }
                // 7
                if (CTRL && D && e.KeyCode == k[7] || RCTRL && e.KeyCode == k[7])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[7]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");
                }
                // 8
                if (CTRL && D && e.KeyCode == k[8] || RCTRL && e.KeyCode == k[8])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[8]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");
                }
                // 9
                if (CTRL && D && e.KeyCode == k[9] || RCTRL && e.KeyCode == k[9])
                {
                    Clipboard.Clear();
                    Clipboard.SetText(clipboard[9]); // ������� ������� �������� � ����� ������
                    SendKeys.Send("^{v}");
                }
            }
            catch (Exception ex) { }

            lstLog.Items.Add("Up\t" + e.KeyCode.ToString()); //����� key up � ��������

            //������� ����� ��� �������
            if (e.KeyCode == Keys.C)
            {
                C = false;
            }
            if (e.KeyCode == Keys.D)
            {
                D = false;
            }
            else if (e.KeyCode == Keys.LControlKey)
            {
                CTRL = false;
            }
            else if (e.KeyCode == Keys.RControlKey)
            {
                RCTRL = false;
            }
            else if (e.KeyCode == Keys.V)
            {
                V = false;
                //textBox1.Text += "V ������";
            }
            else if (e.KeyCode == Keys.Q)
            {
                Q = false;
            }
            else if (e.KeyCode == Keys.RMenu)
            {
                RALT = false;
            }

            e.Handled = false;
        }

        private void lstLog_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            lstLog.Items.Clear();
            textBox1.Clear();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) //����� ������� � �������� � ��������3
        {
            int i = 0;
            foreach (String element in clipboard)
            {
                textBox3.Text += i + " => " + element + Environment.NewLine;
                i++;
            }
            textBox3.Text += Environment.NewLine;
        }
    }
}