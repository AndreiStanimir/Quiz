using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public partial class WrongQuestion : Question
    {
        [ObservableProperty]
        ObservableCollection<Answer> answers;

        public WrongQuestion(Question question,List<Answer> answers) : base(question)
        {
            this.answers = answers.ToObservableCollection();
        }

        public WrongQuestion()
        {
            
        }
    }
}
