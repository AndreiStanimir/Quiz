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
        private QuizContext quizContext;

        [ObservableProperty]
        private Question currentQuestion;

        private ICollection<Question> questions;

        [ObservableProperty]
        private int correctAnswers;

        public QuizViewModel()
        {
            quizContext = QuizContextFactory.GetContext();
            questions = quizContext.Questions.ToList();
            CurrentQuestion = questions.First();
            CorrectAnswers = 0;
        }

        public QuizViewModel(int id)
        {
            quizContext = QuizContextFactory.GetContext();
            var quiz = quizContext.Quizzes.Find(id);
            questions = quiz.Questions.ToList();
            currentQuestion = questions.First();
            correctAnswers = 0;
        }

        [RelayCommand]
        public void GetNextQuestion()
        {
            var firstQuestion = questions.FirstOrDefault();
            if (firstQuestion == default)
            {
                QuizAttempt quizAttempt = new QuizAttempt
                {
                    NumberCorrectAnswers = correctAnswers,
                };
            }
            questions.Remove(firstQuestion);
            CurrentQuestion = questions.First();
            //Answers = currentQuestion.Answers;
        }

        internal bool DidUserAnswerCorrectly(IEnumerable<Answer> answers)
        {
            if (answers.SequenceEqual(this.currentQuestion.CorrectAnswers))
            {
                correctAnswers++;
                return true;
            }

            return false;
        }
    }
}