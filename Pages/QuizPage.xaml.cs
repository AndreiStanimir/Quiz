using Quiz.DatabaseModels;
using Quiz.Models;
using Microsoft.EntityFrameworkCore;
using Quiz.CustomControls;
using Quiz.ViewModels;

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
        ViewModel = new QuizViewModel();
        BindingContext = ViewModel;
        buttonsAnswers = new[] { ans1, ans2, ans3, ans4 };

        foreach (var button in buttonsAnswers)
        {
            button.SetBinding(Button.TextProperty,new Binding())
        }
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
        for (int i = 0; i < answers.Length; i++)
        {
            //buttonsAnswers[i].SetBinding(ButtonAnswer.)
            buttonsAnswers[i].BindingContext = question.Answers.ElementAt(i).Text;
            buttonsAnswers[i].IsSelected = false;
            buttonsAnswers[i].Correct = question.Answers.ElementAt(i).Correct;
            //buttonsAnswers[i].tag)
        }
        for (int i = answers.Length; i < buttonsAnswers.Length; i++)
        {
            buttonsAnswers[i].IsVisible = false;
        }
    }

    void buttonAnswerClick(object sender, EventArgs e)
    {
        var answerClicked = (ButtonAnswer)sender;
        //currentQuestion.Answers.First(a => ((Button)sender).Text == a.Text);
    }

    private void buttonNextQuestion_Clicked(object sender, EventArgs e)
    {
        var correctAnswer = buttonsAnswers.All(b => b.IsSelected == b.Correct);
        if (correctAnswer)
            ViewModel.CorrectAnswers++;
        GetNextQuestion();
    }
}