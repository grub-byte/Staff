using System.Windows;
using Autofac;
using ClientStaff.Service;
using ClientStaff.ViewModels;
using NLog;

namespace ClientStaff
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static string NLOG_LAYOUT = "[${threadid}]|${longdate}|[${level}]|${callsite:className=True:fileName=False:includeSourcePath=False:methodName=True}|${message}${onexception:inner=|${exception}${when:when=(level > LogLevel.Warn):inner=|[!] ${exception:format=ToString:innerFormat=Message:maxInnerExceptionLevel=5} }}";
        internal static IContainer? Container { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Container = BuildUpContainer();
            LogManager.Setup().LoadConfiguration(b =>
            {
                b.ForLogger().FilterMinLevel(LogLevel.Debug).WriteToConsole(layout: NLOG_LAYOUT);
            });
        }

        private static IContainer BuildUpContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<EmployeeDataService>().AsImplementedInterfaces().SingleInstance();

            return builder.Build();
        }

        internal static MainWindowViewModel? MainVM => Container?.Resolve<MainWindowViewModel>();
    }
}
