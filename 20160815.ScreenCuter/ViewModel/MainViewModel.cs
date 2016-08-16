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

        private ICommand buttonsCommand;
        public MainWindow MainWindow { get; set; }

        #endregion
        #region Properties

        public ICommand ButtonsCommand
        {
            get
            {
                buttonsCommand = new RelayCommand<string>(ButtonCommand);
                return buttonsCommand;
            }
        }

        #endregion

        #region Methods

        private void ButtonCommand(string obj)
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
                    break;

                case "Exit":
                    break;

                default:break;
            }
        }

        private void ScreenShotViewModel_ScreenShotEvent(object sender, EventArgs e)
        {
            
        }

        #endregion

    }
}
