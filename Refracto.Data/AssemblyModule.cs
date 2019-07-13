using Autofac;
using Refracto.Services;

namespace Refracto.Data
{
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileStore>().As<IStore>().SingleInstance();
        }
    }
}
