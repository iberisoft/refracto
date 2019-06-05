using System;
using System.Collections.Generic;
using System.Linq;

namespace Refracto.Data
{
    public class Timeline
    {
        public Timeline(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public DateTime Timestamp => Data.FirstOrDefault()?.Timestamp ?? DateTime.Now;

        public List<Readout> Data { get; } = new List<Readout>();
    }
}
