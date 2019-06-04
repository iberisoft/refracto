using System;
using System.IO.Ports;
using System.Threading.Tasks;
using Refracto.Data;

namespace Refracto.DataInput
{
    class RsiDevice : IDevice
    {
        SerialPort m_Port;
        const int m_Timeout = 10000;

        public RsiDevice()
        {
            m_Port = new SerialPort(Properties.Settings.Default.SerialPort);
            m_Port.BaudRate = 4800;
            m_Port.Open();
        }

        public void Dispose()
        {
            m_Port.Dispose();
        }

        public Readout Read()
        {
            m_Port.Write("@1302\r");
            if (WaitResponse())
            {
                var response = m_Port.ReadExisting();
                return ParseResponse(response);
            }
            return null;
        }

        private bool WaitResponse()
        {
            for (var tickCount = Environment.TickCount; Environment.TickCount - tickCount < m_Timeout;)
            {
                if (m_Port.BytesToRead > 0)
                    return true;
                Task.Delay(10).Wait();
            }
            return false;
        }

        private static Readout ParseResponse(string response)
        {
            if (response[0] == '@')
            {
                response = response.Substring(1);
                var tokens = response.Split(new[] { ' ', (char)0 }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 1)
                {
                    if (float.TryParse(tokens[0], out float temperature) && float.TryParse(tokens[1], out float brix))
                    {
                        return new Readout(DateTime.Now) { Brix = brix, Temperature = temperature };
                    }
                }
            }
            return null;
        }
    }
}
