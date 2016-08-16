using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _20160815.ScreenCuter
{
    public class MainViewModel:MainModel
    {
        #region Constructor

        public MainViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        
        #endregion

        #region Members

        public MainWindow MainWindow { get; set; }
        private ICommand buttonsClick;

        #endregion

        #region Properties

        public ICommand ButtonsClick
        {
            get
            {
                buttonsClick = new RelayCommand<string>(ButtonsCommand);
                return buttonsClick;
            }
        }

        

        private void ScreenShotViewModel_ScreenShotEvent(object sender, EventArgs e)
        {
            
            //byte[] img = (byte[])sender;
            //MainWindow.richTextBox.Document.Blocks.Add(ScreenToChat(img, "Levent"));
            //MainWindow.richTextBox.ScrollToEnd();
        }



        #endregion

        #region Methods

        private void ButtonsCommand(string obj)
        {
            switch (obj)
            {
                case "Cut":
                    MainWindow.Visibility = Visibility.Collapsed;
                    ScreenShot win = new ScreenShot();
                    win.WindowState = WindowState.Maximized;
                    win.ScreenShotViewModel.ScreenShotEvent += ScreenShotViewModel_ScreenShotEvent;

                    win.ShowDialog();
                    break;

                case "Show":

                    string windir = Environment.GetEnvironmentVariable("WINDIR");
                    System.Diagnostics.Process prc = new System.Diagnostics.Process();
                    prc.StartInfo.FileName = windir + @"\explorer.exe";
                    prc.StartInfo.Arguments = @"C:\Users\Public\Documents\Screen Cutter\";
                    prc.Start();

                    break;

                default:
                    break;
            }
        }


        #endregion

    }
}
