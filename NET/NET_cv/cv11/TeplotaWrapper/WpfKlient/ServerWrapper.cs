using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfKlient
{
    public class ServerWrapper
    {

        [DllImport("Server.dll")]
        public static extern void Init();

        [DllImport("Server.dll")]
        public static extern void Stop();

        [DllImport("Server.dll")]
        public static extern void Start();

        [DllImport("Server.dll")]
        public static extern bool IsRunning();

        [DllImport("Server.dll")]
        public static extern void RegisterCallBack(CallBackFunc call_back);

        public  delegate void CallBackFunc(int teplota);
    }
}
