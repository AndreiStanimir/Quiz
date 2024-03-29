﻿using CommunityToolkit.Mvvm.ComponentModel;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    [Table("Questions")]
    // write a paragraph about observable validator
    public partial class Question : ObservableValidator
    {
        [Key]
        public int Id { get; set; }
        [ObservableProperty]
        private string text;

        [Required, MinLength(1), MaxLength(4)]
        [ObservableProperty]
        private ObservableCollection<Answer> answers;

        public virtual ICollection<Models.Quiz> Quizzes { get; set; } = new List<Models.Quiz>();

        [Required, MinLength(1)]
        public IEnumerable<Answer> CorrectAnswers { get; private set; }

        [Required, Range(0,10000)]
        public int Number { get; set; }
        
        public Question()
        {

        }
        public Question(ObservableCollection<Answer> answers)
        {
            this.answers = answers;
            CorrectAnswers = Answers.Where(a => a.Correct).ToList();
        }
        // generate copy constructor for Question
        public Question(Question question)
        {
            text = question.Text;
            answers = question.Answers;
            Number = question.Number;
            CorrectAnswers = question.CorrectAnswers;
        }
        public override bool Equals(object obj)
        {
            return obj is Question question &&
                   Id == question.Id &&
                   text == question.text &&
                   EqualityComparer<ObservableCollection<Answer>>.Default.Equals(answers, question.answers) &&
                   EqualityComparer<IEnumerable<Answer>>.Default.Equals(CorrectAnswers, question.CorrectAnswers) &&
                   Number == question.Number;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, text, answers, CorrectAnswers, Number);
        }
        public override string ToString()
        {
            return $"{Number}. {Text}";
        }        // generate copy constructor for Question
    }
}
