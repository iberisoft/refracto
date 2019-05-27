using System;
using System.Collections.Specialized;
using Caliburn.Micro;
using Refracto.Data;

namespace Refracto
{
    public class DataViewModel : PropertyChangedBase
    {
        public DataViewModel(string id)
        {
            Timeline = new Timeline(id);
            Chart = new ChartViewModel(Timeline.Data);
        }

        public DataViewModel(Timeline timeline)
        {
            Timeline = timeline;
            Chart = new ChartViewModel(Timeline.Data);
        }

        public Timeline Timeline { get; }

        public ChartViewModel Chart { get; }

        public string Id
        {
            get => Timeline.Id;
        }

        public DateTime? Timestamp => Timeline.Timestamp;

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
                Chart.AddReadout(readout);
            }
        }

        public bool m_IsModified;

        public bool IsModified
        {
            get => m_IsModified;
            set
            {
                if (Set(ref m_IsModified, value))
                {
                    NotifyOfPropertyChange();
                }
            }
        }

        public override bool Equals(object obj)
        {
            var op2 = obj as DataViewModel;
            return op2 != null && op2.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
