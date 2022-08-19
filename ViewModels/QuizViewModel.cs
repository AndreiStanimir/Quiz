using MedicalQuiz.DatabaseModels;
using MedicalQuiz.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ViewModels
{
    internal class QuizViewModel : INotifyPropertyChanged
    {
        QuizContext quizContext;
        public Question CurrentQuestion;
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
                    OnPropertyChange("CorrectAnswers");
                }
            }
        }
        public QuizViewModel()
        {
            quizContext = new QuizContext();
            questions = quizContext.Questions.ToList();
            CurrentQuestion = questions.First();
            CorrectAnswers = 0;
        }

       public void GetNextQuestion()
        {
            CorrectAnswers++;
            questions.Remove(questions.First());
            CurrentQuestion=questions.First();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
