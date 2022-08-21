using CommunityToolkit.Maui;
using Quiz.Models;

namespace Quiz;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var quizContext = new QuizContext();
        
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        string dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "quizes.db");
        //builder.Services.AddDbContext<QuizContext>(options
        //    {
            
        //});
        builder.Services.AddSingleton<QuizContext>(s => ActivatorUtilities.CreateInstance<QuizContext>(s, dbPath));
        return builder.Build();
    }
}