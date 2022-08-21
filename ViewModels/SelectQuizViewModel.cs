using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ViewModels
{
    partial class SelectQuizViewModel : ObservableObject
    {
        QuizContext quizContext;
        [ObservableProperty]
        private ObservableCollection<Quiz.Models.Quiz> quizzes;
        public SelectQuizViewModel()
        {
            quizContext = new QuizContext();
            quizzes = quizContext.Quizzes.ToObservableCollection();
        }
    }
}
