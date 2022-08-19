using Quiz.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public ICollection<Question> Questions{ get; set; }
    }
}
