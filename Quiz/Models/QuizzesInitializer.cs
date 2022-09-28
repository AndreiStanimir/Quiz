using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    internal class QuizzesInitializer : DropCreateDatabaseIfModelChanges<QuizContext>
    {
        protected override void Seed(QuizContext context)
        {
            GetCategories().ForEach(c => context.Categories.Add(c));
            GetProducts().ForEach(p => context.Products.Add(p));
        }
        public void PopulateDB(string pathQuestions, string pathAnswers, int numberOfQuizes)
        {
            //Database.EnsureDeleted();


            var (questionsWithAnswers, correctAnswers, questionNumbers) = ReadFiles(pathQuestions, pathAnswers);
            Dictionary<int, string> correctAnswersDic = new Dictionary<int, string>(correctAnswers.Count());
            foreach (var correctAnswer in correctAnswers)
            {
                var split = correctAnswer.Trim(' ', ',', ';', '.', '\t', '\n').Split(" ");
                correctAnswersDic[int.Parse(split[0].Trim())] = split[1].Trim();
            }

            for (var i = 0; i < questionsWithAnswers.Count(); i++)
            {
                var questionWithAnswer = questionsWithAnswers.ElementAt(i);
                var questionWithAnswerSplit = questionWithAnswer.Split("\r\n")
                    .Select(a => a.Trim().Trim(' ', ',', ';', '.', '\t', '\n'))
                    .Where(a => !string.IsNullOrWhiteSpace(a));
                if (questionWithAnswerSplit.Count() <= 1)
                    continue;
                var questionAnswers = questionWithAnswerSplit.Skip(1)
                    .Select(a => Regex.Replace(a.Trim(), "^ *[a-d]\\.", "", RegexOptions.Compiled)).ToArray();
                char letter = 'a';
                ObservableCollection<Answer> answers = new(questionAnswers.Select((answer, i) =>
                {
                    var a = new Answer
                    {
                        //AnswerId=++id,
                        //Question = question,
                        Text = answer,
                        Letter = letter,
                        Correct = correctAnswersDic[int.Parse(questionNumbers.ElementAt(i))].Contains(letter),
                    };
                    letter++;
                    return a;
                }
                                )
                                );
                if (answers == null)
                    throw new Exception("no answers found fo r" + questionWithAnswer);
                Question question = new Question(answers)
                {
                    //Id = id.ToString(),
                    Text = questionWithAnswerSplit.First().Trim(),
                    Number = int.Parse(questionNumbers.ElementAt(i)),
                };
                //questionAnswers.ToList().ForEach(a=>a.Question=question);
                answers1.AddRange(question.Answers);
                //SaveChanges();
                questions.Add(question);
                //SaveChanges();
            }
            SaveChanges();
            Questions.AttachRange(questions);
            var quizes = GenerateQuizes(questions, numberOfQuizes);
            quizzes.AddRange(quizes.ToArray());
            SaveChanges();

            //Questions.ForEachAsync(q =>
            //{
            //    Quizzes.Where(quiz => q.Id == quiz.Id)
            //    .ForEachAsync(quiz =>
            //        q.Quizzes.Add(quiz));
            //}
            //);
            this.SaveChanges();
        }
    }
}
