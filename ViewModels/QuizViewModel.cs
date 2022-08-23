﻿using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Quiz.DatabaseModels;
using Quiz.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Quiz.ViewModels
{
    public partial class QuizViewModel : ObservableObject
    {
        QuizContext quizContext;
        [ObservableProperty]
        Question currentQuestion;
        ICollection<Question> questions;
        [ObservableProperty]
        private int correctAnswers;
       

        public QuizViewModel()
        {
            quizContext = new QuizContext();
            questions = quizContext.Questions.ToList();
            CurrentQuestion = questions.First();
            CorrectAnswers = 0;
        }

        [RelayCommand]
        public void GetNextQuestion()
        {
            CorrectAnswers++;
            questions.Remove(questions.First());
            CurrentQuestion = questions.First();
            //Answers = currentQuestion.Answers;
        }

    }
}