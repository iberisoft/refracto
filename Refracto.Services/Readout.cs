using System;

namespace Refracto.Services
{
    public class Readout
    {
        public Readout(DateTime timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTime Timestamp { get; }

        public float Brix { get; set; }

        public float Temperature { get; set; }
    }
}
