using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //
        const double ShiftX = 1.1;
        const double ShiftY = 1.1;
        const double RotateAngle = 0.175;

        List<Line> lineCurrentList;
        Point currentPoint;
        Line lineCurrent;
        int numberLine;
        public int pointIndex;
        string write = "";

        public struct Marker
        {
            public double x;
            public double y;

            public Marker(double value_x, double value_y)
            {
                x = value_x;
                y = value_y;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            paintSurface.Focusable = true;
            paintSurface.Focus();
            labelMousePoint.Content = "";
            labelLine.Content = "";
            ResetControls();

            lineCurrentList = new List<Line>();
            currentPoint = new Point();
            lineCurrent = null;
            numberLine = 0;
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
+: Добавление в группу
-: Очистка группы 
R: Поворот
S: Сохранение 
L: Загрузка";
        }

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) { return; }
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
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.X1 = ((line.X1 - (paintSurface.ActualWidth / 2)) / ShiftX) + (paintSurface.ActualWidth / 2);
                            line.X2 = ((line.X2 - (paintSurface.ActualWidth / 2)) / ShiftX) + (paintSurface.ActualWidth / 2);
                        }
                    }

                    break;
                case Key.Up:
                    //увеличение масштаба по Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = ((lineCurrent.Y1 - (paintSurface.ActualHeight / 2)) * ShiftY) + (paintSurface.ActualHeight / 2);
                        lineCurrent.Y2 = ((lineCurrent.Y2 - (paintSurface.ActualHeight / 2)) * ShiftY) + (paintSurface.ActualHeight / 2);
                    }
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.Y1 = ((line.Y1 - (paintSurface.ActualHeight / 2)) * ShiftY) + (paintSurface.ActualHeight / 2);
                            line.Y2 = ((line.Y2 - (paintSurface.ActualHeight / 2)) * ShiftY) + (paintSurface.ActualHeight / 2);
                        }
                    }
                    break;
                case Key.Right:

                    //увеличение масштаба по X
                    if (lineCurrent != null)
                    {
                        lineCurrent.X1 = ((lineCurrent.X1 - (paintSurface.ActualWidth / 2)) * ShiftX) + (paintSurface.ActualWidth / 2);
                        lineCurrent.X2 = ((lineCurrent.X2 - (paintSurface.ActualWidth / 2)) * ShiftX) + (paintSurface.ActualWidth / 2);
                    }
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.X1 = ((line.X1 - (paintSurface.ActualWidth / 2)) * ShiftX) + (paintSurface.ActualWidth / 2);
                            line.X2 = ((line.X2 - (paintSurface.ActualWidth / 2)) * ShiftX) + (paintSurface.ActualWidth / 2);
                        }
                    }
                    break;
                case Key.Down:
                    //уменьшение масштаба по Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = ((lineCurrent.Y1 - (paintSurface.ActualHeight / 2)) / ShiftY) + (paintSurface.ActualHeight / 2);
                        lineCurrent.Y2 = ((lineCurrent.Y2 - (paintSurface.ActualHeight / 2)) / ShiftY) + (paintSurface.ActualHeight / 2);
                    }
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.Y1 = ((line.Y1 - (paintSurface.ActualHeight / 2)) / ShiftY) + (paintSurface.ActualHeight / 2);
                            line.Y2 = ((line.Y2 - (paintSurface.ActualHeight / 2)) / ShiftY) + (paintSurface.ActualHeight / 2);
                        }
                    }
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
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.Y1 = paintSurface.ActualHeight / 2;
                            line.Y2 = paintSurface.ActualHeight / 2;
                        }
                    }
                    break;
                case Key.N:
                    //проекция на Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.X1 = paintSurface.ActualWidth / 2;
                        lineCurrent.X2 = paintSurface.ActualWidth / 2;
                    }
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.X1 = paintSurface.ActualWidth / 2;
                            line.X2 = paintSurface.ActualWidth / 2;
                        }
                    }
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
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            double tempX1 = (paintSurface.ActualWidth / 2) + (((line.X1 - (paintSurface.ActualWidth / 2)) * Math.Cos(RotateAngle)) + (((line.Y1 - (paintSurface.ActualHeight / 2)) * Math.Sin(RotateAngle))));
                            double tempX2 = (paintSurface.ActualWidth / 2) + (((line.X2 - (paintSurface.ActualWidth / 2)) * Math.Cos(RotateAngle)) + (((line.Y2 - (paintSurface.ActualHeight / 2)) * Math.Sin(RotateAngle))));
                            double tempY1 = (paintSurface.ActualHeight / 2) + (-((line.X1 - (paintSurface.ActualWidth / 2)) * Math.Sin(RotateAngle)) + (((line.Y1 - (paintSurface.ActualHeight / 2)) * Math.Cos(RotateAngle))));
                            double tenpY2 = (paintSurface.ActualHeight / 2) + (-((line.X2 - (paintSurface.ActualWidth / 2)) * Math.Sin(RotateAngle)) + (((line.Y2 - (paintSurface.ActualHeight / 2)) * Math.Cos(RotateAngle))));
                            line.X1 = tempX1;
                            line.X2 = tempX2;
                            line.Y1 = tempY1;
                            line.Y2 = tenpY2;
                            paintSurface.InvalidateVisual();
                        }
                    }
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
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.X1 = (paintSurface.ActualWidth / 2) - (line.X1 - (paintSurface.ActualWidth / 2));
                            line.X2 = (paintSurface.ActualWidth / 2) - (line.X2 - (paintSurface.ActualWidth / 2));
                        }
                    }

                    break;
                case Key.Y:
                    //зеркалирование по Y
                    if (lineCurrent != null)
                    {
                        lineCurrent.Y1 = (paintSurface.ActualHeight / 2) - (lineCurrent.Y1 - (paintSurface.ActualHeight / 2));
                        lineCurrent.Y2 = (paintSurface.ActualHeight / 2) - (lineCurrent.Y2 - (paintSurface.ActualHeight / 2));
                    }
                    if (lineCurrentList.Count != 0)
                    {
                        foreach (Line line in lineCurrentList)
                        {
                            line.Y1 = (paintSurface.ActualHeight / 2) - (line.Y1 - (paintSurface.ActualHeight / 2));
                            line.Y2 = (paintSurface.ActualHeight / 2) - (line.Y2 - (paintSurface.ActualHeight / 2));
                        }
                    }

                    break;
                #endregion
                #region взаимодействие с группой
                case Key.Add:
                    //добавление в группу
                    if (lineCurrent != null) { lineCurrentList.Add(lineCurrent); }
                    break;
                case Key.Subtract:
                    //удаление из группы
                    lineCurrentList.Clear();
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
                        //перебор и добавление
                        for (int i = 0; i < save.GetLength(0); i++)
                        {
                            //создается новая линия
                            Line newline = new Line
                            {
                                Stroke = new SolidColorBrush(Colors.Black),
                                X1 = save[i, 0],
                                Y1 = save[i, 1],
                                X2 = save[i, 2],
                                Y2 = save[i, 3]
                            };
                            //currentPoint = e.GetPosition(this);
                            newline.Name = "line_" + numberLine;
                            paintSurface.Children.Add(newline);
                        }
                    break;
                    #endregion
            }
        }
        string saveFilter = "Файлы сохранения линий (*.lsave)|*.lsave|Любые типы файлов (*.*)|*.*";
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
                    string[] temp = lines[j] != "" ? lines[j].Split(' ') : new string[0] ;
                    for (int i = 0; i < 4; i++)
                        read[j, i] = Convert.ToDouble(temp[i]);
                }
                return read;
            }
            return null;
        }

        private new void KeyUp(object sender, KeyEventArgs e) { ResetControls(); }

        private void CanvasMouseDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) currentPoint = e.GetPosition(this); }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {

            labelMousePoint.Content = $"( {Math.Round(e.GetPosition(this).X - (paintSurface.ActualWidth / 2), 1)}; {Math.Round(e.GetPosition(this).Y - (paintSurface.ActualHeight / 2), 1)})";
            Marker marker = new Marker(-1, -1);
            //нажатие левой клавиши (проведение новой линии)
            if (e.LeftButton == MouseButtonState.Pressed && e.RightButton != MouseButtonState.Pressed)
            {
                Line line = null;
                //если нет текущей линии
                if (lineCurrent != null)
                {
                    //добавление второй точки линии
                    if (pointIndex == 1)
                    {
                        lineCurrent.X2 = e.GetPosition(this).X;
                        lineCurrent.Y2 = e.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }
                    //создание первой точки линии
                    else
                    {
                        lineCurrent.X1 = e.GetPosition(this).X;
                        lineCurrent.Y1 = e.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }

                }
                //если текущая линия существует
                else
                {
                    //для каждого объекта
                    foreach (object foundLine in paintSurface.Children)
                    {
                        //попытка преобразовать объект в линию
                        try
                        {
                            line = (Line)foundLine;
                        }
                        //если не получается - переход к следующему объекту
                        catch (Exception)
                        {
                            line = null; continue;
                        }
                        //если просматриваемая линия - последняя линия - выход из цикла
                        if (line.Name == "line_" + numberLine)
                        {
                            break;
                        }
                        else
                        {
                            line = null;
                        }
                    }
                    //если линии не существует
                    if (line == null)
                    {
                        //создается новая линия
                        line = new Line
                        {
                            Stroke = new SolidColorBrush(Colors.Black),//SystemColors.WindowFrameBrush,
                            X1 = currentPoint.X,
                            Y1 = currentPoint.Y,
                            X2 = e.GetPosition(this).X,
                            Y2 = e.GetPosition(this).Y
                        };
                        currentPoint = e.GetPosition(this);
                        line.Name = "line_" + numberLine;
                        paintSurface.Children.Add(line);
                    }
                    //иначе изменяется вторая кооордината линии
                    else
                    {
                        line.X2 = e.GetPosition(this).X;
                        line.Y2 = e.GetPosition(this).Y;
                        paintSurface.InvalidateVisual();
                    }
                }
            }
            //просто движение мыши (выделение, обработка кругов)
            else
            {
                Line line;
                Ellipse elli;
                //для каждого объекта
                foreach (Object foundLine in paintSurface.Children)
                {

                    try
                    {
                        line = (Line)foundLine;
                    }
                    catch (Exception)
                    {
                        line = null; continue;
                    }
                    //определение порядка концов линии относительно точки начала координат
                    //отметка линии как активной, вывод функции
                    if (Math.Abs(line.X1 - e.GetPosition(this).X) < 20 && Math.Abs(e.GetPosition(this).Y - line.Y1) < 20)
                    {
                        marker.x = line.X1;
                        marker.y = line.Y1;
                        pointIndex = 0;
                        lineCurrent = line;
                        lineCurrent.Stroke = new SolidColorBrush(Colors.LightSeaGreen);
                        double a = lineCurrent.Y1 - lineCurrent.Y2, b = lineCurrent.X2 - lineCurrent.X1;
                        double c = (lineCurrent.X1 - paintSurface.ActualWidth / 2) * (lineCurrent.Y2 - paintSurface.ActualHeight / 2) - (lineCurrent.X2 - paintSurface.ActualWidth / 2) * (lineCurrent.Y1 - paintSurface.ActualHeight / 2);
                        labelLine.Content = $"Уравнение функции: {Math.Round(a, 2)}x{(Math.Round(b, 2) < 0 ? Math.Round(b, 2).ToString() : "+" + Math.Round(b, 2).ToString()) }y{(Math.Round(c, 2) < 0 ? Math.Round(c, 2).ToString() : "+" + Math.Round(c, 2).ToString())}";
                        paintSurface.InvalidateVisual();
                    }
                    else if (Math.Abs(line.X2 - e.GetPosition(this).X) < 20 && Math.Abs(e.GetPosition(this).Y - line.Y2) < 20)
                    {
                        marker.x = line.X2;
                        marker.y = line.Y2;
                        pointIndex = 1;
                        lineCurrent = line;
                        lineCurrent.Stroke = new SolidColorBrush(Colors.LightSeaGreen);
                        double a = lineCurrent.Y1 - lineCurrent.Y2, b = lineCurrent.X2 - lineCurrent.X1;
                        double c = (lineCurrent.X1 - paintSurface.ActualWidth / 2) * (lineCurrent.Y2 - paintSurface.ActualHeight / 2) - (lineCurrent.X2 - paintSurface.ActualWidth / 2) * (lineCurrent.Y1 - paintSurface.ActualHeight / 2);
                        labelLine.Content = $"Уравнение функции: {Math.Round(a, 2)}x{(Math.Round(b, 2) < 0 ? Math.Round(b, 2).ToString() : "+" + Math.Round(b, 2).ToString()) }y{(Math.Round(c, 2) < 0 ? Math.Round(c, 2).ToString() : "+" + Math.Round(c, 2).ToString())}";
                        paintSurface.InvalidateVisual();
                    }

                }
                //если маркер существует
                if (marker.x != -1 && marker.y != -1)
                {
                    //перебор всех объектов в поиске маркера
                    foreach (Object foundEllipse in paintSurface.Children)
                    {
                        //попытка удалить маркер, если есть
                        try
                        {
                            elli = (Ellipse)foundEllipse;
                            paintSurface.Children.Remove(elli);
                            paintSurface.InvalidateVisual();
                            break;
                        }
                        //если маркеров больше нет - дальше
                        catch (Exception)
                        {
                            elli = null; continue;
                        }
                    }
                    //создание маркера заново
                    elli = new Ellipse
                    {
                        Stroke = new SolidColorBrush(Colors.Blue),
                        Fill = new SolidColorBrush(Colors.Blue),
                        Width = 10,
                        Height = 10
                    };
                    Canvas.SetLeft(elli, marker.x - 5);
                    Canvas.SetTop(elli, marker.y - 5);
                    paintSurface.Children.Add(elli);
                }
                //если маркера не существует
                else
                {
                    foreach (Object foundEllipse in paintSurface.Children)
                    {
                        try
                        {
                            elli = (Ellipse)foundEllipse;
                            paintSurface.Children.Remove(elli);
                            if (lineCurrent != null)
                            {
                                lineCurrent.Stroke = new SolidColorBrush(Colors.Black);
                            }
                            lineCurrent = null;
                            labelLine.Content = "";
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
            //двигать линию
            if (e.RightButton == MouseButtonState.Pressed && e.LeftButton != MouseButtonState.Pressed)
            {
                if (lineCurrent != null)
                {
                    double oldX1 = lineCurrent.X1;
                    double oldY1 = lineCurrent.Y1;
                    double oldX2 = lineCurrent.X2;
                    double oldY2 = lineCurrent.Y2;
                    if (pointIndex == 1)
                    {
                        lineCurrent.X2 = e.GetPosition(this).X;
                        lineCurrent.Y2 = e.GetPosition(this).Y;
                        lineCurrent.X1 += e.GetPosition(this).X - oldX2;
                        lineCurrent.Y1 += e.GetPosition(this).Y - oldY2;
                        paintSurface.InvalidateVisual();
                    }
                    else
                    {
                        lineCurrent.X1 = e.GetPosition(this).X;
                        lineCurrent.Y1 = e.GetPosition(this).Y;
                        lineCurrent.X2 += e.GetPosition(this).X - oldX1;
                        lineCurrent.Y2 += e.GetPosition(this).Y - oldY1;
                        paintSurface.InvalidateVisual();
                    }
                }
            }
            //удалять линию
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                Line line;
                if (lineCurrent != null)
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
                        if (line.Name == lineCurrent.Name)
                        {
                            lineCurrent = null;
                            labelLine.Content = "";
                            paintSurface.Children.Remove(line);
                            paintSurface.InvalidateVisual();
                            break;
                        }
                        else
                        {
                            line = null;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Увеличение индекса новой линии при отпускании клавиши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paintSurfaceMouseUp(object sender, MouseButtonEventArgs e) { numberLine++; }

        /// <summary>
        /// при изменении размера поверхности для отрисовки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void paintSurfaceSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rectangle center;
            foreach (object foundCenter in paintSurface.Children)
            {
                try
                {
                    center = (Rectangle)foundCenter;
                    paintSurface.Children.Remove(center);
                    paintSurface.InvalidateVisual();
                    break;
                }
                catch (Exception)
                {
                    center = null; continue;
                }
            }
            center = new Rectangle
            {
                Stroke = new SolidColorBrush(Colors.DarkViolet),
                Fill = new SolidColorBrush(Colors.DarkViolet),
                Width = 4,
                Height = 4
            };
            Canvas.SetLeft(center, (paintSurface.ActualWidth / 2) - 2.5);
            Canvas.SetTop(center, (paintSurface.ActualHeight / 2) - 2.5);
            paintSurface.Children.Add(center);

        }
    }
}
