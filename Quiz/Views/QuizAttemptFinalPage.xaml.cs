namespace Quiz.Views;

public partial class QuizAttemptFinalPage : ContentPage
{
	public QuizAttemptFinalPage()
	{
		InitializeComponent();

		if(ViewModel.DidUserPass())
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