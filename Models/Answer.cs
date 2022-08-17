using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalQuiz.DatabaseModels
{
    //[Keyless]
    [Table("Answers")]
    public class Answer
    {
        //[Key]
        //[Required]
        public int AnswerId { get; set; }

        //public int QuestionId;
        public string Text { get; set; }
        public bool Correct { get; set; }
 
        public Question Question { get; set; }

    }
}