using Quiz.DatabaseModels;
using Quiz.Models;
using Microsoft.EntityFrameworkCore;
using Quiz.CustomControls;
using Quiz.ViewModels;
using System.Data.Entity;
using CommunityToolkit.Mvvm.Input;

namespace Quiz.Pages;

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
        GetNextQuestion();

    }
    void GetNextQuestion()
    {
        ViewModel.GetNextQuestion();
        var question = ViewModel.CurrentQuestion;
        var answers = question.Answers.ToArray();
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
}