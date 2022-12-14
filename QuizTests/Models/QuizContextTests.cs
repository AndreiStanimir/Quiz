using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.DatabaseModels;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using NUnit.Framework;

namespace Quiz.Models.Tests
{
    public class QuizContextTests
    {
        private QuizContext quizContext;

        [SetUp]
        protected void Setup()
        {
            var dbPath=Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "quizes.db");
            quizContext = new QuizContext(dbPath);
        }

        [Test]
        public void AllQuizesHave100Questions()
        {
            foreach (var quiz in quizContext.Quizzes)
            {
                Assert.NotNull(quiz);
                Assert.NotNull(quiz.Questions);
                Assert.True(quiz.Questions.All(q => q.Answers.Count >= 3));
                Assert.AreEqual(100, quiz.Questions.Count);
            }
        }

        [Test]
        public void AllQuestionsHaveAtLeastOneQuiz()
        {
            foreach (var question in quizContext.Questions)
            {
                Assert.True(question.Quizzes.Count > 0);
            }
        }

        [Test]
        public void HasMinimumQuestions()
        {
            int actual = quizContext.Questions.Count();
            Assert.True(actual > 100);
            var uniqueQuestions = quizContext.Questions.ToList().Distinct((IEqualityComparer<Question>)new QuestionEqualityComparer());
            uniqueQuestions.Where(q => quizContext.Questions.Contains(q));

            Assert.AreEqual(uniqueQuestions.Count(), actual);
            Assert.AreEqual(1029, actual);
        }

        [Test]
        public void HasCorrectNumberOfQuizes()
        {
            Assert.AreEqual(7, quizContext.Quizzes.Count());
        }

        [Test]
        public void AllAnswersDontStartWithAnswerLetter()
        {
            IQueryable<string> actual = quizContext.Answers.Select(a => a.Text);
            Assert.That(actual, Has.None.StartsWith("a."));
            Assert.That(actual, Has.None.StartsWith("b."));
            Assert.That(actual, Has.None.StartsWith("c."));
            Assert.That(actual, Has.None.StartsWith("d."));
        }

        private class QuestionEqualityComparer : EqualityComparer<Question>
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