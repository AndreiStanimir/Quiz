using CommunityToolkit.Mvvm.ComponentModel;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.DatabaseModels
{
    [Table("Questions")]
    public partial class Question : ObservableObject
    {
        [Key]
        public int Id { get; set; }
        [ObservableProperty]
        private string text;

        // [Required, MinLength(1), MaxLength(4)]
        [ObservableProperty]
        private ObservableCollection<Answer> answers;

        public virtual ICollection<Quiz.Models.Quiz> Quizzes { get; set; }

        public IEnumerable<Answer> CorrectAnswers
        {
            get => answers.Where(a => a.Correct).ToList();
        }
    }
}
