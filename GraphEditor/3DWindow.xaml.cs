using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor
{
    public partial class _3DWindow : Window
    {
        static double t = 0;
        static double f = 0;
        static double z = 80;
        static double s = 80;

        double[,] coord = new double[,] {
            { 0, 0, 3, 1 },
            { 0, 0, 0, 1 },
            { 3, 0, 0, 1 },
            { 3, 0, 3, 1 },
            { 0, 3, 3, 1 },
            { 0, 3, 0, 1 },
            { 3, 3, 0, 1 },
            { 3, 3, 3, 1 },
            // Крыша
            { 1.5, 5, 3, 1 },
            { 1.5, 5, 0, 1 },};
        double[,] matrix;

        public _3DWindow()
        {
            InitializeComponent();

            sliderX.Value = 3.14;
            sliderY.Value = 3.14;
            sliderZ.Value = 80;
            sliderS.Value = 13;

            ResetControls();
        }

        public double[,] newCoordGet(double[,] coord, double[,] matrix)
        {
            double[,] result = new double[coord.GetLength(0), 4];
            // Генерация новых координат
            for (int i = 0; i < coord.GetLength(0); i++)
            {
                result[i, 0] = coord[i, 0] * matrix[0, 0] + coord[i, 1] * matrix[1, 0] + coord[i, 2] * matrix[2, 0] + coord[i, 3] * matrix[3, 0];
                result[i, 1] = coord[i, 0] * matrix[0, 1] + coord[i, 1] * matrix[1, 1] + coord[i, 2] * matrix[2, 1] + coord[i, 3] * matrix[3, 1];
                result[i, 2] = coord[i, 0] * matrix[0, 2] + coord[i, 1] * matrix[1, 2] + coord[i, 2] * matrix[2, 2] + coord[i, 3] * matrix[3, 2];
                result[i, 3] = coord[i, 0] * matrix[0, 3] + coord[i, 1] * matrix[1, 3] + coord[i, 2] * matrix[2, 3] + coord[i, 3] * matrix[3, 3];
            }
            // Проверка на однородность
            for (int i = 0; i < coord.GetLength(0); i++)
            {
                if (result[i, 3] != 1)
                {
                    result[i, 0] = result[i, 0] / result[i, 3];
                    result[i, 1] = result[i, 1] / result[i, 3];
                    result[i, 2] = result[i, 2] / result[i, 3];
                    result[i, 3] = result[i, 3] / result[i, 3];
                }
            }
            return result;
        }
        /// <summary>
        /// Обрабатывает значения double такие, как NaN, infinity, либо возвращает само число
        /// </summary>
        /// <param name="number">Число для проверки</param>
        /// <returns>Число, не вызывающее исключений</returns>
        private double VerifyDouble(double number)
        {
            switch (number)
            {
                case double.NaN:
                    return 0.0;
                case double.PositiveInfinity:
                    return double.MaxValue;
                case double.NegativeInfinity:
                    return double.MinValue;
                default:
                    return number;
            }
        }
        /// <summary>
        /// Обновляет положение фигуры на плоскости
        /// </summary>
        private void UpdateRenderedImage()
        {
            t = sliderY.Value;
            f = sliderX.Value;
            z = sliderZ.Value;
            s = sliderS.Value;

            double cosF = Math.Cos(f);
            double sinF = Math.Sin(f);
            double cosT = Math.Cos(t);
            double sinT = Math.Sin(t);
            // Перерасчет таблицы
            matrix = new double[4, 4] {
                { cosF, sinF * sinT, 0, VerifyDouble((sinF * cosT) / z )},
                { 0, cosT, 0, VerifyDouble(sinT / (-z)) },
                { sinF, (cosF * sinT) * -1, 0, VerifyDouble((cosF * cosT) / -z )},
                { 0, 0, 0, 1 } };

            double[,] newCoord = newCoordGet(coord, matrix);
            // Удаление старых линий
            List<Line> toDelete = new List<Line>();
            foreach (object line1 in paintSurface.Children)
            {
                try
                {
                    toDelete.Add(line1 as Line);
                }
                catch { continue; }
            }
            foreach (Line line2 in toDelete)
            {
                paintSurface.Children.Remove(line2);
            }

            //Добавление новых линий
            for (int i = 0; i < newCoord.GetLength(0); i++)
            {
                for (int j = 0; j < newCoord.GetLength(0); j++)
                {
                    if (i != j)
                    {
                        Line line = new Line()
                        {
                            X1 = (newCoord[i, 0] * s) + 600,
                            Y1 = (newCoord[i, 1] * s) + 250,
                            X2 = (newCoord[j, 0] * s) + 600,
                            Y2 = (newCoord[j, 1] * s) + 250
                        };
                        line.Stroke = Brushes.Blue;
                        paintSurface.Children.Add(line);
                    }
                }
            }
        }

        private void SliderYValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRenderedImage();
        }

        private void SliderXValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRenderedImage();
        }

        private void sliderZ_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRenderedImage();
        }

        private void sliderS_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateRenderedImage();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string write = "";
            if (e.IsRepeat) { return; }
            switch (e.Key)
            {
                #region загрузка и сохранение
                case Key.S:
                    //сохранение
                    write = $"{sliderX.Value} {sliderY.Value} {sliderZ.Value} {sliderS.Value}\n";
                    WriteSave(write);
                    break;
                case Key.L:
                    //загрузка
                    double[,] save = ReadSave();
                    if (save != null)
                        //перебор и добавление
                        for (int i = 0; i < save.GetLength(0); i++)
                        {
                            sliderX.Value = save[i, 0];
                            sliderY.Value = save[i, 1];
                            sliderZ.Value = save[i, 2];
                            sliderS.Value = save[i, 3];
                        }
                    //UpdateRenderedImage();
                    break;
                #endregion
                case Key.E:
                    this.Close();
                    break;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {

           // labelMousePoint.Content = $"( {Math.Round(e.GetPosition(this).X - (paintSurface.ActualWidth / 2), 1)}; {Math.Round(e.GetPosition(this).Y - (paintSurface.ActualHeight / 2), 1)})";
        }

        private void ResetControls()
        {
            labelButtons.Content =
@"Вертикальный ползунок: 
Вращение по y
Горизонтальный ползунок: 
Вращение по x

Горизонтальный ползунок: 
Изменение точки зрения (Zc)
Горизонтальный ползунок: 
Изменение масштабирования

E: Выход";
        }

        string saveFilter = "Файлы сохранения редактора (*.3dsave)|*.3dsave|Любые типы файлов (*.*)|*.*";
        //запись в строке: х1,у1,х2,у2 новая строка
        private void WriteSave(string data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = saveFilter;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllLines(saveFileDialog.FileName, data.Split('\n'));
            }
        }
        //массив, строка - одна линия, х1,у1,х2,у2
        private double[,] ReadSave()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = saveFilter;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                //чтение файла
                string[] lines = File.ReadAllLines(openFileDialog.FileName);
                //создание массива, соотвествующего файлу
                double[,] read = new double[lines.Length - 1, 4];
                //перебор всех чситанных строк, за исключением последней (она "")
                for (int j = 0; j < lines.Length - 1; j++)
                {
                    string[] temp = lines[j] != "" ? lines[j].Split(' ') : new string[0];
                    for (int i = 0; i < 4; i++)
                        read[j, i] = Convert.ToDouble(temp[i]);
                }
                return read;
            }
            return null;
        }

        
    }
}

