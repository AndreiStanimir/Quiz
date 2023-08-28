using CommunityToolkit.Mvvm.ComponentModel;
using Quiz.Models;
using Quiz.ViewModels;
using System.Drawing.Text;

namespace Quiz.Views;

public partial class QuizAttemptFinalPage : ContentPage
{
    QuizAttemptFinishViewModel ViewModel;

    public QuizAttemptFinalPage(QuizAttemptFinishViewModel quizAttempt = null)
    {
        QuizAttempt quizAttempt1 = QuizContextFactory.GetContext().QuizAttempts.OrderBy(x => x.DateTime).Last();
        quizAttempt ??= new(quizAttempt1);
        BindingContext = ViewModel = quizAttempt;
        InitializeComponent();

        if (quizAttempt.DidUserPass())
        {
            labelPassedExam.Text = "Ai trecut testul!";
            labelPassedExam.TextColor = Color.Parse("green");
        }
        else
        {
            labelPassedExam.Text = "Ai picat testul!";
            labelPassedExam.TextColor = Color.Parse("red");
        }
    }

    private void ReturnToSelectQuizButton_Clicked(object sender, EventArgs e)
    {
        Navigation.PopAsync();
        if (Navigation.NavigationStack.Last() is not SelectQuizPage)
            Navigation.PushAsync(new SelectQuizPage());
    }
}