using Xunit;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.DatabaseModels;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace Quiz.Models.Tests
{
    public class QuizContextTests
    {
        QuizContext quizContext;

        public QuizContextTests()
        {
            quizContext = new QuizContext();
        }
        [Fact]
        public void AllQuizesHave100Questions()
        {
            foreach (var quiz in quizContext.Quizzes)
            {
                Assert.NotNull(quiz);
                Assert.NotNull(quiz.Questions);
                Assert.All(quiz.Questions, q => Assert.True(q.Answers.Count >= 3, $"Answers were {q.Answers.Count}"));
                Assert.Equal(100, quiz.Questions.Count);
            }

        }

        [Fact]
        public void AllQuestionsHaveAtLeastOneQuiz()
        {

            foreach (var question in quizContext.Questions)
            {
                Assert.True(question.Quizzes.Count > 0);
            }
        }
        [Fact]
        public void HasMinimumQuestions()
        {
            int actual = quizContext.Questions.Count();
            Assert.True(actual > 100);
            var uniqueQuestions = quizContext.Questions.ToList().Distinct((IEqualityComparer<Question>)new QuestionEqualityComparer());
            uniqueQuestions.Where(q => quizContext.Questions.Contains(q));

            Assert.Equal(uniqueQuestions.Count(), actual);
            Assert.Equal(1029, actual);
        }
        [Fact]
        public void HasCorrectNumberOfQuizes()
        {
            Assert.Equal(7, quizContext.Quizzes.Count());
        }
        class QuestionEqualityComparer : EqualityComparer<Question>
        {
            public override bool Equals(Question b1, Question b2)
            {
                if (b1 == null && b2 == null)
                    return true;
                else if (b1 == null || b2 == null)
                    return false;

                return b1.Text == b2.Text;
            }

            public override int GetHashCode([DisallowNull] Question obj)
            {
                return obj.Text.GetHashCode();
            }
        }
    }
}