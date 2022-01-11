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
    /// Логика взаимодействия для WindowSpline.xaml
    /// </summary>
    
    public partial class WindowSpline : Window
    {
        int NumOfPoints = 10;

        Random random = new Random();
        public WindowSpline()
        {
            InitializeComponent();

            double[,] coord = new double[NumOfPoints, 2];
            double[,] coordSpline = new double[NumOfPoints * 10, 2];
            int cnt = 0;
            int cnt2 = 0;

            // Острая линия
            Polygon myPolygon = new Polygon();
            myPolygon.Stroke = Brushes.DarkBlue; 
            myPolygon.StrokeThickness = 2;
            myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
            myPolygon.VerticalAlignment = VerticalAlignment.Center;

            // Заполнение полигонов, заранее определенное количество
            for (int i = 0; i < NumOfPoints; i++)
                myPolygon.Points.Add(new Point(random.Next(10, 700), random.Next(10, 400)));

            paintSurface.Children.Add(myPolygon);

            foreach (Point point in myPolygon.Points)
            {
                coord[cnt, 0] = point.X;
                coord[cnt, 1] = point.Y;
                cnt++;
            }

            for (int i = 0; i < myPolygon.Points.Count; i++)//для каждой из 8 записанных точек
            {
                double[,] temp = Spline(coord, i);//находим 10 точек сплайна. Функция описана ниже
                /*Записываем результат в массив точек сплайна*/
                for (int j = 0; j < 10; j++)
                {
                    coordSpline[cnt2, 0] = temp[j, 0];
                    coordSpline[cnt2, 1] = temp[j, 1];
                    cnt2++;
                }
            }
            // Сплайн
            Polyline mySplinePolygon = new Polyline();
            mySplinePolygon.Stroke = Brushes.DarkCyan; 
            mySplinePolygon.StrokeThickness = 2;
            mySplinePolygon.HorizontalAlignment = HorizontalAlignment.Left;
            mySplinePolygon.VerticalAlignment = VerticalAlignment.Center;
            PointCollection mySplinePointCollection = new PointCollection();

            for (int i = 0; i < NumOfPoints * 10; i++)
            {
                mySplinePointCollection.Add(new Point(coordSpline[i, 0], coordSpline[i, 1]));
            }

            mySplinePointCollection.Add(new Point(coordSpline[0, 0], coordSpline[0, 1]));
            mySplinePolygon.Points = mySplinePointCollection;
            paintSurface.Children.Add(mySplinePolygon);

            ResetControls();
        }

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
        PointCollection list = null;
        Point currentPoint = new Point();


        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e) { if (e.LeftButton == MouseButtonState.Pressed) currentPoint = e.GetPosition(this); }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            labelMousePoint.Content = $"( {Math.Round(e.GetPosition(this).X - (paintSurface.ActualWidth / 2), 1)}; {Math.Round(e.GetPosition(this).Y - (paintSurface.ActualHeight / 2), 1)})";
            Marker marker = new Marker(-1, -1);
            list = new PointCollection();
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (currentPoint != null)
                {
                    Polygon polygon = null;
                    foreach (Object foundPolygon in paintSurface.Children)
                    {
                        try
                        {
                            polygon = (Polygon)foundPolygon;
                        }
                        catch
                        {
                            polygon = null; continue;
                        }
                        foreach (Point foundPoint in polygon.Points)
                        {
                            if (currentPoint.X == foundPoint.X && currentPoint.Y == foundPoint.Y)
                            {
                                list.Add(new Point(e.GetPosition(this).X, e.GetPosition(this).Y));
                            }
                            else
                            {
                                list.Add(foundPoint);
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                Polygon polygon;
                Ellipse elli;
                foreach (Object foundPolygon in paintSurface.Children)
                {
                    try
                    {
                        polygon = (Polygon)foundPolygon;
                    }
                    catch
                    {
                        polygon = null; continue;
                    }
                    foreach (Point foundPoint in polygon.Points)
                    {
                        if (Math.Abs(foundPoint.X - e.GetPosition(this).X) < 10 && Math.Abs(e.GetPosition(this).Y - foundPoint.Y) < 10)
                        {
                            marker.x = foundPoint.X;
                            marker.y = foundPoint.Y;
                            currentPoint = foundPoint;
                            paintSurface.InvalidateVisual();
                        }
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
                        Stroke = new SolidColorBrush(Colors.Blue),
                        Fill = new SolidColorBrush(Colors.Blue),
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
            if (list != null)
            {
                Polygon polygon = null;
                Polyline polyline = null;
                foreach (Shape foundPolygon in paintSurface.Children)
                {
                    try
                    {
                        polygon = (Polygon)foundPolygon;
                        paintSurface.Children.Remove(polygon);
                        paintSurface.InvalidateVisual();
                        break;
                    }
                    catch
                    {
                        polygon = null; continue;
                    }

                }
                Polygon myPolygon = new Polygon();
                myPolygon.Stroke = Brushes.DarkBlue; //основные линии
                myPolygon.StrokeThickness = 2;
                myPolygon.HorizontalAlignment = HorizontalAlignment.Left;
                myPolygon.VerticalAlignment = VerticalAlignment.Center;
                myPolygon.Points = list;
                paintSurface.Children.Add(myPolygon);
                foreach (Polyline foundPolygon in paintSurface.Children)
                {
                    try
                    {
                        polyline = (Polyline)foundPolygon;
                        paintSurface.Children.Remove(polyline);
                        paintSurface.InvalidateVisual();
                        break;
                    }
                    catch
                    {
                        polygon = null; continue;
                    }

                }

                double[,] coord = new double[NumOfPoints, 2];
                double[,] coordSpline = new double[NumOfPoints * 10, 2];
                int cnt = 0;
                int cnt2 = 0;
                foreach (Point point in list)
                {
                    coord[cnt, 0] = point.X;
                    coord[cnt, 1] = point.Y;
                    cnt++;
                }
                for (int i = 0; i < NumOfPoints; i++)//для каждой из 8 записанных точек
                {
                    double[,] temp = Spline(coord, i);//находим 10 точек сплайна. Функция описана ниже
                    /*Записываем результат в массив точек сплайна*/
                    for (int j = 0; j < 10; j++)
                    {
                        coordSpline[cnt2, 0] = temp[j, 0];
                        coordSpline[cnt2, 1] = temp[j, 1];
                        cnt2++;
                    }
                }
                Polyline mySplinePolygon = new Polyline();
                mySplinePolygon.Stroke = Brushes.DarkCyan;
                mySplinePolygon.StrokeThickness = 2;
                mySplinePolygon.HorizontalAlignment = HorizontalAlignment.Left;
                mySplinePolygon.VerticalAlignment = VerticalAlignment.Center;
                PointCollection mySplinePointCollection = new PointCollection();
                for (int i = 0; i < NumOfPoints * 10; i++)
                {
                    mySplinePointCollection.Add(new Point(coordSpline[i, 0], coordSpline[i, 1]));
                }
                mySplinePointCollection.Add(new Point(coordSpline[0, 0], coordSpline[0, 1]));
                mySplinePolygon.Points = mySplinePointCollection;
                paintSurface.Children.Add(mySplinePolygon);
                list = null;
            }
        }

        /// <summary>
        /// Вычсляет Сплайн, на входе все точки и индекс текущей точки
        /// </summary>
        /// <param name="data">Матрица с координатами точек</param>
        /// <param name="curr">Индекс точки, для которой ищется сплайн</param>
        /// <returns>Матрица 10 на 2 с координатами точек сплайна</returns>
        public double[,] Spline(double[,] data, int curr)//функция сплайн, по входящим точкам, и по номеру текущей точки, находит координаты точек сплайна
        {
            int prev = curr - 1;//номер n-1 точки
            int next = curr + 1;//номер n+1 точки
            int nextNext = curr + 2;//номер n+2 точки
            if (curr == 0)//если номер точки 1, то предыдущая точка номер 8
            {
                prev = NumOfPoints - 1;
            }
            if (curr == NumOfPoints - 1)//если номер точки 8, то следующая точка номер 1, а n+2 номер 2
            {
                next = 0;
                nextNext = 1;
            }
            if (curr == NumOfPoints - 2)//если номер точки 7,то n+2 номер 1
            {
                nextNext = 0;
            }

            double[,] output = new double[10, 2];
            // Создание и заполнение массива с конечными коэфициентами
            double[] coorX = {
                a0(data[prev, 0], data[curr, 0], data[next, 0]),
                a1(data[prev, 0], data[next, 0]),
                a2(data[prev, 0], data[curr, 0], data[next, 0]),
                a3(data[prev, 0], data[curr, 0], data[next, 0], data[nextNext, 0])
            };
            double[] coorY = {
                a0(data[prev, 1], data[curr, 1], data[next, 1]),
                a1(data[prev, 1], data[next, 1]),
                a2(data[prev, 1], data[curr, 1], data[next, 1]),
                a3(data[prev, 1], data[curr, 1], data[next, 1], data[nextNext, 1]) 
            };

            double t = 0;//единичный шаг
            
            for (int i = 0; i < 10; i++, t += 0.1)
            {
                //подставляем полученные коэфициенты в уравнение
                output[i, 0] = coorX[0] + coorX[1] * t + coorX[2] * t * t + coorX[3] * t * t * t; //полученная координата х
                output[i, 1] = coorY[0] + coorY[1] * t + coorY[2] * t * t + coorY[3] * t * t * t; //полученная координата у
            }

            return output; // Возвращаем массив с конечными координатами
        }

        #region формулы сплайна
        private double a0(double prev, double curr, double next) { return (prev + (4 * curr) + next) / 6; }
        private double a1(double prev, double next) { return (-prev + next) / 2; }
        private double a2(double prev, double curr, double next) { return (prev - (2 * curr) + next) / 2; }
        private double a3(double prev, double curr, double next, double doubNext) { return (-prev + (3 * curr - (3 * next) + doubNext)) / 6; }
        #endregion

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat) { return; }
            //string write = "";
            switch (e.Key)
            {
                #region загрузка и сохранение
                case Key.S:
                    //сохранение
                    //Line newLine;
                    //foreach (object foundLine in paintSurface.Children)
                    //{
                    //    //попытка преобразовать объект в линию
                    //    try
                    //    {
                    //        newLine = (Line)foundLine;
                    //    }
                    //    //если не получается - переход к следующему объекту
                    //    catch (Exception)
                    //    {
                    //        newLine = null; continue;
                    //    }
                    //    //запись координат линии
                    //    write += newLine.X1 + " " + newLine.Y1 + " " + newLine.X2 + " " + newLine.Y2 + "\n";
                    //}
                    //WriteSave(write);
                    break;
                case Key.L:
                    //загрузка
                    //double[,] save = ReadSave();
                    //if (save != null)
                    //    //перебор и добавление
                    //    for (int i = 0; i < save.GetLength(0); i++)
                    //    {
                    //        //создается новая линия
                    //        Line newline = new Line
                    //        {
                    //            Stroke = new SolidColorBrush(Colors.Black),
                    //            X1 = save[i, 0],
                    //            Y1 = save[i, 1],
                    //            X2 = save[i, 2],
                    //            Y2 = save[i, 3]
                    //        };
                    //        //currentPoint = e.GetPosition(this);
                    //        paintSurface.Children.Add(newline);
                    //    }
                    break;
                #endregion
                case Key.E:
                    //сохранение
                    this.Close();
                    break;
            }


        }

        string saveFilter = "Файлы сохранения сплайна (*.ssave)|*.ssave|Любые типы файлов (*.*)|*.*";

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

        private void ResetControls()
        {
            labelButtons.Content =
@"E: Выход";
        }
    }
}
