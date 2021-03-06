﻿using Caliburn.Micro;
using LiveCharts;
using Refracto.Services;
using System.Collections.Generic;
using System.Linq;

namespace Refracto.ViewModels
{
    class ChartViewModel : PropertyChangedBase
    {
        public ChartViewModel(List<Readout> data)
        {
            Data = data;
        }

        public List<Readout> Data { get; }

        public List<string> TimestampLabels => new List<string>(Data.Select(readout => readout.Timestamp.ToString("T")));

        int m_TimestampMin;

        public int TimestampMin
        {
            get => m_TimestampMin;
            set
            {
                Set(ref m_TimestampMin, value);
            }
        }

        int m_TimestampMax = Properties.Settings.Default.XAxisLength;

        public int TimestampMax
        {
            get => m_TimestampMax;
            set
            {
                Set(ref m_TimestampMax, value);
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
