using Caliburn.Micro;

namespace Refracto
{
    public class ConfigViewModel : Screen
    {
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
                if (Set(ref m_StorePath, value))
                {
                    NotifyOfPropertyChange();
                }
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

        public string[] AllSerialPorts => System.IO.Ports.SerialPort.GetPortNames();

        string m_SerialPort = "";

        public string SerialPort
        {
            get => m_SerialPort;
            set
            {
                if (Set(ref m_SerialPort, value))
                {
                    NotifyOfPropertyChange();
                }
            }
        }

        public void Accept()
        {
            TryClose(true);
        }
    }
}
