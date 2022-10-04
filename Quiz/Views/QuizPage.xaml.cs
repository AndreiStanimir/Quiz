
using Quiz.Models;
using Microsoft.EntityFrameworkCore;
using Quiz.CustomControls;
using Quiz.ViewModels;
using System.Data.Entity;
using CommunityToolkit.Mvvm.Input;

namespace Quiz.Views;

public partial class QuizPage : ContentPage
{
    ButtonAnswer[] buttonsAnswers;
    QuizViewModel ViewModel;
    //QuizViewModel viewModel
    //{
    //    get =>
    //        (QuizViewModel)this.BindingContext;
    //}
    int wrongAnswers;
    public QuizPage()
    {
        InitializeComponent();
        //BindingContext = new QuizViewModel();
        //ViewModel = new QuizViewModel();
        //BindingContext = ViewModel;
        //listViewAnswers.SetBinding(CollectionView.ItemsSourceProperty, "Answers");

        //buttonsAnswers = listViewAnswers.items
        //foreach (var button in buttonsAnswers)
        //{
        //    button.SetBinding(Button.TextProperty, new Binding());
        //}
        //labelCorrectAnswers.SetBinding(ContentProperty, new Binding("CorrectAnswers"));
        //ans1.SetBinding(ContentProperty, new Binding())
        BindingContext = ViewModel = new QuizViewModel();

        //ViewModel.GetNextQuestion();
    }

    public QuizPage(int id)
    {
        InitializeComponent();
        BindingContext = ViewModel = new QuizViewModel(id);
    }

    void GetNextQuestion()
    {
        var answers = listViewAnswers.SelectedItems.Cast<Answer>();
        //ViewModel.DidUserAnswerCorrectly(answers);
        bool gameEnded = !ViewModel.GetNextQuestion();
        if (gameEnded)
        {
            Navigation.PopAsync();
            Navigation.PushAsync(new QuizAttemptFinalPage(new QuizAttemptFinishViewModel()));
        };
        var question = ViewModel.CurrentQuestion;
        //LabelQuestion.Text = question.Text;
    }
    void buttonAnswerClick(object sender, EventArgs e)
    {
        var answerClicked = (ButtonAnswer)sender;
        //listViewAnswers.SelectedItems
        //    .AsEnumerable<Answer>()
        //    .All(answer => answer.Correct == true);
        //listViewAnswers.ItemsSource
        //    .AsQueryable()
        //    .ToListAsync()
        //    .Result
        //    .All()

        //currentQuestion.Answers.First(a => ((Button)sender).Text == a.Text);
    }

    private void buttonNextQuestion_Clicked(object sender, EventArgs e)
    {
        var  answers = listViewAnswers.SelectedItems.Select<object,Answer>(s=>(Answer)s);
        ViewModel.DidUserAnswerCorrectly(answers);
        GetNextQuestion();
        listViewAnswers.SelectedItems.Clear();
    }

    private void listViewAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            buttonNextQuestion.BackgroundColor = Color.Parse("green");
            buttonNextQuestion.Opacity = 1;
            buttonNextQuestion.IsEnabled = true;
        }
        else
        {
            buttonNextQuestion.BackgroundColor = Color.Parse("red");
            buttonNextQuestion.Opacity = 0.5;
            buttonNextQuestion.IsEnabled = false;

        }
    }
}