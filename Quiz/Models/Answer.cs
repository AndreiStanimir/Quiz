using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.DatabaseModels
{
    //[Keyless]
    [Table("Answers")]
    public partial class Answer : ObservableObject
    {
        [Key]
        [Required]
        public int AnswerId { get; set; }

        //public int QuestionId;
        //[Required]
        [ObservableProperty]
        private string text;

        [ObservableProperty]
        private bool correct;

        //[Required]
        //public Question Question { get; set; }

    }
}