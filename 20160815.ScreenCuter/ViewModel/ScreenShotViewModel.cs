using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _20160815.ScreenCuter
{
    public class ScreenShotViewModel:ScreenShotModel
    {
        #region Constructor

        public double x;
        public double y;

        public double newx;
        public double newy;

        public double width;
        public double height;

        public bool isMouseDown = false;

        public ScreenShotViewModel(ScreenShot screenShot)
        {
            MainWindow m = new MainWindow();

            ScreenShot = screenShot;
            ScreenShot.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    ScreenShot.Close();
                    
                    m.MainViewModel.MainWindow.Visibility = Visibility.Visible;
                }
            };
            ScreenShot.MouseRightButtonUp += (sender, e) =>
            {
                ScreenShot.Close();
                m.MainViewModel.MainWindow.Visibility = Visibility.Visible;
            };
            Screen(ScreenShot, ScreenShot.canvas, m.MainViewModel.MainWindow);
        }
        
            
        #endregion

        #region Members

        public ScreenShot ScreenShot { get; set; }

        #endregion

        #region Event

        public event EventHandler ScreenShotEvent;

        #endregion

        #region Methods

        /// <summary>
        /// Ekran görüntüsü olarak nerenin alınacağı seçilir
        /// </summary>
        /// <param name="window">Windowun ismi</param>
        /// <param name="canvas">Window daki Canvasın ismi</param>
        public void Screen(Window window, Canvas canvas, Window parentWindow)
        {
            window.MouseLeftButtonDown += (sender, e) =>
            {
                isMouseDown = true;
                x = e.GetPosition(null).X;
                y = e.GetPosition(null).Y;
            };
            window.PreviewMouseMove += (sender, e) =>
            {
                ScreenShot.Cursor = Cursors.Cross;

                if (isMouseDown)
                {

                    double cutx = e.GetPosition(null).X;
                    double cuty = e.GetPosition(null).Y;

                    System.Windows.Shapes.Rectangle tmpRectangle = new System.Windows.Shapes.Rectangle();

                    Panel.SetZIndex(tmpRectangle, 1);
                    Panel.SetZIndex(ScreenShot.canvas, 0);
                    SolidColorBrush tmpColor = new SolidColorBrush(Colors.Orange);
                    tmpRectangle.Stroke = tmpColor;
                    tmpRectangle.StrokeThickness = 5;
                    tmpRectangle.Width = Math.Abs(cutx - x);
                    tmpRectangle.Height = Math.Abs(cuty - y);


                    if (cutx < x)
                    {
                        newx = cutx;
                        Canvas.SetLeft(tmpRectangle, cutx);
                        if (cuty < y)
                        {
                            newy = cuty;
                            Canvas.SetTop(tmpRectangle, cuty);
                        }
                        else
                        {
                            newy = y;
                            Canvas.SetTop(tmpRectangle, y);
                        }
                    }

                    else
                    {
                        newx = x;
                        Canvas.SetLeft(tmpRectangle, x);
                        if (cuty < y)
                        {
                            newy = cuty;
                            Canvas.SetTop(tmpRectangle, cuty);
                        }
                        else
                        {
                            newy = y;
                            Canvas.SetTop(tmpRectangle, y);
                        }
                    }

                    canvas.Children.Clear();
                    canvas.Children.Add(tmpRectangle);

                    if (e.LeftButton == MouseButtonState.Released)
                    {
                        canvas.Children.Clear();



                        if (x - newx == 0)
                        {
                            width = Math.Abs(e.GetPosition(null).X - x);
                            if (y - newy == 0)
                            {
                                height = Math.Abs(e.GetPosition(null).Y - y);
                            }
                            else
                            {
                                height = Math.Abs(y - newy);
                            }
                        }
                        else
                        {
                            width = Math.Abs(x - newx);
                            if (y - newy == 0)
                            {
                                height = Math.Abs(e.GetPosition(null).Y - y);
                            }
                            else
                            {
                                height = Math.Abs(y - newy);
                            }
                        }

                        if (width > 13 && height > 13 && newx != 0 && newy != 0)
                        {
                            CutScreen(newx, newy, width, height, window, parentWindow);
                        }
                        x = 0;
                        y = 0;
                        isMouseDown = false;
                    }
                }
            };
        }

        /// <summary>
        /// İşaretlenen yeri resime çevirir
        /// </summary>
        /// <param name="x">Seçilen alanın x koordinatı</param>
        /// <param name="y">Seçilen alanın y koordinatı</param>
        /// <param name="width">Seçilen alanın genişliği</param>
        /// <param name="height">Seçilen alanın yüksekliği</param>
        private void CutScreen(double x, double y, double width, double height, Window window, Window parentWindow)
        {

            int ix = Convert.ToInt16(x);
            int iy = Convert.ToInt16(y);
            int iwidth = Math.Abs(Convert.ToInt16(width));
            int iheight = Math.Abs(Convert.ToInt16(height));

            Bitmap image = new Bitmap(iwidth - 13, iheight - 13, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics tmpGraphics = Graphics.FromImage(image);
            tmpGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            tmpGraphics.CopyFromScreen(ix, iy, 0, 0, new System.Drawing.Size(iwidth - 13, iheight - 13), CopyPixelOperation.SourceCopy);

            Directory.CreateDirectory(@"C:\Users\Public\Documents\Screen Cutter");
            image.Save(@"C:\Users\Public\Documents\Screen Cutter\" + DateTime.Now.Second+".jpg");
            

            window.Close();
            parentWindow.Visibility = System.Windows.Visibility.Visible;

        }

        #endregion
    }
}
