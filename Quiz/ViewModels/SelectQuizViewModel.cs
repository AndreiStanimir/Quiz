using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
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
    partial class SelectQuizViewModel : ObservableObject
    {
        QuizContext quizContext;
        [ObservableProperty]
        private ObservableCollection<Quiz.Models.Quiz> quizzes;
        [ObservableProperty]
        public ObservableCollection<QuizAttempt> attempts;
        public SelectQuizViewModel()
        {
            quizContext = QuizContextFactory.GetContext();
            quizzes = quizContext.Quizzes.ToObservableCollection();
            var currentUser = UserService.GetCurrentUser();
            foreach(var quiz in quizzes)
            {
                quiz.BestAttempt = quizContext.QuizAttempts
                    .Where(attempt => attempt.User.Id == currentUser.Id)
                    .OrderByDescending(attempt => attempt.NumberCorrectAnswers)
                    .FirstOrDefault();
            }
        }
    }
}
