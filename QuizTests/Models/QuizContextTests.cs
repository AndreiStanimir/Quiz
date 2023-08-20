using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using NUnit.Framework;
using Castle.Core.Internal;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Models.Tests
{
    public class QuizContextTests
    {
        private QuizContext quizContext = null!;

        [OneTimeSetUp]
        protected void Setup()
        {
            var dbPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources", "quizzes.db");
            Assert.That(File.Exists(dbPath), Is.True);
            quizContext = QuizContextFactory.GetContext(dbPath);
        }

        [Test]
        public void AllQuizesHave100Questions()
        {
            Assert.IsNotEmpty(quizContext.Quizzes);
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
            Assert.IsNotEmpty(quizContext.Questions);
            foreach (var question in quizContext.Questions)
            {
                Assert.IsNotEmpty(question.Quizzes);
            }
        }

        [Test]
        public void HasMinimumQuestions()
        {
            int actual = quizContext.Questions.Count();
            Assert.Greater(actual, 100);
            foreach (var question in quizContext.Questions.ToList())
            {
                Assert.IsNotEmpty(question.Answers.ToList());
            }
            var uniqueQuestions = quizContext.Questions.ToList().Distinct((IEqualityComparer<Question>)new QuestionEqualityComparer());
            uniqueQuestions.Where(q => quizContext.Questions.Contains(q));

            Assert.AreEqual(uniqueQuestions.Count(), actual);
            Assert.AreEqual(1029, actual);
        }

        [Test]
        public void HasCorrectNumberOfQuizes()
        {
            Assert.AreEqual(15, quizContext.Quizzes.Count());
        }

        [Test]
        public void AllAnswersDontStartWithAnswerLetter()
        {
            IQueryable<string> actual = quizContext.Answers.Select(a => a.Text);
            Assert.IsNotEmpty(actual);
            Assert.That(actual, Has.None.StartsWith("a."));
            Assert.That(actual, Has.None.StartsWith("b."));
            Assert.That(actual, Has.None.StartsWith("c."));
            Assert.That(actual, Has.None.StartsWith("d."));
        }
        [Test]
        public void AllQuestionsHaveAtLeastCorrectOneAnswer()
        {
            Assert.That(quizContext.Questions.All(q => q.Answers.Count() > 0));
            Assert.That(quizContext.Questions.All(q => q.CorrectAnswers.Count()>0));
        }
        [Test]
        public void AllAnswersHaveValidText()
        {
            Assert.That(quizContext.Answers.All(a => !string.IsNullOrEmpty(a.Text)));
        }

        [Test]
        public void NoDuplicateQuestions()
        {
            // First, retrieve all questions from the database
            var allQuestions = quizContext.Questions.Include(q => q.Answers).ToList();

            // Then, perform the Distinct operation on the client side
            var distinctQuestions = allQuestions.Distinct(new QuestionEqualityComparer()).ToList();

            Assert.AreEqual(allQuestions.Count, distinctQuestions.Count);
        }



        [Test]
        public void NoDuplicateAnswersForAQuestion()
        {
            bool noDuplicates = quizContext.Questions.All(q =>
                q.Answers.Select(a => a.Text).Distinct().Count() == q.Answers.Count);
            Assert.IsTrue(noDuplicates);
        }

        [Test]
        public void AllQuizzesHaveUniqueQuestions()
        {
            bool areUnique = quizContext.Quizzes.All(quiz =>
                quiz.Questions.Distinct().Count() == quiz.Questions.Count);
            Assert.IsTrue(areUnique);
        }

        [Test]
        public void AllQuestionsHaveValidNumberRange()
        {
            // Assuming 0 to 10000 is the valid range
            Assert.That(quizContext.Questions.All(q => q.Number >= 0 && q.Number <= 10000));
        }
        public class QuestionEqualityComparer : IEqualityComparer<Question>
        {
            public bool Equals(Question x, Question y)
            {
                if (x == null || y == null)
                    return false;

                if (x.Text != y.Text)
                    return false;

                if (x.Answers.Count != y.Answers.Count)
                    return false;

                return x.Answers.OrderBy(a => a.Text).SequenceEqual(y.Answers.OrderBy(a => a.Text));
            }

            public int GetHashCode(Question obj)
            {
                unchecked
                {
                    int hashText = obj.Text == null ? 0 : obj.Text.GetHashCode();
                    int hashAnswers = 0;

                    foreach (var answer in obj.Answers)
                    {
                        hashAnswers ^= answer.Text.GetHashCode();
                    }

                    return hashText ^ hashAnswers;
                }
            }
        }
    }
}