using Refracto.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Refracto.Data
{
    static class ReadoutSerializer
    {
        public static IEnumerable<string> Serialize(IEnumerable<Readout> readouts)
        {
            return readouts.Select(readout =>
            {
                var tokens = new string[3];
                tokens[0] = readout.Timestamp.ToString("yyyyMMddHHmmss");
                tokens[1] = readout.Brix.ToString();
                tokens[2] = readout.Temperature.ToString();
                return string.Join(",", tokens);
            });
        }

        public static IEnumerable<Readout> Deserialize(IEnumerable<string> lines)
        {
            return lines.Select(line =>
            {
                var tokens = line.Split(',');
                var readout = new Readout(DateTime.ParseExact(tokens[0], "yyyyMMddHHmmss", null));
                readout.Brix = float.Parse(tokens[1]);
                readout.Temperature = float.Parse(tokens[2]);
                return readout;
            });
        }
    }
}
