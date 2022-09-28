using Quiz.DatabaseModels;
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
        BindingContext = new QuizViewModel();
        ViewModel.GetNextQuestion();
    }

    public QuizPage(int id)
    {
        InitializeComponent();
        BindingContext = new QuizViewModel(id);
    }

    void GetNextQuestion()
    {
        var answers = listViewAnswers.SelectedItems.Cast<Answer>() ;
        ViewModel.DidUserAnswerCorrectly(answers);
        ViewModel.GetNextQuestion();
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
        //listViewAnswers.SelectedItems<La
        //    .ItemsSource
        //    .AsQueryable()
        //    .ToListAsync()
        //    .
        //    if (correctAnswer)
        //    ViewModel.CorrectAnswers++;
        GetNextQuestion();
    }

    private void listViewAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            buttonNextQuestion.BackgroundColor = Color.Parse("green");
            buttonNextQuestion.Opacity = 1;
        }
        else
        {
            buttonNextQuestion.BackgroundColor = Color.Parse("red");
            buttonNextQuestion.Opacity = 0.5;
        }
    }
}