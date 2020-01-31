using Autofac;
using Refracto.Services;

namespace Refracto.Acquisition
{
    public class AssemblyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<IDevice>(context =>
            {
                var settings = context.Resolve<ISettings>();
                if (settings.SerialPort != "")
                {
                    return new RsiDevice(settings);
                }
                else
                {
                    return new DeviceEmulator();
                }
            }).As<IDevice>().ExternallyOwned();
        }
    }
}
