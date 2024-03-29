﻿using CommunityToolkit.Mvvm.ComponentModel;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.ViewModels
{
    public partial class QuizAttemptFinishViewModel : ObservableObject
    {
        [ObservableProperty]
        private QuizAttempt quizAttempt;
        private QuizContext context;
        public QuizAttemptFinishViewModel(QuizAttempt quizAttempt)
        {
            this.quizAttempt = quizAttempt;
        }

        public bool DidUserPass()
        {
            return QuizAttempt.NumberCorrectAnswers >= 3;
        }
    }
}