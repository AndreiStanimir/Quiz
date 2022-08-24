using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quiz.DatabaseModels;
using Quiz.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Quiz.ViewModels
{
    public partial class QuizViewModel : ObservableObject
    {
        QuizContext quizContext;
        [ObservableProperty]
        Question currentQuestion;
        ICollection<Question> questions;
        [ObservableProperty]
        private int correctAnswers;


        public QuizViewModel()
        {
            quizContext = new QuizContext();
            questions = quizContext.Questions.ToList();
            CurrentQuestion = questions.First();
            CorrectAnswers = 0;
        }

        public QuizViewModel(int id)
        {
            quizContext = new QuizContext();
            var quiz = quizContext.Quizzes.Find(id);
            questions = quiz.Questions.ToList();
            currentQuestion = questions.First();
            correctAnswers = 0;
        }

        [RelayCommand]
        public void GetNextQuestion()
        {
            questions.Remove(questions.First());
            CurrentQuestion = questions.First();
            //Answers = currentQuestion.Answers;
        }


        internal bool DidUserAnswerCorrectly(Answer[] answers)
        {
            if (answers.Count() == currentQuestion.CorrectAnswers.Count() &&
                answers.All(a => a.Correct))
            {
                correctAnswers++;
                return true;
            }

            return false;
        }
    }
}
