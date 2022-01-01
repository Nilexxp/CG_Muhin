using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FractTreeWinF
{
    public partial class Form1 : Form
    {
        public Graphics g; //Графика
        public Bitmap map; //Битмап
        public Pen p; //Ручка
        public double angle = Math.PI / 2; //Угол поворота на 90 градусов
        public double ang1 = Math.PI / 4;  //Угол поворота на 45 градусов
        public double ang2 = Math.PI / 6;  //Угол поворота на 30 градусов
        public double maxIter;

        private int colorR = 150;
        private int colorG = 200;
        private int colorB = 200;

        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            map = new Bitmap(pictureBox1.Width, pictureBox1.Height);//Подключаем Битмап
            g = Graphics.FromImage(map); //Подключаем графику
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//Включаем сглаживание
            p = new Pen(Color.DarkCyan);   //Создание типа линий
            maxIter = 10; //стандартное ограничение по количеству итераций
        }

        //Рекурсивная функция отрисовки дерева
        //x и y - координаты родительской вершины
        //a - параметр, который фиксирует количество итераций в рекурсии
        //angle - угол поворота на каждой итерации
        public int DrawTree(double x, double y, double a, double angle)
        {

            if (a > maxIter)
            {
                a *= 0.7; //Меняем параметр a
                //a--;
                //Считаем координаты для вершины-ребенка
                double xnew = Math.Round(x + a * Math.Cos(angle)),
                       ynew = Math.Round(y - a * Math.Sin(angle));

                //рисуем линию между вершинами
                g.DrawLine(p, (float)x, (float)y, (float)xnew, (float)ynew);

                //Переприсваеваем координаты
                x = xnew;
                y = ynew;

                //Вызываем рекурсивную функцию для левого и правого ребенка
                DrawTree(x, y, a, angle - ang2);
                DrawTree(x, y, a, angle + ang2);
            }
            return 0;
        }
        private void ManDrawTree(int x, int y, double i, double angle)
        {
            g.Clear(Color.Transparent);
            p = new Pen(Color.FromArgb(colorR, colorG, colorB));
            switch (random.Next(0, 2))
            {
                case 0:
                    colorR -= 20;
                    if (colorR > 255) colorR = 255;
                    if (colorR < 0) colorR = 0;
                    break;
                case 1:
                    colorG -= 20;
                    if (colorG > 255) colorG = 255;
                    if (colorG < 0) colorG = 0;
                    break;
                case 2:
                    colorB -= 20;
                    if (colorB > 255) colorB = 255;
                    if (colorB < 0) colorB = 0;
                    break;
                default:
                    break;
            }

            //Вызов рекурсивной функции отрисовки дерева
            DrawTree(x, y, i, angle);

            //Переносим картинку из битмапа на picturebox	
            pictureBox1.Image = map;
            pictureBox1.Refresh();
        }
        private void buttonGenTree_Click(object sender, EventArgs e)
        {
            angle = Convert.ToDouble(textBoxStartAngle.Text) * Math.PI / 180;
            ang2 = Convert.ToDouble(textBoxSubAngle.Text) * Math.PI / 180;
            maxIter = Convert.ToDouble(textBoxIter.Text);
            ManDrawTree(300, 450, 200, angle);
        }

        #region
        private void buttonSaveTree_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            // сохраняем текст в файл
            string data = textBoxStartAngle.Text + " " + textBoxSubAngle.Text + " " + textBoxIter.Text;
            WriteSave(data, saveFileDialog1.FileName);
        }

        private void buttonLoadTree_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            
            string[] data = ReadSave(openFileDialog1.FileName)[0].Split(' ');
            textBoxStartAngle.Text = data[0];
            textBoxSubAngle.Text = data[1];
            textBoxIter.Text = data[2];
            ManDrawTree(300, 450, 200, angle);
        }

        private void WriteSave(string data, string filename)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(data);
            }
        }

        private List<string> ReadSave(string filename)
        {

            List<string> data = new List<string>();
            using (StreamReader sr = new StreamReader(filename, System.Text.Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null && line != "")
                {
                    data.Add(line);
                }
            }

            return data;
        }
        #endregion
    }
}
