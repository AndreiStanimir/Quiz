using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quiz.Models
{
    //[Keyless]
    [Table("Answers")]
    // Document observable pattern for objects
    public partial class Answer : ObservableObject
    {
        [Key]
        [Required]
        public int AnswerId { get; set; }
        [Required]
        public string Text { get; internal set; }



        [Required]
        public char Letter { get; set; }

        [ObservableProperty, DefaultValue(false)]
        private bool correct;

        [ObservableProperty]
        private string fullText;

        public Answer()
        {
            fullText = Letter + "." + Text;
        }

        public override bool Equals(object obj)
        {
            return obj is Answer answer &&
                   AnswerId == answer.AnswerId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AnswerId, Text, Letter, correct, fullText, Correct, FullText);
        }

        //[Required]
        //public Question Question { get; set; }

    }
}