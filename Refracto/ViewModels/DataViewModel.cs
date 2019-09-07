using Caliburn.Micro;
using Refracto.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Refracto.ViewModels
{
    class DataViewModel : PropertyChangedBase
    {
        public delegate DataViewModel Factory(Timeline timeline);

        readonly ChartViewModel.Factory m_ChartFactory;

        public DataViewModel(ChartViewModel.Factory chartFactory, Timeline timeline)
        {
            m_ChartFactory = chartFactory;
            Timeline = timeline;
        }

        public Timeline Timeline { get; }

        ChartViewModel m_Chart;

        public ChartViewModel Chart
        {
            get
            {
                if (m_Chart == null)
                {
                    m_Chart = m_ChartFactory(Timeline.Data);
                }
                return m_Chart;
            }
        }

        public string Id
        {
            get => Timeline.Id;
        }

        public DateTime Timestamp => Timeline.Timestamp;

        BindableCollection<Readout> m_Data;

        public BindableCollection<Readout> Data
        {
            get
            {
                if (m_Data == null)
                {
                    m_Data = new BindableCollection<Readout>(Timeline.Data);
                    m_Data.CollectionChanged += Data_CollectionChanged;
                }
                return m_Data;
            }
        }

        Queue<Readout> m_ChartReadouts = new Queue<Readout>();

        private void Data_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var readout = (Readout)e.NewItems[0];
                Timeline.Data.Add(readout);
                if (Timeline.Data.Count == 1)
                {
                    NotifyOfPropertyChange(() => Timestamp);
                }

                m_ChartReadouts.Enqueue(readout);
                if (Timeline.Data.Count % Properties.Settings.Default.ChartUpdateRate == 0)
                {
                    foreach (var readout2 in m_ChartReadouts)
                    {
                        Chart.AddReadout(readout2);
                    }
                    m_ChartReadouts.Clear();
                }
            }
        }

        public bool m_IsModified;

        public bool IsModified
        {
            get => m_IsModified;
            set
            {
                Set(ref m_IsModified, value);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is DataViewModel op2 && op2.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
