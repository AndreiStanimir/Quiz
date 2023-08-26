using CommunityToolkit.Maui;
using Quiz.Models;
using Microsoft.Maui.LifecycleEvents;
namespace Quiz;

public static class MauiProgram

{
    public static MauiApp Main()
    {
        // Document builder pattern 
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                        events.AddWindows(windows => windows
                           .OnActivated((window, args) => LogEvent(nameof(WindowsLifecycle.OnActivated)))
                           .OnClosed((window, args) => OnAppExit())
                           .OnLaunched((window, args) => LogEvent(nameof(WindowsLifecycle.OnLaunched)))
                           .OnLaunching((window, args) => LogEvent(nameof(WindowsLifecycle.OnLaunching)))
                           .OnVisibilityChanged((window, args) => LogEvent(nameof(WindowsLifecycle.OnVisibilityChanged)))
                           .OnPlatformMessage((window, args) =>
                           {
                               if (args.MessageId == Convert.ToUInt32("031A", 16))
                               {
                                   // System theme has changed
                               }
                           }));
                              static bool LogEvent(string eventName, string type = null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lifecycle event: {eventName}{(type == null ? string.Empty : $" ({type})")}");
                        return true;
                    }
#endif
            });
        var deviceId = Preferences.Get("my_deviceId", string.Empty);
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            deviceId = System.Guid.NewGuid().ToString();
            Preferences.Set("my_deviceId", deviceId);
        }
        string dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "quizzes.db");
        //builder.Services.AddDbContext<QuizContext>(options
        //    {
        //});
        //builder.Services.AddSqlite()
        //builder.Services.AddSingleton<QuizContext>(s => ActivatorUtilities.CreateInstance<QuizContext>(s, dbPath));
        builder.Services.AddDbContext<QuizContext>(ServiceLifetime.Singleton);
        return builder.Build();
    }
    public static void OnAppExit()
    {
        var context = QuizContextFactory.GetContext();
        context.Dispose();
    } // Generate on app exit
}   