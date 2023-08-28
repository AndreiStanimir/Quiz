using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public partial class WrongQuestion : Question
    {
        [ObservableProperty]
        List<Answer> answers;

        public WrongQuestion(Question question,List<Answer> answers) : base(question)
        {
            this.answers = answers;
        }

        public WrongQuestion()
        {
            
        }
    }
}
