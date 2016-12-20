using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
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

namespace WpfKlient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        void TeplotaCallBack(int teplota)
        {
            Teplota = teplota;
        }

        private ServerWrapper.CallBackFunc cb;

        public MainWindow()
        {
            InitializeComponent();
            ServerWrapper.Init();
            cb = new ServerWrapper.CallBackFunc(TeplotaCallBack);
            ServerWrapper.RegisterCallBack(cb);
        }

        RelayCommand startCmd;

        public RelayCommand StartCmd
        {
            get
            {
                if (startCmd == null)
                {
                    startCmd = new RelayCommand(
                        param => new Thread(() => ServerWrapper.Start()).Start(),
                        param => !ServerWrapper.IsRunning());
                }
                return startCmd;
            }
        }

        RelayCommand stopCmd;

        public RelayCommand StopCmd
        {
            get
            {
                if (stopCmd == null)
                {
                    stopCmd = new RelayCommand(
                        param => ServerWrapper.Stop(),
                        param => ServerWrapper.IsRunning());
                }
                return stopCmd;
            }
        }

        int teplota = 0;
        public int Teplota
        {
            get { return teplota; }
            set { 
                teplota = value;
               
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Teplota"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
