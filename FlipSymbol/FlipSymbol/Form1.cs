using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlipSymbol
{
    public partial class Form1 : Form
    {

        public string FilePic;
        public string FileText;
        public int CountText;
        public Bitmap bPic;
        public List<byte> bList = new List<byte>();

        public string path;
        public int IMAGE_SIZE = 0; // дополнительные переменные
        public int TEXT_SIZE = 0;
        public int MAX_TEXT_SIZE = 0;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void btnImgSel_Click(object sender, EventArgs e)
        {
            OpenFileDialog dPic = new OpenFileDialog();
            dPic.Filter = "Файлы изображений (*.bmp)|*.bmp|" +
                          "Файлы изображений (*.png)|*.png|" +
                          "Файлы изображений (*.jpeg)|*.jpeg";
            if (dPic.ShowDialog() == DialogResult.OK)
            {
                FilePic = dPic.FileName;
            }
            else
            {
                FilePic = "";
                return;
            }

            FileStream rFile;
            try
            {
                rFile = new FileStream(FilePic, FileMode.Open); //открываем поток
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } if (rFile.Length != 0)
            {
                bPic = new Bitmap(rFile);
                pictureBox1.Image = bPic;
                IMAGE_SIZE = (bPic.Width * bPic.Height);
                MAX_TEXT_SIZE = IMAGE_SIZE / 8;
                label5.Text = IMAGE_SIZE.ToString();
                label9.Text = MAX_TEXT_SIZE.ToString();
                textBox2.Text = dPic.FileName;
                rFile.Close();
            }
            else MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnTxtSel_Click(object sender, EventArgs e)
        {
            OpenFileDialog dText = new OpenFileDialog();
            dText.Filter = "Текстовые файлы (*.txt)|*.txt";
            if (dText.ShowDialog() == DialogResult.OK)
            {
                FileText = dText.FileName;
                textBox1.Text = FileText;
                richTextBox1.Text = File.ReadAllText(FileText);
            }
            else
            {
                FileText = "";
                textBox1.Text = FileText;
                return;
            }
            
            FileStream rText;
            try
            {
                rText = new FileStream(FileText, FileMode.Open); //открываем поток
            }
            catch (IOException)
            {
                MessageBox.Show("Ошибка открытия файла", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            BinaryReader bText = new BinaryReader(rText, Encoding.ASCII);
            //byte[] bytes = Encoding.GetEncoding(1251).GetBytes(richTextBox1.Text);
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    bList.Add(bytes[i]);
            //}
            //while (bText.PeekChar() != -1)
            //{ //считали весь текстовый файл для шифрования в лист байт
            //    bList.Add(bText.ReadByte());
            //}
            //richTextBox1.Text = bList.ToString();
            //CountText = bList.Count; // в CountText - количество в байтах текста, который нужно закодировать
            bText.Close();
            TEXT_SIZE = richTextBox1.Text.Length;
            //label14.Text = CountText.ToString();
        
    }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            bList.Clear();
            byte[] bytes = Encoding.GetEncoding(1251).GetBytes(richTextBox1.Text);
            for (int i = 0; i < bytes.Length; i++)
            {
                bList.Add(bytes[i]);
            }
            //while (bText.PeekChar() != -1)
            //{ //считали весь текстовый файл для шифрования в лист байт
            //    bList.Add(bText.ReadByte());
            //}
            //richTextBox1.Text = bList.ToString();
            CountText = bList.Count; // в CountText - количество в байтах текста, который нужно закодировать

            TEXT_SIZE = richTextBox1.Text.Length;
            int s = richTextBox1.Text.Length;
            int b = s * 8;
            label10.Text = s.ToString();
            label11.Text = b.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (bPic == null)
            {
                MessageBox.Show("Выберите изображение!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (richTextBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите текст!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                label11.Text = (bList.Count * 8).ToString();
                //проверяем, поместиться ли исходный текст в картинке
                if (Convert.ToInt16(label11.Text) > ((bPic.Width * bPic.Height)) - 4)
                {
                    MessageBox.Show("Выбранная картинка мала для размещения выбранного текста", "Информация", MessageBoxButtons.OK);
                    return;
                }

                //проверяем, может быть картинка уже зашифрована
                if (isEncryption(bPic))
                {
                    MessageBox.Show("Файл уже зашифрован", "Информация", MessageBoxButtons.OK);
                    return;
                }

                byte[] Symbol = Encoding.GetEncoding(1251).GetBytes("/");
                BitArray ArrBeginSymbol = ByteToBit(Symbol[0]);
                Color curColor = bPic.GetPixel(0, 0);
                BitArray tempArray = ByteToBit(curColor.R);
                tempArray[0] = ArrBeginSymbol[0];
                tempArray[1] = ArrBeginSymbol[1];
                byte nR = BitToByte(tempArray);

                tempArray = ByteToBit(curColor.G);
                tempArray[0] = ArrBeginSymbol[2];
                tempArray[1] = ArrBeginSymbol[3];
                tempArray[2] = ArrBeginSymbol[4];
                byte nG = BitToByte(tempArray);

                tempArray = ByteToBit(curColor.B);
                tempArray[0] = ArrBeginSymbol[5];
                tempArray[1] = ArrBeginSymbol[6];
                tempArray[2] = ArrBeginSymbol[7];
                byte nB = BitToByte(tempArray);

                Color nColor = Color.FromArgb(nR, nG, nB);
                bPic.SetPixel(0, 0, nColor);
                //то есть в первом пикселе будет символ /, который говорит о том, что картика зашифрована

                WriteCountText(CountText, bPic); //записываем количество символов для шифрования

                int index = 0;
                bool st = false;
                for (int i = 4; i < bPic.Width; i++)
                {
                    for (int j = 0; j < bPic.Height; j++)
                    {
                        Color pixelColor = bPic.GetPixel(i, j);
                        if (index == bList.Count)
                        {
                            st = true;
                            break;
                        }
                        BitArray colorArray = ByteToBit(pixelColor.R);
                        BitArray messageArray = ByteToBit(bList[index]);
                        colorArray[0] = messageArray[0]; //меняем
                        colorArray[1] = messageArray[1]; // в нашем цвете биты
                        byte newR = BitToByte(colorArray);

                        colorArray = ByteToBit(pixelColor.G);
                        colorArray[0] = messageArray[2];
                        colorArray[1] = messageArray[3];
                        colorArray[2] = messageArray[4];
                        byte newG = BitToByte(colorArray);

                        colorArray = ByteToBit(pixelColor.B);
                        colorArray[0] = messageArray[5];
                        colorArray[1] = messageArray[6];
                        colorArray[2] = messageArray[7];
                        byte newB = BitToByte(colorArray);

                        Color newColor = Color.FromArgb(newR, newG, newB);
                        bPic.SetPixel(i, j, newColor);
                        index++;
                    }
                    if (st)
                    {
                        break;
                    }
                }
                pictureBox1.Image = bPic;

                String sFilePic;
                SaveFileDialog dSavePic = new SaveFileDialog();
                dSavePic.Filter = "Файлы изображений(*.bmp)| *.bmp | " +
                                  "Файлы изображений (*.png)|*.png|" +
                                  "Файлы изображений (*.jpeg)|*.jpeg";
                if (dSavePic.ShowDialog() == DialogResult.OK)
                {
                    sFilePic = dSavePic.FileName;
                }
                else
                {
                    sFilePic = "";
                    return;
                };

                FileStream wFile;
                try
                {
                    wFile = new FileStream(sFilePic, FileMode.Create); //открываем поток на запись результатов
                }
                catch (IOException)
                {
                    MessageBox.Show("Ошибка открытия файла на запись", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bPic.Save(wFile, System.Drawing.Imaging.ImageFormat.Bmp);
                wFile.Close(); //закрываем поток

                string key = (CountText * 8).ToString();
                DialogResult dialogResult = MessageBox.Show("Вы хотите сохранить ключь " + key + "?", "Сохранить", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    saveFileDialog1 = new SaveFileDialog();
                    saveFileDialog1.Filter = "Text file (*.rgz)|*.txt";
                    saveFileDialog1.FilterIndex = 2;
                    saveFileDialog1.RestoreDirectory = true;
                    //string fl = saveFileDialog1.FileName;
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (sfd.ShowDialog() == DialogResult.OK)
                        File.WriteAllText(sfd.FileName, key);

                    //                        System.IO.File.WriteAllText(filename, textBox1.Text);
                    //do something
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

                richTextBox1.Clear();
                bPic = null;
                textBox2.Clear();
                textBox1.Clear();
                pictureBox1.Image = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (bPic != null)
            {
                if (!isEncryption(bPic))
                {
                    MessageBox.Show("Файл не зашифрован", "Информация", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    panel1.Visible = true;
                    radioBtnWrite.Checked = true;
                }
                //KeyForm keyForm = new KeyForm();
                //keyForm.Show();
                //Decod();
            }
            else MessageBox.Show("Выберите файл!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void Decod()
        {
            //CountText = 10;
                int countSymbol = ReadCountText(bPic); //считали количество зашифрованных символов
                byte[] message = new byte[countSymbol];
                int index = 0;
                bool st = false;
                for (int i = 4; i < bPic.Width; i++)
                {
                    for (int j = 0; j < bPic.Height; j++)
                    {
                        Color pixelColor = bPic.GetPixel(i, j);
                        if (index == message.Length)
                        {
                            st = true;
                            break;
                        }
                        BitArray colorArray = ByteToBit(pixelColor.R);
                        BitArray messageArray = ByteToBit(pixelColor.R); ;
                        messageArray[0] = colorArray[0];
                        messageArray[1] = colorArray[1];

                        colorArray = ByteToBit(pixelColor.G);
                        messageArray[2] = colorArray[0];
                        messageArray[3] = colorArray[1];
                        messageArray[4] = colorArray[2];

                        colorArray = ByteToBit(pixelColor.B);
                        messageArray[5] = colorArray[0];
                        messageArray[6] = colorArray[1];
                        messageArray[7] = colorArray[2];
                        message[index] = BitToByte(messageArray);
                        index++;
                    }
                    if (st)
                    {
                        break;
                    }
                }
                string strMessage = Encoding.GetEncoding(1251).GetString(message);

                richTextBox1.Text = strMessage;
        }

        private BitArray ByteToBit(byte src)
        {
            BitArray bitArray = new BitArray(8);
            bool st = false;
            for (int i = 0; i < 8; i++)
            {
                if ((src >> i & 1) == 1)
                {
                    st = true;
                }
                else st = false;
                bitArray[i] = st;
            }
            return bitArray;
        }

        private byte BitToByte(BitArray scr)
        {
            byte num = 0;
            for (int i = 0; i < scr.Count; i++)
                if (scr[i] == true)
                    num += (byte)Math.Pow(2, i);
            return num;
        }

        private bool isEncryption(Bitmap scr)
        {
            byte[] rez = new byte[1];
            Color color = scr.GetPixel(0, 0);
            BitArray colorArray = ByteToBit(color.R); //получаем байт цвета и преобразуем в массив бит
            BitArray messageArray = ByteToBit(color.R); ;//инициализируем результирующий массив бит
            messageArray[0] = colorArray[0];
            messageArray[1] = colorArray[1];

            colorArray = ByteToBit(color.G);//получаем байт цвета и преобразуем в массив бит
            messageArray[2] = colorArray[0];
            messageArray[3] = colorArray[1];
            messageArray[4] = colorArray[2];

            colorArray = ByteToBit(color.B);//получаем байт цвета и преобразуем в массив бит
            messageArray[5] = colorArray[0];
            messageArray[6] = colorArray[1];
            messageArray[7] = colorArray[2];
            rez[0] = BitToByte(messageArray); //получаем байт символа, записанного в 1 пикселе
            string m = Encoding.GetEncoding(1251).GetString(rez);
            if (m == "/")
            {
                return true;
            }
            else return false;
        }

        private void WriteCountText(int count, Bitmap src)
        {
            byte[] CountSymbols = Encoding.GetEncoding(1251).GetBytes(count.ToString());
            for (int i = 0; i < CountSymbols.Length; i++)
            {
                BitArray bitCount = ByteToBit(CountSymbols[i]); //биты количества символов
                Color pColor = src.GetPixel(0, i + 1); //1, 2, 3 пикселы
                BitArray bitsCurColor = ByteToBit(pColor.R); //бит цветов текущего пикселя
                bitsCurColor[0] = bitCount[0];
                bitsCurColor[1] = bitCount[1];
                byte nR = BitToByte(bitsCurColor); //новый бит цвета пиксея

                bitsCurColor = ByteToBit(pColor.G);//бит бит цветов текущего пикселя
                bitsCurColor[0] = bitCount[2];
                bitsCurColor[1] = bitCount[3];
                bitsCurColor[2] = bitCount[4];
                byte nG = BitToByte(bitsCurColor);//новый цвет пиксея

                bitsCurColor = ByteToBit(pColor.B);//бит бит цветов текущего пикселя
                bitsCurColor[0] = bitCount[5];
                bitsCurColor[1] = bitCount[6];
                bitsCurColor[2] = bitCount[7];
                byte nB = BitToByte(bitsCurColor);//новый цвет пиксея

                Color nColor = Color.FromArgb(nR, nG, nB); //новый цвет из полученных битов
                src.SetPixel(0, i + 1, nColor); //записали полученный цвет в картинку
            }
        }

        private int ReadCountText(Bitmap src)
        {
            byte[] CountSymbols = Encoding.GetEncoding(1251).GetBytes(CountText.ToString());
            int count = CountSymbols.Length;
            byte[] rez = new byte[count]; //массив на 3 элемента, т.е. максимум 999 символов шифруется
            for (int i = 0; i < count; i++)
            {
                Color color = src.GetPixel(0, i + 1); //цвет 1, 2, 3 пикселей 
                BitArray colorArray = ByteToBit(color.R); //биты цвета
                BitArray bitCount = ByteToBit(color.R); ; //инициализация результирующего массива бит
                bitCount[0] = colorArray[0];
                bitCount[1] = colorArray[1];

                colorArray = ByteToBit(color.G);
                bitCount[2] = colorArray[0];
                bitCount[3] = colorArray[1];
                bitCount[4] = colorArray[2];

                colorArray = ByteToBit(color.B);
                bitCount[5] = colorArray[0];
                bitCount[6] = colorArray[1];
                bitCount[7] = colorArray[2];
                rez[i] = BitToByte(bitCount);
            }
            string m = Encoding.GetEncoding(1251).GetString(rez);
            return Convert.ToInt32(m, 10);
        }

        private void radioBtnSel_CheckedChanged(object sender, EventArgs e)
        {
            tbWriteKey.Enabled = false;
            tbSelKey.Enabled = true;
        }

        private void radioBtnWrite_CheckedChanged(object sender, EventArgs e)
        {
            tbSelKey.Enabled = false;
            tbWriteKey.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!tbWriteKey.Text.Equals(""))
            {
                int count = Convert.ToInt32(tbWriteKey.Text);
                CountText = count / 8;
                Decod();
                panel1.Visible = false;
                tbWriteKey.Clear();
            }
            else MessageBox.Show("Введите или выберите ключь!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void tbSelKey_DoubleClick(object sender, EventArgs e)
        {

            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string path;
            string kol = "";

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            path = openFileDialog1.FileName;
                            textBox1.Text = path;
                            string key = System.IO.File.ReadAllText(path);
                            char[] chars = new char[key.Length];
                            chars = key.ToCharArray();
                            for (int i = 0; i < key.Length; i++)
                            {
                                if (chars[i].Equals(' '))
                                {
                                    break;
                                }
                                else
                                    kol = kol.Insert(kol.Length, chars[i].ToString());
                            }
                            tbWriteKey.Text = kol;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
    }
}
