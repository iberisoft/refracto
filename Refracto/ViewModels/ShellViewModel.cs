using Caliburn.Micro;
using Refracto.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Refracto.ViewModels
{
    class ShellViewModel : Screen, IShell
    {
        readonly IWindowManager m_WindowManager;
        readonly IDialogManager m_DialogManager;
        readonly IStore m_Store;
        readonly DataViewModel.Factory m_DataFactory;
        readonly CreateTimelineViewModel.Factory m_CreateTimelineFactory;
        readonly ConfigViewModel.Factory m_ConfigFactory;

        public ShellViewModel(IWindowManager windowManager, IDialogManager dialogManager, IStore store, DataViewModel.Factory dataFactory, CreateTimelineViewModel.Factory createTimelineFactory,
            ConfigViewModel.Factory configFactory)
        {
            m_WindowManager = windowManager;
            m_DialogManager = dialogManager;
            m_Store = store;
            m_DataFactory = dataFactory;
            m_CreateTimelineFactory = createTimelineFactory;
            m_ConfigFactory = configFactory;
        }

        BindableCollection<DataViewModel> m_Items;

        public BindableCollection<DataViewModel> Items
        {
            get
            {
                if (m_Items == null)
                {
                    m_Items = new BindableCollection<DataViewModel>(m_Store.ReadAll().Select(timeline => m_DataFactory(timeline)));
                }
                return m_Items;
            }
        }

        DataViewModel m_SelectedItem;

        public DataViewModel SelectedItem
        {
            get => m_SelectedItem;
            set
            {
                if (value != null && value.Timeline.Data.Count == 0)
                {
                    m_Store.ReadData(value.Timeline);
                }

                if (Set(ref m_SelectedItem, value))
                {
                    NotifyOfPropertyChange(() => CanSaveItem);
                    NotifyOfPropertyChange(() => CanDeleteItem);
                }
            }
        }

        DataViewModel m_RunningItem;

        public DataViewModel RunningItem
        {
            get => m_RunningItem;
            set
            {
                if (Set(ref m_RunningItem, value))
                {
                    NotifyOfPropertyChange(() => CanCreateItem);
                    NotifyOfPropertyChange(() => CanStopRunning);
                    NotifyOfPropertyChange(() => CanSaveItem);
                    NotifyOfPropertyChange(() => CanDeleteItem);
                    NotifyOfPropertyChange(() => CanConfig);
                }
            }
        }

        public override void CanClose(Action<bool> callback)
        {
            foreach (var item in m_Items.Where(item => item.IsModified && item.Timeline.Data.Count == 0).ToArray())
            {
                m_Store.Delete(item.Id);
                m_Items.Remove(item);
            }

            var close = true;
            if (m_Items.Any(item => item.IsModified))
            {
                switch (m_DialogManager.ConfirmSave())
                {
                    case true:
                        foreach (var item in m_Items.Where(item => item.IsModified))
                        {
                            m_Store.Update(item.Timeline);
                        }
                        break;
                    case false:
                        foreach (var item in m_Items.Where(item => item.IsModified))
                        {
                            m_Store.Delete(item.Id);
                        }
                        break;
                    case null:
                        close = false;
                        break;
                }
            }
            callback(close);
        }

        public bool CanCreateItem => RunningItem == null;

        public void CreateItem()
        {
            var createTimeline = m_CreateTimelineFactory();
            if (m_WindowManager.ShowDialog(createTimeline) == true)
            {
                var item = m_DataFactory(new Timeline(createTimeline.TimelineName, DateTime.Now));
                item.IsModified = true;
                if (m_Store.Create(item.Timeline))
                {
                    Items.Add(item);
                    SelectedItem = item;
                    RunningItem = item;
                    Task.Run(() => StartRunning());
                }
                else
                {
                    m_DialogManager.WarnExist(item.Timeline);
                }
            }
        }

        private void StartRunning()
        {
            try
            {
                using (var device = IoC.Get<IDevice>())
                {
                    while (true)
                    {
                        var readout = device.Read();
                        if (readout == null)
                        {
                            continue;
                        }
                        var item = RunningItem;
                        if (item == null)
                        {
                            break;
                        }
                        item.Data.Add(readout);
                        item.IsModified = true;
                        NotifyOfPropertyChange(() => CanSaveItem);
                    }
                }
            }
            catch (Exception ex)
            {
                RunningItem = null;
                Execute.OnUIThread(() => m_DialogManager.Error(ex.InnerException ?? ex));
            }
        }

        public bool CanStopRunning => RunningItem != null;

        public void StopRunning()
        {
            RunningItem = null;
        }

        public bool CanSaveItem => SelectedItem != null && SelectedItem != RunningItem && SelectedItem.IsModified;

        public void SaveItem()
        {
            m_Store.Update(SelectedItem.Timeline);
            SelectedItem.IsModified = false;
            NotifyOfPropertyChange(() => CanSaveItem);
        }

        public bool CanDeleteItem => SelectedItem != null && SelectedItem != RunningItem;

        public void DeleteItem()
        {
            if (m_DialogManager.ConfirmDelete(SelectedItem.Timeline))
            {
                m_Store.Delete(SelectedItem.Id);
                Items.Remove(SelectedItem);
            }
        }

        public bool CanConfig => RunningItem == null;

        public void Config()
        {
            var config = m_ConfigFactory();
            config.StorePath = Properties.Settings.Default.FileStorePath;
            config.SerialPort = Properties.Settings.Default.SerialPort;
            config.XAxisLength = Properties.Settings.Default.XAxisLength;
            if (m_WindowManager.ShowDialog(config) == true)
            {
                Properties.Settings.Default.FileStorePath = config.StorePath.Trim();
                Properties.Settings.Default.SerialPort = config.SerialPort;
                Properties.Settings.Default.XAxisLength = config.XAxisLength;
                Properties.Settings.Default.Save();
                m_Items = null;
                NotifyOfPropertyChange(() => Items);
            }
        }
    }
}
