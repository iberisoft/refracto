using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using LiveCharts;
using Refracto.Services;

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

        public int m_TimestampMin;

        public int TimestampMin
        {
            get => m_TimestampMin;
            set
            {
                if (Set(ref m_TimestampMin, value))
                {
                    NotifyOfPropertyChange();
                }
            }
        }

        public int m_TimestampMax = Properties.Settings.Default.XAxisLength;

        public int TimestampMax
        {
            get => m_TimestampMax;
            set
            {
                if (Set(ref m_TimestampMax, value))
                {
                    NotifyOfPropertyChange();
                }
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

            if (Data.Count > Properties.Settings.Default.XAxisLength)
            {
                TimestampMax = Data.Count - 1;
                TimestampMin = TimestampMax - Properties.Settings.Default.XAxisLength;
            }
        }
    }
}
