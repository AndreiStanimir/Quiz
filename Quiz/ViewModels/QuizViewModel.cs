﻿using CommunityToolkit.Mvvm.ComponentModel;

using Quiz.Models;
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

        private IEnumerator<Question> questionsEnumerator;

        private Quiz.Models.Quiz quiz;

        public QuizViewModel(QuizContext quizContext)
        {
            this.quizContext = quizContext;
            questions = quiz.Questions.ToArray();
            questionsEnumerator = questions.GetEnumerator();
            GetNextQuestion();
            CorrectAnswers = 0;
        }

        public void SetQuiz(int id)
        {
            quiz = quizContext.Quizzes.Find(id);
        }

        //[RelayCommand]

        public bool GetNextQuestion()
        {
            if (!questionsEnumerator.MoveNext())
            {
                QuizAttempt quizAttempt = new(quiz)
                {
                    NumberCorrectAnswers = correctAnswers,
                    DateTime = DateTime.Now,
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