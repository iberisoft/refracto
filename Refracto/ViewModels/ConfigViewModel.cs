using Caliburn.Micro;
using Refracto.Services;
using System.Collections.Generic;
using System.Linq;

namespace Refracto.ViewModels
{
    class ConfigViewModel : Screen
    {
        public delegate ConfigViewModel Factory();

        readonly IDialogManager m_DialogManager;

        public ConfigViewModel(IDialogManager dialogManager)
        {
            m_DialogManager = dialogManager;
        }

        string m_StorePath = "";

        public string StorePath
        {
            get => m_StorePath;
            set
            {
                Set(ref m_StorePath, value);
            }
        }

        public void BrowseStore()
        {
            var path = m_DialogManager.BrowseFolder();
            if (path != null)
            {
                StorePath = path;
            }
        }

        public string[] AllSerialPorts
        {
            get
            {
                var ports = new List<string>(System.IO.Ports.SerialPort.GetPortNames().Distinct());
                ports.Insert(0, "");
                return ports.ToArray();
            }
        }

        string m_SerialPort = "";

        public string SerialPort
        {
            get => m_SerialPort;
            set
            {
                Set(ref m_SerialPort, value);
            }
        }

        int m_XAxisLength;

        public int XAxisLength
        {
            get => m_XAxisLength;
            set
            {
                Set(ref m_XAxisLength, value);
            }
        }

        public void Accept()
        {
            TryClose(true);
        }
    }
}
