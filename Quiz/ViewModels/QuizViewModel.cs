using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Quiz.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

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

        IEnumerator<Question> questionsEnumerator;

        public QuizViewModel()
        {
            quizContext = QuizContextFactory.GetContext();
            questions = quizContext.Quizzes.First().Questions.ToArray();
            questionsEnumerator = questions.GetEnumerator();
            GetNextQuestion();
            CorrectAnswers = 0;
        }

        public QuizViewModel(int id)
        {
            quizContext = QuizContextFactory.GetContext();
            var quiz = quizContext.Quizzes.Find(id);
            questions = quiz.Questions.ToList();
            questionsEnumerator = questions.GetEnumerator();
            GetNextQuestion();
            correctAnswers = 0;
        }

        //[RelayCommand]

        public bool GetNextQuestion()
        {
            if (!questionsEnumerator.MoveNext())
            {
                QuizAttempt quizAttempt = new QuizAttempt
                {
                    NumberCorrectAnswers = correctAnswers,
                };
                quizContext.QuizAttempts.Add(quizAttempt);
                quizContext.SaveChanges();
                return false;
            }
            //Debug.Assert(questions.Remove (firstQuestion)==true);
            CurrentQuestion = questionsEnumerator.Current;
            return true;
            //Answers = currentQuestion.Answers;
        }

        internal bool DidUserAnswerCorrectly(IEnumerable<Answer> answers)
        {
            Debug.Assert(answers != null);
            Debug.Assert(this.currentQuestion?.CorrectAnswers != null);
            //if (answers.SequenceEqual(this.currentQuestion.CorrectAnswers))
            if (answers.Count() == this.currentQuestion.CorrectAnswers.Count())
                if (answers.All(a => a.Correct))
                //if (this.currentQuestion.CorrectAnswers.All(a => answers.FirstOrDefault(answerFromUser => a.AnswerId == answerFromUser.AnswerId) != default))
                {
                    CorrectAnswers++;
                    return true;
                }

            return false;
        }
    }
}