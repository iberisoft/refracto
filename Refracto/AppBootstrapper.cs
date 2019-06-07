using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Refracto.Data;
using Refracto.DataInput;

namespace Refracto
{
    public class AppBootstrapper : BootstrapperBase
    {
        SimpleContainer container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IDialogManager, DialogManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.PerRequest<IShell, ShellViewModel>();
            container.Singleton<IStore, FileStore>();
            container.PerRequest<IDevice, DeviceEmulator>("emulator");
            container.PerRequest<IDevice, RsiDevice>("");
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
