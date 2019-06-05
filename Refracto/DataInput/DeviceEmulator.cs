using System;
using System.Threading.Tasks;
using Refracto.Data;

namespace Refracto.DataInput
{
    class DeviceEmulator : IDevice
    {
        int m_Index;

        public void Dispose()
        {
        }

        public Readout Read()
        {
            Task.Delay(2000).Wait();

            var readout = new Readout(DateTime.Now);
            readout.Brix = (float)Math.Sin(m_Index * Math.PI / 18) + 3;
            readout.Temperature = (float)Math.Cos(m_Index * Math.PI / 18) + 25;
            ++m_Index;
            return readout;
        }
    }
}
