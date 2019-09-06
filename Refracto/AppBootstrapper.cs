using Autofac;
using Caliburn.Micro.Autofac;
using Refracto.Services;
using Refracto.ViewModels;
using System.Windows;

namespace Refracto
{
    public class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();

            EnforceNamespaceConvention = false;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AssemblyModule>();
            builder.RegisterModule<Data.AssemblyModule>();
            builder.RegisterModule<Acquisition.AssemblyModule>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<IShell>();
        }
    }
}
