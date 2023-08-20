using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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
        DbPath = @"C:\Users\andrei.stanimir\Source\Repos\AndreiStanimir\Quiz\QuizTests\Resources\quizzes.db";
        Database.EnsureDeleted();
        Database.EnsureCreated();

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
    private static readonly Regex QuestionSplitRegex = new(@"\n\d+\.", RegexOptions.Compiled);

    public async Task PopulateDB(string pathQuestions, string pathAnswers, int numberOfQuizes)
    {
        using var streamer = await FileSystem.OpenAppPackageFileAsync(pathQuestions);
        using var streamReader = new StreamReader(streamer);
        string text = await streamReader.ReadToEndAsync();
        var questionsWithAnswers = QuestionSplitRegex.Split(text);

        int id = 1;
        using var streamer2 = await FileSystem.OpenAppPackageFileAsync(pathAnswers);
        using var streamReader2 = new StreamReader(streamer2);
        var correctAnswers = (await streamReader2.ReadToEndAsync()).Split('\n')
            .Select(line => line.Trim().Split(" "));

        await SaveChangesAsync(); // Assuming there's an asynchronous version of SaveChanges
        Dictionary<int, string> correctAnswersDic = new(correctAnswers.Count());
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
            List<Answer> answers = new List<Answer>();
            for (int i = 0; i < questionAnswers.Length; i++)
            {
                char currentAnswerLetter = (char)('a' + i);
                var answer = questionAnswers[i];
                answers.Add(new Answer()
                {
                    Text = answer,
                    Correct = correctAnswersDic[id].Contains(currentAnswerLetter)
                });
            }

            Question question = new Question(new ObservableCollection<Answer>(answers))
            {
                Text = questionWithAnswerSplit.First().Trim(),
            };
            Debug.Assert(question.CorrectAnswers.Count() > 0);
            Answers.AddRange(question.Answers);
            Questions.Add(question);
        }
        await SaveChangesAsync();
        var quizes = await GenerateQuizzes(Questions.ToList(), numberOfQuizes); // Assuming GenerateQuizzes is async
        await Quizzes.AddRangeAsync(quizes); // Assuming AddRangeAsync exists
        await SaveChangesAsync();

        // Assuming there's an asynchronous version of SaveChanges and BulkSaveChanges
        await this.SaveChangesAsync();
        await this.BulkSaveChangesAsync();
        
    }

    private async Task<ICollection<Quiz>> GenerateQuizzes(IList<Question> questions, int numberOfQuizes, int numberOfQuestionsPerQuiz = 5)
    {
        await questions.ShuffleAsync();

        ICollection<Quiz> generatedQuizes = new List<Quiz>(numberOfQuizes);
        List<Question[]> quizzesQuestions = new();
        while (quizzesQuestions.Count < numberOfQuizes)
        {
            await questions.ShuffleAsync();
            quizzesQuestions.AddRange(Enumerable.Chunk<Question>(questions.Skip(questions.Count % numberOfQuestionsPerQuiz), numberOfQuestionsPerQuiz));
        }
        if (quizzesQuestions.Any(q => q.Length != numberOfQuestionsPerQuiz))
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
        => options.UseSqlite($"Data Source={DbPath};Pooling=False;");

    public User GetCurrentUser { get; set; }

    public async Task<RawReadFiles> ReadFilesAsync(string pathQuestions, string pathAnswers)
    {
        // Read both files concurrently
        var readQuestionsTask = ReadFileAsync(pathQuestions);
        var readAnswersTask = ReadFileAsync(pathAnswers);

        await Task.WhenAll(readQuestionsTask, readAnswersTask);

        var questionsText = readQuestionsTask.Result;
        var answersText = readAnswersTask.Result;

        var questionsWithAnswersAndNumbers =
            Regex.Split(questionsText, @"(\n\d+\.)\n")
            .Select(q => q.Trim(' ', ',', ';', '.', '\t', '\n', '\r'))
            .Where(q => !string.IsNullOrWhiteSpace(q))
            .ToArray();

        string[] questionNumbers = null, questionsWithAnswers = null;

        // Parallelize the processing of the data
        Parallel.Invoke(
            () => questionNumbers = questionsWithAnswersAndNumbers.Where((x, i) => i % 2 == 0).ToArray(),
            () => questionsWithAnswers = questionsWithAnswersAndNumbers.Where((x, i) => i % 2 == 1).ToArray()
        );

        var correctAnswers = answersText.Split('\n');

        return new RawReadFiles(questionsWithAnswers, correctAnswers, questionNumbers);
    }

    private async Task<string> ReadFileAsync(string path)
    {
        using var streamer = await FileSystem.OpenAppPackageFileAsync(path);
        using var streamReader = new StreamReader(streamer);
        return await streamReader.ReadToEndAsync();
    }

}

public static class ListExtensions
{
    public static async Task ShuffleAsync<T>(this IList<T> list)
    {
        int numberOfChunks = Environment.ProcessorCount; // Number of CPU cores

        var chunks = SplitList(list, numberOfChunks);

        var tasks = chunks.Select(chunk => Task.Run(() => ShuffleChunk(chunk))).ToArray();

        await Task.WhenAll(tasks);

        // Merge the shuffled chunks
        var shuffledList = new List<T>();
        foreach (var chunk in chunks)
        {
            shuffledList.AddRange(chunk);
        }

        // Copy the shuffled items back to the original list
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = shuffledList[i];
        }
    }

    private static void ShuffleChunk<T>(IList<T> chunk)
    {
        Random rng = new Random(Environment.TickCount);
        int n = chunk.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = chunk[k];
            chunk[k] = chunk[n];
            chunk[n] = value;
        }
    }

    private static List<T>[] SplitList<T>(IList<T> list, int numberOfChunks)
    {
        int sizeOfChunk = list.Count / numberOfChunks;
        var chunks = new List<T>[numberOfChunks];

        for (int i = 0; i < numberOfChunks; i++)
        {
            chunks[i] = new List<T>();
            int chunkStart = i * sizeOfChunk;
            int chunkEnd = (i == numberOfChunks - 1) ? list.Count : chunkStart + sizeOfChunk;
            for (int j = chunkStart; j < chunkEnd; j++)
            {
                chunks[i].Add(list[j]);
            }
        }

        return chunks;
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