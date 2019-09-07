using Refracto.Services;
using System;
using System.IO.Ports;
using System.Threading;

namespace Refracto.Acquisition
{
    class RsiDevice : IDevice
    {
        readonly SerialPort m_Port;
        const int m_Timeout = 10000;

        public RsiDevice(ISettings settings)
        {
            m_Port = new SerialPort(settings.SerialPort);
            m_Port.BaudRate = 4800;
            m_Port.Open();
            m_Port.Write("@2301\r");
        }

        public void Dispose()
        {
            m_Port.Dispose();
        }

        public Readout Read()
        {
            if (WaitResponse())
            {
                var response = m_Port.ReadExisting();
                if (response.StartsWith("OK"))
                {
                    return null;
                }
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
                Thread.Sleep(10);
            }
            return false;
        }

        private static Readout ParseResponse(string response)
        {
            if (response[0] == '@')
            {
                response = response.Substring(1);
                var tokens = response.Split(new[] { ' ', (char)0 }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 2)
                {
                    if (float.TryParse(tokens[1], out float temperature) && float.TryParse(tokens[2], out float brix))
                    {
                        return new Readout(DateTime.Now) { Brix = brix, Temperature = temperature };
                    }
                }
            }
            return null;
        }
    }
}
