using MedicalQuiz.DatabaseModels;
using MedicalQuiz.Models;
using Microsoft.EntityFrameworkCore;
using Quiz.CustomControls;

namespace Quiz.Pages;

public partial class QuizPage : ContentPage
{
    QuizContext quizContext;
    ButtonAnswer[] buttonsAnswers;
    Question currentQuestion;
    ICollection<Question> questions;
    private int correctAnswers;

    public int CorrectAnswers
    {
        get { return correctAnswers; }
        set
        {
            if (correctAnswers != value)
            {
                correctAnswers = value;
                OnPropertyChanged("CorrectAnswers");
            }
        }
    }

    int wrongAnswers;
    public QuizPage()
    {
        InitializeComponent();
        quizContext = new QuizContext();
        buttonsAnswers = new[] { ans1, ans2, ans3, ans4 };
        questions = quizContext.Questions.ToList();
        CorrectAnswers = 0;

        labelCorrectAnswers.SetBinding(ContentProperty, new Binding("CorrectAnswers"));
        GetNextQuestion();

    }
    void GetNextQuestion()
    {
        var question = questions.First();
        questions.Remove(question);
        var answers = question.Answers.ToArray();
        LabelQuestion.Text = question.Text;
        for (int i = 0; i < answers.Length; i++)
        {
            buttonsAnswers[i].Text = question.Answers.ElementAt(i).Text;
            buttonsAnswers[i].IsSelected = false;
            buttonsAnswers[i].Correct = question.Answers.ElementAt(i).Correct;
            //buttonsAnswers[i].tag)
        }
        for (int i = answers.Length; i < buttonsAnswers.Length; i++)
        {
            buttonsAnswers[i].IsVisible = false;
        }
        currentQuestion = question;
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
            CorrectAnswers++;
        GetNextQuestion();
    }
}