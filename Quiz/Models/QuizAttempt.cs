using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public partial class QuizAttempt : ObservableObject, IComparable<QuizAttempt>
    {
        public int CompareTo(QuizAttempt other)
        {
            return DateTime.CompareTo(other.DateTime);
        }
        public int Id { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        public User User { get; set; }
        public DateTime DateTime { get; set; }

        [ObservableProperty]
        private int numberCorrectAnswers;

        [ObservableProperty]
        private List<WrongQuestion> wrongQuestions = new();

        public QuizAttempt()
        {
        }
        public QuizAttempt(Quiz quiz)
        {
            Quiz = quiz;
            userPassed = numberCorrectAnswers >= 3;
        }
        [ObservableProperty]
        private bool userPassed;
        
    }
}