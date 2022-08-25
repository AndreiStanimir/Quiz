using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    }
}