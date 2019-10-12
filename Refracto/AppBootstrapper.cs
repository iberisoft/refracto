using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Autofac;
using Refracto.Services;
using Refracto.ViewModels;
using System.Linq;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace Refracto
{
    class AppBootstrapper : AutofacBootstrapper<ShellViewModel>
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void ConfigureBootstrapper()
        {
            base.ConfigureBootstrapper();

            EnforceNamespaceConvention = false;

            var valueConvention = ConventionManager.AddElementConvention<IntegerUpDown>(IntegerUpDown.ValueProperty, "Value", "ValueChanged");
            var baseBindProperties = ViewModelBinder.BindProperties;
            ViewModelBinder.BindProperties = (elements, viewModelType) =>
            {
                foreach (var element in elements.OfType<IntegerUpDown>())
                {
                    var propertyName = element.Name;
                    var property = viewModelType.GetPropertyCaseInsensitive(propertyName);
                    if (property != null)
                    {
                        ConventionManager.SetBindingWithoutBindingOverwrite(viewModelType, propertyName, property, element, valueConvention, valueConvention.GetBindableProperty(element));
                    }
                }

                return baseBindProperties(elements, viewModelType);
            };
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
