using Quiz.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Quiz.Models;

public class QuizContext : DbContext
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public DbSet<Quiz> Quizzes { get; set; }

    public DbSet<QuizAttempt> QuizAttempts { get; set; }
    public DbSet<User> Users { get; set; }

    public string DbPath { get; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Question>()
    //        .HasOne<Answer>()
    //        .WithMany(q => q.Question)
    //        .HasForeignKey(q => q.AnswerId);

    //}

    public QuizContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = @"D:\quizes.db";
        Database.EnsureDeleted();
        Database.EnsureCreated();
        PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
        //SaveChanges();
        this.BulkSaveChanges();
    }

    public QuizContext(string dbPath)
    {
        DbPath = dbPath;
    }

    private ICollection<Quiz> GenerateQuizes(IList<Question> questions, int numberOfQuizes)
    {
        if (questions.Count < 100)
            throw new Exception();
        questions.Shuffle();

        ICollection<Quiz> generatedQuizes = new List<Quiz>(numberOfQuizes);
        List<Question[]> quizzesQuestions = new();
        while(quizzesQuestions.Count<numberOfQuizes)
        {
            questions.Shuffle();
            quizzesQuestions.AddRange(Enumerable.Chunk<Question>(questions.Skip(questions.Count % 100), 100));
        }
        if (quizzesQuestions.Any(q => q.Length != 100))
            throw new Exception("quiz questions was not 100");
        for (int id = 0; id < numberOfQuizes; id++)
        {
            generatedQuizes.Add(new Quiz
            {
                //Id = id,
                Questions = quizzesQuestions[id].Take(100).ToList(),
                QuizName = "Quiz " + id
            });
        }

        return generatedQuizes;
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    public void PopulateDB(string pathQuestions, string pathAnswers, int numberOfQuizes)
    {
        using var streamer = FileSystem.OpenAppPackageFileAsync(pathQuestions).Result;
        using var streamReader = new StreamReader(streamer);
        string text = streamReader.ReadToEndAsync().Result;
        var questionsWithAnswers = System.Text.RegularExpressions.Regex.Split(text, @"\n\d+\.")
            //.Take(50)
            ;
        if (questionsWithAnswers.Count() != 1024)
            throw new Exception();
        int id = 1;
        using var streamer2 = FileSystem.OpenAppPackageFileAsync(pathAnswers).Result;
        using var streamReader2 = new StreamReader(streamer2);
        var correctAnswers = streamReader2.ReadToEndAsync().Result.Split('\n')
            //.Take(53)
            .Select(line => line.Trim().Split(" "));

        SaveChanges();
        Dictionary<int, string> correctAnswersDic = new Dictionary<int, string>(correctAnswers.Count());
        foreach (var correctAnswer in correctAnswers)
        {
            correctAnswersDic[int.Parse(correctAnswer[0].Trim())] = correctAnswer[1].Trim();
        }
        foreach (var questionWithAnswer in questionsWithAnswers)
        {
            var questionWithAnswerSplit = questionWithAnswer.Split("\r\n")
                .Select(a => a.Trim().Trim(' ', ',', ';', '.', '\t', '\n'))
                .Where(a => !string.IsNullOrWhiteSpace(a));
            if (questionWithAnswerSplit.Count() <= 1)
                continue;
            var questionAnswers = questionWithAnswerSplit.Skip(1)
                .Select(a => Regex.Replace(a.Trim(), "^ *[a-d]\\.", "", RegexOptions.Compiled))
                .Select(answer => new Answer
                {
                    //AnswerId=++id,
                    //Question = question,
                    Text = answer,
                    Correct = correctAnswersDic[id].Contains(answer.First())
                });
            Question question = new Question()
            {
                //Id = id.ToString(),
                Text = questionWithAnswerSplit.First().Trim(),
                Answers = new System.Collections.ObjectModel.ObservableCollection<Answer>(questionAnswers.ToArray())
            };
            //questionAnswers.ToList().ForEach(a=>a.Question=question);
            Answers.AddRange(question.Answers);
            //SaveChanges();
            Questions.Add(question);
            //SaveChanges();
        }
        SaveChanges();
        var quizes = GenerateQuizes(Questions.ToList(), numberOfQuizes);
        Quizzes.AddRange(quizes);
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

    public User GetCurrentUser { get; set; }
}

internal static class ExtensionsClass
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random(Environment.TickCount);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}