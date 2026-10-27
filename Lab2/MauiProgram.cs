using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Lab2.Services;

namespace Lab2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    events.AddWindows(windows => windows.OnWindowCreated(window =>
                    {
                        window.AppWindow.Closing += async (sender, args) =>
                        {
                            if (ExitService.IsConfirmed)
                            {
                                return;
                            }

                            args.Cancel = true;
                            await ExitService.ConfirmExitAsync();
                        };
                    }));
#endif
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}