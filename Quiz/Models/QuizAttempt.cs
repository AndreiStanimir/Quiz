using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public partial class QuizAttempt : ObservableObject
    {
        public int Id { get; set; }

        [Required]
        public Quiz Quiz { get; set; }

        public User User { get; set; }
        public DateTime DateTime { get; set; }

        [ObservableProperty]
        private int numberCorrectAnswers;

        public QuizAttempt()
        {

        }
        public QuizAttempt(Quiz quiz)
        {
            Quiz = quiz;
        }
    }
}