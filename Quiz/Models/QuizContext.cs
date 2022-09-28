using Microsoft.EntityFrameworkCore;
using Quiz.DatabaseModels;
using System.Collections.ObjectModel;
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

    private List<Question> questions = new List<Question>();
    private List<Quiz> quizzes = new List<Quiz>();
    private List<Answer> answers1 = new List<Answer>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<Question>()
        //    .HasData(questions)
        //    ;
        //modelBuilder.Entity<Quiz>()
        //  .HasData(quizzes)
        //  ;
        //modelBuilder.Entity<Answer>()
        //  .HasData(answers1)
        //  ;
    }

    public QuizContext()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        DbPath = @"C:\Users\andre\source\repos\AndreiStanimir\Quiz\QuizTests\Resources\quizzes.db";
        Database.EnsureDeleted();
        Database.EnsureCreated();
        PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);

        //if(!File.Exists(DbPath))
        //PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
        //SaveChanges();
    }
    public QuizContext(string dbPath)
    {
        DbPath = dbPath;
        //PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
        //SaveChanges();

        Database.EnsureCreated();
    }

    private ICollection<Quiz> GenerateQuizes(IList<Question> questions, int numberOfQuizes)
    {
        if (questions.Count < 100)
            throw new Exception();
        questions.Shuffle();

        ICollection<Quiz> generatedQuizes = new List<Quiz>(numberOfQuizes);
        List<Question[]> quizzesQuestions = new();
        while (quizzesQuestions.Count < numberOfQuizes)
        {
            questions.Shuffle();
            quizzesQuestions.AddRange(Enumerable.Chunk<Question>(questions.Skip(questions.Count % 100), 100));
        }
        if (quizzesQuestions.Any(q => q.Length != 100))
            throw new Exception("quiz questions was not 100");
        for (int id = 0; id < numberOfQuizes; id++)
        {
            var currentQuizQuestions = quizzesQuestions[id].Take(100).ToList();
            var currentQuiz = new Quiz
            {
                //Id = id,
                Questions = currentQuizQuestions,
                QuizName = "Quiz " + id
            };
            currentQuizQuestions.AsParallel().ForAll(question => question.Quizzes.Add(currentQuiz));
            generatedQuizes.Add(currentQuiz);
        }

        return generatedQuizes;
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

   



    public User GetCurrentUser { get; set; }

    public RawReadFiles ReadFiles(string pathQuestions, string pathAnswers)
    {
        using var streamer = FileSystem.OpenAppPackageFileAsync(pathQuestions).Result;
        using var streamReader = new StreamReader(streamer);
        string text = streamReader.ReadToEndAsync().Result;
        var questionsWithAnswersAndNumbers =
            System.Text.RegularExpressions.Regex.Split(text, @"(\n\d+\.)")

            //.Take(50)
            .Select(q => q.Trim(' ', ',', ';', '.', '\t', '\n', '\r'))
            .Where(q => !String.IsNullOrWhiteSpace(q))
            .ToArray();
        var questionNumbers = questionsWithAnswersAndNumbers.Where((x, i) => i % 2 == 0);
        var questionsWithAnswers = questionsWithAnswersAndNumbers.Where((x, i) => i % 2 == 1);
        if (questionsWithAnswers.Count() != 1023)
            throw new Exception();
        using var streamer2 = FileSystem.OpenAppPackageFileAsync(pathAnswers).Result;
        using var streamReader2 = new StreamReader(streamer2);
        var correctAnswers = streamReader2.ReadToEndAsync().Result.Split('\n');

        return new RawReadFiles(questionsWithAnswers.ToArray(), correctAnswers, questionNumbers.ToArray());
    }
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

public record struct RawReadFiles(string[] questionsWithAnswers, string[] correctAnswers, string[] questionNumbers)
{
    public static implicit operator (string[] questionsWithAnswers, string[] correctAnswers, string[] questionNumbers)(RawReadFiles value)
    {
        return (value.questionsWithAnswers, value.correctAnswers, value.questionNumbers);
    }

    public static implicit operator RawReadFiles((string[] questionsWithAnswers, string[] correctAnswers, string[] questionNumbers) value)
    {
        return new RawReadFiles(value.questionsWithAnswers, value.correctAnswers, value.questionNumbers);
    }
}