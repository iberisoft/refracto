using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using LiveCharts;
using Refracto.Data;

namespace Refracto
{
    public class ChartViewModel : PropertyChangedBase
    {
        public ChartViewModel(List<Readout> data)
        {
            Data = data;
        }

        public List<Readout> Data { get; }

        public List<string> TimestampLabels
        {
            get
            {
                return new List<string>(Data.Select(readout => readout.Timestamp.ToString("T")));
            }
        }

        ChartValues<float> m_BrixValues;

        public ChartValues<float> BrixValues
        {
            get
            {
                if (m_BrixValues == null)
                {
                    m_BrixValues = new ChartValues<float>(Data.Select(readout => readout.Brix));
                }
                return m_BrixValues;
            }
        }

        ChartValues<float> m_TemperatureValues;

        public ChartValues<float> TemperatureValues
        {
            get
            {
                if (m_TemperatureValues == null)
                {
                    m_TemperatureValues = new ChartValues<float>(Data.Select(readout => readout.Temperature));
                }
                return m_TemperatureValues;
            }
        }

        public void AddReadout(Readout readout)
        {
            NotifyOfPropertyChange(() => TimestampLabels);
            BrixValues.Add(readout.Brix);
            TemperatureValues.Add(readout.Temperature);
        }
    }
}
