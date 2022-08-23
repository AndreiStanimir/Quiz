namespace Quiz.Views;

public partial class SelectQuizPage : ContentPage
{
	public SelectQuizPage()
	{
		InitializeComponent();
	}

	private void collectionViewQuizzes_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		var quiz=e.CurrentSelection.FirstOrDefault() as Quiz.Models.Quiz;
		Navigation.PushAsync(new QuizPage(quiz.Id));
	}
}