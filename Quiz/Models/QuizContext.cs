using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.BestAttempt)
            .WithOne(b => b.Quiz)
            .HasForeignKey<QuizAttempt>();
        //modelBuilder.Entity<Question>()
        //    .HasData(questions)
        //    ;
        //modelBuilder.Entity<Quiz>()
        //  .HasData(quizzes)
        //  ;
        //modelBuilder.Entity<Answer>()
        //  .HasData(answers1)
        //  ;
        //modelBuilder
        //modelBuilder.Entity<QuizAttempt>()
        //    .HasOne(q=>q.Quiz)
        //    .(q=>q.User)
        //    .ha
    }

    public QuizContext()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        DbPath = @"C:\Users\andre\source\repos\AndreiStanimir\Quiz\QuizTests\Resources\quizzes.db";
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
        //PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
        
        //if(!File.Exists(DbPath))
        //PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
        //SaveChanges();
    }
    public QuizContext(string dbPath)
    {

        DbPath = dbPath;
        Database.EnsureCreated();

        //PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
        //SaveChanges();

        //Database.EnsureCreated();
    }
    // make the following method async
    public void PopulateDB(string pathQuestions, string pathAnswers, int numberOfQuizes)
    {
        using var streamer = FileSystem.OpenAppPackageFileAsync(pathQuestions).Result;
        using var streamReader = new StreamReader(streamer);
        string text = streamReader.ReadToEndAsync().Result;
        var questionsWithAnswers = System.Text.RegularExpressions.Regex.Split(text, @"\n\d+\.")
            //.Take(50)
            ;
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
                .ToArray();
            List<Answer> answers=new List<Answer>();
            for (int i = 0; i < questionAnswers.Count(); i++)
            {
                char currentAnswerLetter = (char)('a' + i);
                var answer = questionAnswers[i];
                answers.Add(new Answer()
                {
                    Text = answer,
                    Correct = correctAnswersDic[id].Contains(currentAnswerLetter)
                }
                );
            }
                
            Question question = new Question(new System.Collections.ObjectModel.ObservableCollection<Answer>(answers))
            {
                //Id = id.ToString(),
                Text = questionWithAnswerSplit.First().Trim(),
            };
            Debug.Assert(question.CorrectAnswers.Count() > 0);
            //questionAnswers.ToList().ForEach(a=>a.Question=question);
            Answers.AddRange(question.Answers);
            //SaveChanges();
            Questions.Add(question);
            //SaveChanges();
        }
        SaveChanges();
        var quizes = GenerateQuizzes(Questions.ToList(), numberOfQuizes);
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
        this.BulkSaveChanges();
    }

    private ICollection<Quiz> GenerateQuizzes(IList<Question> questions, int numberOfQuizes, int numberOfQuestionsPerQuiz = 5)
    {
        if (questions.Count < numberOfQuestionsPerQuiz)
            throw new Exception("Not enough questions to generate quizzes.");

        questions.Shuffle();

        ICollection<Quiz> generatedQuizes = new List<Quiz>(numberOfQuizes);
        List<Question[]> quizzesQuestions = new();

        while (quizzesQuestions.Count < numberOfQuizes)
        {
            questions.Shuffle();
            quizzesQuestions.AddRange(Enumerable.Chunk<Question>(questions, numberOfQuestionsPerQuiz));
        }

        //if (quizzesQuestions.Any(q => q.Length != numberOfQuestionsPerQuiz))
        //    throw new Exception("Some quizzes do not have the required number of questions.");

        for (int id = 0; id < numberOfQuizes; id++)
        {
            var currentQuizQuestions = quizzesQuestions[id].ToList();
            var currentQuiz = new Quiz
            {
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
            Regex.Split(text, @"(\n\d+\.)\n")

            //.Take(50)
            .Select(q => q.Trim(' ', ',', ';', '.', '\t', '\n', '\r'))
            .Where(q => !string.IsNullOrWhiteSpace(q))
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
    public override void Dispose()
    {
        SaveChanges(); // Save any pending changes to the database
        Database.ExecuteSqlRaw("PRAGMA wal_checkpoint(FULL);");
        base.Dispose(); // Call the base Dispose method
    }
}

internal static class ExtensionsClass
{
    public static void Shuffle<T>(this IList<T> list)
    {
        Random rng = new(Environment.TickCount);
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
public record struct RawReadFiles2(string[] questionsWithAnswers, string[] correctAnswers, string[] questionNumbers)
{
    public static implicit operator (string[] questionsWithAnswers, string[] correctAnswers, string[] questionNumbers)(RawReadFiles2 value)
    {
        return (value.questionsWithAnswers, value.correctAnswers, value.questionNumbers);
    }
    public static implicit operator RawReadFiles2((string[] questionsWithAnswers, string[] correctAnswers, string[] questionNumbers) value)
    {
        return new RawReadFiles2(value.questionsWithAnswers, value.correctAnswers, value.questionNumbers);
    }
}