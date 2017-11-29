using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab6SAPCIS
{
    public partial class Form1 : Form
    {
        int count_symbol = 0; //кол-во найденных совпадений
        int current_symbol = 0; //текущее подствеченное совпадение
        int newStartIndex = 0; //индекс последнего вхождения текущей подсветки
        int oldStartIndex = 0; //старый индекс (для возврата предыдущего вхождения)
        List<int> indexes = new List<int> { }; //массив с индексами

        public Form1()
        {
            InitializeComponent();
        }
    

        public void clearSelectionColor() //метод для очистки подсветки
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.TextLength;
            richTextBox1.SelectionBackColor = richTextBox1.BackColor;
        }

        public void find_symbol() //поиск по тексту
        {
            clearSelectionColor(); //очистка прошлой подсвтетки
            String text = richTextBox1.Text; //haystack - текст в котором ищем

            String needle = textBox1.Text; //needle - искомый текст 
            int index = 0; // начальное положеие курсора (откуда ведеться поиск)
            count_symbol = 0; // сбрасываем кол-во найденных соападений

            while (index < text.LastIndexOf(needle))
            {
                richTextBox1.Find(needle, index, richTextBox1.TextLength, RichTextBoxFinds.None);
                indexes.Add(richTextBox1.Find(needle, index, richTextBox1.TextLength, RichTextBoxFinds.None)); //добавляем все инд в массив
                richTextBox1.SelectionBackColor = Color.Yellow;
                index = text.IndexOf(needle, index) + 1;
                count_symbol += 1; //при нахождении каждого совпадения - считаем его
            }
        }

        //навигация и подсветка другим цветом
        public void textFindNav() //навигация по подвеченному тексту
        {
            if (count_symbol != 0)
            {
                //отладочные данные
                //String n = richTextBox1.Text;
                //Console.WriteLine("startIndex = "+ startIndex +" => "+ n[startIndex]+ " ");
                //

                richTextBox1.Find(textBox1.Text, newStartIndex, richTextBox1.TextLength, RichTextBoxFinds.None);
                richTextBox1.SelectionBackColor = Color.Orange;
                //startIndex++;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                clearSelectionColor(); //сбросить подсветку если ничего не введено
                count_symbol = 0; //сбросить счетчики
                current_symbol = 0;
                newStartIndex = 0;
                label1.Visible = false;
                textBox1.BackColor = SystemColors.Window;
                label1.Visible = true;
            }
            if (textBox1.Text.Length != 0)
            {
                label1.Visible = false;
                find_symbol(); // поиск
            }

            if (count_symbol > 0) //нашли хотя бы одно совпадение
            {
                textBox1.BackColor = Color.LightGreen;
                textFindNav(); //навигация по найденным  участкам
                current_symbol = 1; //по умолчанию подсвечиваем первый найденный текст
                button1.Visible = true;
                button2.Visible = true;
                label1.Visible = true;
            }
            else if (count_symbol == 0)
            {
                current_symbol = 0;
                if (textBox1.Text.Length != 0) textBox1.BackColor = Color.LightPink; //что-то ввели, но не найдено - красный цвет
                button1.Visible = false;
                button2.Visible = false;
            }
            label1.Text = current_symbol.ToString() + "/" + count_symbol.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (current_symbol < count_symbol) // переходим на следующее слово, либо на самое первое
            {
                current_symbol++;
                oldStartIndex = newStartIndex;
                newStartIndex++;
                newStartIndex = richTextBox1.Find(textBox1.Text, newStartIndex, richTextBox1.TextLength, RichTextBoxFinds.None) + textBox1.Text.Length;
            }
            else
            {
                current_symbol = 1;
                newStartIndex = 0; //переместили курсор в начало
            }
            find_symbol();
            textFindNav();
            label1.Text = current_symbol.ToString() + "/" + count_symbol.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //дебаг
            int[] arrIndexes = indexes.ToArray();
            for (int i = 0; i < arrIndexes.Length; i++)
            {
                //Console.WriteLine("sas");
                Console.WriteLine(i + " => " + indexes[i]);
            }
            //

            if (current_symbol == 1)
            {
                current_symbol = count_symbol;
            }
            else
            {
                current_symbol--;
                newStartIndex = richTextBox1.Find(textBox1.Text, oldStartIndex, richTextBox1.TextLength, RichTextBoxFinds.None);
            }
            find_symbol();
            textFindNav();
            label1.Text = current_symbol.ToString() + "/" + count_symbol.ToString();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
