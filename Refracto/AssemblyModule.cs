using Autofac;
using Refracto.Services;

namespace Refracto
{
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShellViewModel>().As<IShell>();
            builder.RegisterType<DialogManager>().As<IDialogManager>().InstancePerLifetimeScope();
            builder.RegisterInstance(Properties.Settings.Default).As<ISettings>();
        }
    }
}
