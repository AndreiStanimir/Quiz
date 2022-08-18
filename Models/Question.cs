﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalQuiz.DatabaseModels
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }

        [Required,MinLength(1),MaxLength(4)]
        public ICollection<Answer> Answers { get; set; }
    }
}
