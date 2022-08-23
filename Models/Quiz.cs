using CommunityToolkit.Mvvm.ComponentModel;
using Quiz.DatabaseModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    [Table("Quizzes")]
    public partial class Quiz : ObservableObject
    {
        [Key]
        public int Id { get; set; }
        [ObservableProperty]
        string quizName;
        [Required]
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
