using Caliburn.Micro;
using Refracto.Services;
using System.Collections.Generic;

namespace Refracto.ViewModels
{
    static class ViewModelFactory
    {
        public delegate DataViewModel DataViewModelFactory(Timeline timeline);

        public static DataViewModelFactory DataViewModel { get; } = IoC.Get<DataViewModelFactory>();

        public delegate ChartViewModel ChartViewModelFactory(List<Readout> data);

        public static ChartViewModelFactory ChartViewModel { get; } = IoC.Get<ChartViewModelFactory>();

        public delegate CreateTimelineViewModel CreateTimelineViewModelFactory();

        public static CreateTimelineViewModelFactory CreateTimelineViewModel { get; } = IoC.Get<CreateTimelineViewModelFactory>();

        public delegate ConfigViewModel ConfigViewModelFactory();

        public static ConfigViewModelFactory ConfigViewModel { get; } = IoC.Get<ConfigViewModelFactory>();
    }
}
