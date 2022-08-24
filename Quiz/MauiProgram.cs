using CommunityToolkit.Maui;
using Quiz.Models;

namespace Quiz;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        var deviceId = Preferences.Get("my_deviceId", string.Empty);
        if (string.IsNullOrWhiteSpace(deviceId))
        {
            deviceId = System.Guid.NewGuid().ToString();
            Preferences.Set("my_deviceId", deviceId);
        }
        string dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "quizes.db");
        //builder.Services.AddDbContext<QuizContext>(options
        //    {
        //});
        //builder.Services.AddSqlite()
        //builder.Services.AddSingleton<QuizContext>(s => ActivatorUtilities.CreateInstance<QuizContext>(s, dbPath));
        builder.Services.AddDbContext<QuizContext>(ServiceLifetime.Singleton);
        return builder.Build();
    }
}