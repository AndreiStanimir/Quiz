using CommunityToolkit.Maui;
using MedicalQuiz.Models;

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

        return builder.Build();
    }
}