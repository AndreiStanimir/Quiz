using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Platform;
using Quiz.Models;
using Quiz.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ViewModels
{
    public partial class SelectQuizViewModel : ObservableObject
    {
        QuizContext quizContext;
        [ObservableProperty]
        private ObservableCollection<Quiz.Models.Quiz> quizzes;
        [ObservableProperty]
        public ObservableCollection<QuizAttempt> attempts;
        public SelectQuizViewModel()
        {
            this.quizContext = QuizContextFactory.GetContextAsync().Result;
            quizzes = quizContext.Quizzes.ToObservableCollection();
            //var currentUser = UserService.GetCurrentUserAsync(this.quizContext);
            foreach (var quiz in quizzes)
            {
                quiz.BestAttempt = quizContext.QuizAttempts
                    .Where(q => q.Id == quiz.Id)
                    //.Where(attempt => attempt.User.Id == currentUser.Id)
                    .OrderByDescending(attempt => attempt.NumberCorrectAnswers)
                    .FirstOrDefault();
            }

        }
    }
}
