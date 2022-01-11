using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraphEditor
{
    /// <summary>
    /// Логика взаимодействия для WindowMorph.xaml
    /// </summary>
    public partial class WindowMorph : Window
    {
        const double ShiftX = 1.1;
        const double ShiftY = 1.1;
        const double RotateAngle = 0.175;

        /// <summary>
        /// Создание красной линии по координатам начала и конца
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        Line CreateRedLine(int x1, int y1, int x2, int y2)
        {
            Line line = new Line
            {
                Stroke = SystemColors.WindowFrameBrush,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };
            line.Stroke = new SolidColorBrush(Colors.Red);
            line.Name = "line_" + numberLine + 101;
            paintSurface.Children.Add(line);

            return line;
        }

        /// <summary>
        /// Создание синей линии по координатам начала и конца
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        Line CreateBlueLine(int x1, int y1, int x2, int y2)
        {
            Line line = new Line
            {
                Stroke = SystemColors.WindowFrameBrush,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2
            };
            line.Stroke = new SolidColorBrush(Colors.Blue);
            line.Name = "line_" + numberLine + 100;
            paintSurface.Children.Add(line);

            return line;
        }

        /// <summary>
        /// Создание новой линии
        /// </summary>
        /// <param name="red">Первая линия</param>
        /// <param name="blue">Вторая линия</param>
        /// <param name="t">Коэфициента, первая к второй линии</param>
        /// <returns>Отрисованная линия</returns>
        Polyline CreateMidLine(Line red, Line blue, double t)
        {
            Point upperPoint = new Point((1 - t) * red.X1 + t * blue.X1, (1 - t) * red.Y1 + t * blue.Y1);
            Point lowerpoint = new Point((1 - t) * red.X2 + t * blue.X2, (1 - t) * red.Y2 + t * blue.Y2);
            Polyline polyline = new Polyline();

            // Получение цветов от линий
            Color colorRedLine = ((SolidColorBrush)red.Stroke).Color;
            Color colorBlueLine = ((SolidColorBrush)blue.Stroke).Color;
            // Получение среднего цвета
            Color color = new Color();
            color.A = 255;
            color.R = (byte)((1 - t) * colorRedLine.R + t * colorBlueLine.R);
            color.G = (byte)((1 - t) * colorRedLine.G + t * colorBlueLine.G);
            color.B = (byte)((1 - t) * colorRedLine.B + t * colorBlueLine.B);
            // Заолнение нового цвета
            polyline.Stroke = new SolidColorBrush(color);

            // Добавление точек в линию
            polyline.Points.Add(upperPoint);
            polyline.Points.Add(lowerpoint);
            // Отрисовка линии
            paintSurface.Children.Add(polyline);

            return polyline;
        }

        public WindowMorph()
        {
            InitializeComponent();
            ResetControls();
            // Первая линия (красная)
            Line red = CreateRedLine(200, 200, 100, 100);
            // Вторая линия (синяя)
            Line blue = CreateBlueLine(250, 200, 150, 100);
            // Создание новой линии
            CreateMidLine(red, blue, sliderT.Value / 100);            
        }
        Point currentPoint = new Point();
        Line lineCurrent = null;
        int numberLine = 0;

        public struct Marker
        {
            public double x;
            public double y;
            public Color color;

            public Marker(double value_x, double value_y)
            {
                x = value_x;
                y = value_y;
                color = Colors.Blue;
            }
        }

        public int pointIndex;

        // получение положения мыши при нажатии левой кнопки мыши
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) currentPoint = e.GetPosition(this); }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            labelMousePoint.Content = $"( {Math.Round(e.GetPosition(this).X - (paintSurface.ActualWidth / 2), 1)}; {Math.Round(e.GetPosition(this).Y - (paintSurface.ActualHeight / 2), 1)})";
            Marker marker = new Marker(-1, -1);
            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton != MouseButtonState.Pressed)
            {
                Line line = null;
                if (lineCurrent != null)
                {
                    if (pointIndex == 1)
                    {
                        lineCurrent.X2 = e.GetPosition(this).X;
                        lineCurrent.Y2 = e.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }
                    else
                    {
                        lineCurrent.X1 = e.GetPosition(this).X;
                        lineCurrent.Y1 = e.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }

                }
                else
                {


                    foreach (object foundLine in paintSurface.Children)
                    {
                        try
                        {
                            line = (Line)foundLine;
                        }
                        catch (Exception)
                        {
                            line = null; continue;
                        }

                        if (line.Name == "line_" + numberLine)
                            break;
                        else
                            line = null;
                    }
                    if (line != null)
                    {
                        line.X2 = e.GetPosition(this).X;
                        line.Y2 = e.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }
                }
            }
            else
            {
                Line line;
                Ellipse elli;
                foreach (Object foundLine in paintSurface.Children)
                {

                    try
                    {
                        line = (Line)foundLine;
                    }
                    catch 
                    {
                        line = null; continue;
                    }
                    if (Math.Abs(line.X1 - e.GetPosition(this).X) < 20 && Math.Abs(e.GetPosition(this).Y - line.Y1) < 20)
                    {
                        marker.x = line.X1;
                        marker.y = line.Y1;
                        marker.color = ((SolidColorBrush)line.Stroke).Color;
                        pointIndex = 0;
                        lineCurrent = line;
                        paintSurface.InvalidateVisual();
                    }
                    else if (Math.Abs(line.X2 - e.GetPosition(this).X) < 20 && Math.Abs(e.GetPosition(this).Y - line.Y2) < 20)
                    {
                        marker.x = line.X2;
                        marker.y = line.Y2;
                        marker.color = ((SolidColorBrush)line.Stroke).Color;
                        pointIndex = 1;
                        lineCurrent = line;
                        paintSurface.InvalidateVisual();
                    }

                }

                if (marker.x != -1 && marker.y != -1)
                {
                    foreach (Object foundEllipse in paintSurface.Children)
                    {
                        try
                        {
                            elli = (Ellipse)foundEllipse;
                            paintSurface.Children.Remove(elli);
                            paintSurface.InvalidateVisual();
                            break;
                        }
                        catch (Exception)
                        {
                            elli = null; continue;
                        }
                    }
                    elli = new Ellipse
                    {
                        Stroke = new SolidColorBrush(marker.color),
                        Fill = new SolidColorBrush(marker.color),
                        Width = 10,
                        Height = 10
                    };
                    Canvas.SetLeft(elli, marker.x - 5);
                    Canvas.SetTop(elli, marker.y - 5);
                    paintSurface.Children.Add(elli);
                }
                else
                {
                    foreach (Object foundEllipse in paintSurface.Children)
                    {
                        try
                        {
                            elli = (Ellipse)foundEllipse;
                            paintSurface.Children.Remove(elli);
                            lineCurrent = null;
                            paintSurface.InvalidateVisual();
                            break;
                        }
                        catch (Exception)
                        {
                            elli = null; continue;
                        }
                    }
                }
            }
        }

        private void paintSurface_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LinesChanged(sliderT.Value / 100);
        }

        private void ResetControls()
        {
            labelButtons.Content =
@"X: Зеркалирование по OX   
Y: Зеркалирование по OY 
→: увеличение по OX 
←: уменьшение по OX 
↑: увеличение по OY 
↓: уменьшение по OY 
N: Проекция на OX 
M: Проекция на OY 
R: Поворот
S: Сохранение 
L: Загрузка
Изменяйте значение t 
на слайдере снизу окна

E: Выход из окна морфинга";
        }

        string saveFilter = "Файлы сохранения морфинга (*.msave)|*.msave|Любые типы файлов (*.*)|*.*";

        //запись в строке: х1,у1,х2,у2 новая строка
        private void WriteSave(string data)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = saveFilter;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllLines(saveFileDialog.FileName, data.Split('\n'));
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) { return; }
            string write = "";
            switch (e.Key)
            {
                #region масштабирования
                case Key.Left:
                    //уменьшение масштаба по X
                    if (lineCurrent != null)
                    {
                        lineCurrent.X1 = ((lineCurrent.X1 - (paintSurface.ActualWidth / 2)) / ShiftX) + (paintSurface.ActualWidth / 2);
                        lineCurrent.X2 = ((lineCurrent.X2 - (paintSurface.ActualWidth / 2)) / ShiftX) + (paintSurface.ActualWidth / 2);
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                case Key.Up:
                    //увеличение масштаба по Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = ((lineCurrent.Y1 - (paintSurface.ActualHeight / 2)) * ShiftY) + (paintSurface.ActualHeight / 2);
                        lineCurrent.Y2 = ((lineCurrent.Y2 - (paintSurface.ActualHeight / 2)) * ShiftY) + (paintSurface.ActualHeight / 2);
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                case Key.Right:

                    //увеличение масштаба по X
                    if (lineCurrent != null)
                    {
                        lineCurrent.X1 = ((lineCurrent.X1 - (paintSurface.ActualWidth / 2)) * ShiftX) + (paintSurface.ActualWidth / 2);
                        lineCurrent.X2 = ((lineCurrent.X2 - (paintSurface.ActualWidth / 2)) * ShiftX) + (paintSurface.ActualWidth / 2);
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                case Key.Down:
                    //уменьшение масштаба по Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = ((lineCurrent.Y1 - (paintSurface.ActualHeight / 2)) / ShiftY) + (paintSurface.ActualHeight / 2);
                        lineCurrent.Y2 = ((lineCurrent.Y2 - (paintSurface.ActualHeight / 2)) / ShiftY) + (paintSurface.ActualHeight / 2);
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                #endregion
                #region проекции
                case Key.M:
                    //проекция на X
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = paintSurface.ActualHeight / 2;
                        lineCurrent.Y2 = paintSurface.ActualHeight / 2;
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                case Key.N:
                    //проекция на Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.X1 = paintSurface.ActualWidth / 2;
                        lineCurrent.X2 = paintSurface.ActualWidth / 2;
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                #endregion
                #region поворот
                case Key.R:
                    //поворот
                    if (lineCurrent != null)
                    {
                        double tempX1 = (paintSurface.ActualWidth / 2) + (((lineCurrent.X1 - (paintSurface.ActualWidth / 2)) * Math.Cos(RotateAngle)) + (((lineCurrent.Y1 - (paintSurface.ActualHeight / 2)) * Math.Sin(RotateAngle))));
                        double tempX2 = (paintSurface.ActualWidth / 2) + (((lineCurrent.X2 - (paintSurface.ActualWidth / 2)) * Math.Cos(RotateAngle)) + (((lineCurrent.Y2 - (paintSurface.ActualHeight / 2)) * Math.Sin(RotateAngle))));
                        double tempY1 = (paintSurface.ActualHeight / 2) + (-((lineCurrent.X1 - (paintSurface.ActualWidth / 2)) * Math.Sin(RotateAngle)) + (((lineCurrent.Y1 - (paintSurface.ActualHeight / 2)) * Math.Cos(RotateAngle))));
                        double tenpY2 = (paintSurface.ActualHeight / 2) + (-((lineCurrent.X2 - (paintSurface.ActualWidth / 2)) * Math.Sin(RotateAngle)) + (((lineCurrent.Y2 - (paintSurface.ActualHeight / 2)) * Math.Cos(RotateAngle))));
                        lineCurrent.X1 = tempX1;
                        lineCurrent.X2 = tempX2;
                        lineCurrent.Y1 = tempY1;
                        lineCurrent.Y2 = tenpY2;
                        paintSurface.InvalidateVisual();
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                #endregion
                #region зеркалирования
                case Key.X:
                    //зеркалирование по X
                    if (lineCurrent != null)
                    {
                        lineCurrent.X1 = (paintSurface.ActualWidth / 2) - (lineCurrent.X1 - (paintSurface.ActualWidth / 2));
                        lineCurrent.X2 = (paintSurface.ActualWidth / 2) - (lineCurrent.X2 - (paintSurface.ActualWidth / 2));
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                case Key.Y:
                    //зеркалирование по Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = (paintSurface.ActualHeight / 2) - (lineCurrent.Y1 - (paintSurface.ActualHeight / 2));
                        lineCurrent.Y2 = (paintSurface.ActualHeight / 2) - (lineCurrent.Y2 - (paintSurface.ActualHeight / 2));
                    }
                    LinesChanged(sliderT.Value / 100);
                    break;
                #endregion
                #region загрузка и сохранение
                case Key.S:
                    //сохранение
                    Line newLine;
                    foreach (object foundLine in paintSurface.Children)
                    {
                        //попытка преобразовать объект в линию
                        try
                        {
                            newLine = (Line)foundLine;
                        }
                        //если не получается - переход к следующему объекту
                        catch (Exception)
                        {
                            newLine = null; continue;
                        }
                        //запись координат линии
                        write += newLine.X1 + " " + newLine.Y1 + " " + newLine.X2 + " " + newLine.Y2 + "\n";
                    }
                    WriteSave(write);
                    break;
                case Key.L:
                    //загрузка
                    double[,] save = ReadSave();
                    if (save != null)

                    //поиск и перезаполнение данных
                    {
                        Line line1 = null;
                        Line line2 = null;
                        try
                        {
                            foreach (object line in paintSurface.Children)
                            {
                                try
                                {
                                    if (line1 == null)
                                    {
                                        line1 = (Line)line;
                                    }
                                    else
                                    {
                                        line2 = (Line)line;
                                        break;
                                    }
                                }
                                catch
                                {
                                    continue;
                                }

                            }
                            paintSurface.Children.Remove(line1);
                            paintSurface.Children.Remove(line2);
                            line1 = CreateRedLine((int)save[0, 0], (int)save[0, 1], (int)save[0, 2], (int)save[0, 3]);
                            line2 = CreateBlueLine((int)save[1, 0], (int)save[1, 1], (int)save[1, 2], (int)save[1, 3]);
                            break;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    break;
                #endregion
                case Key.E:
                    //сохранение
                    this.Close();
                    break;
            }
        }

        private void sliderT_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LinesChanged(sliderT.Value / 100);
        }

        private void LinesChanged(double t)
        {
            numberLine++;
            Polyline polyline1;
            Line line1 = null;
            Line line2 = null;
            foreach (object foundPolyline in paintSurface.Children)
            {
                try
                {
                    polyline1 = (Polyline)foundPolyline;
                    paintSurface.Children.Remove(polyline1);
                    foreach (object line in paintSurface.Children)
                    {
                        try
                        {
                            if (line1 == null)
                            {
                                line1 = (Line)line;
                            }
                            else
                            {
                                line2 = (Line)line;
                                break;
                            }
                        }
                        catch
                        {
                            continue;
                        }

                    }
                    CreateMidLine(line1, line2, sliderT.Value / 100);
                    break;
                }
                catch (Exception)
                {
                    polyline1 = null; continue;
                }
            }
        }
    }
}
