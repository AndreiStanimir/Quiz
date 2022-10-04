using Quiz.Models;
using Quiz.ViewModels;

namespace Quiz.Views;

public partial class QuizAttemptFinalPage : ContentPage
{
	public QuizAttemptFinalPage(QuizAttemptFinishViewModel quizAttempt)
	{
		InitializeComponent();

		if(quizAttempt.DidUserPass())
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
}