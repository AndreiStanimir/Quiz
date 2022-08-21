using Quiz.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Models;

public class QuizContext : DbContext
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    //public DbSet<Quiz> Quizzes { get; set; }
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
        DbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, "quizes.db");
        Database.EnsureCreated();
        PopulateDB(@"C:\Users\andreio\Documents\Visual Studio 2022\Projects\Quiz\Quiz\Resources\Raw\Questions.txt", @"C:\Users\andreio\Documents\Visual Studio 2022\Projects\Quiz\Quiz\Resources\Raw\GoodAnswers.txt");
        GenerateQuizes();
    }

    private void GenerateQuizes()
    {
        //List<Question> questions = Questions.ToList();
        //questions.Shuffle();
        //while (true)
        //{

        //}
        //questions.Sort(p => 1<new Guid());
    }


    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    public void PopulateDB(string pathQuestions, string pathAnswers)
    {
        string text = File.ReadAllText(pathQuestions);
        var questionsWithAnswers = System.Text.RegularExpressions.Regex.Split(text, @"\n\d+\.")
            .Take(50);
        int id = 1;
        var correctAnswers = File.ReadAllText(pathAnswers).Split('\n')
            .Take(53)
            .Select(line => line.Trim().Split(" "));
        this.BulkDelete(Answers);
        this.BulkDelete(Questions);

        SaveChanges();
        Dictionary<int, string> correctAnswersDic = new Dictionary<int, string>(correctAnswers.Count());
        foreach (var correctAnswer in correctAnswers)
        {
            correctAnswersDic[int.Parse(correctAnswer[0].Trim())] = correctAnswer[1].Trim();
        }
        foreach (var questionWithAnswer in questionsWithAnswers)
        {
            var answers = questionWithAnswer.Split("\r\n")
                .Select(a => a.Trim())
                .Where(a => !string.IsNullOrWhiteSpace(a));
            if (answers.Count() <= 1)
                continue;
            var questionAnswers = answers.Skip(1)
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
                Text = answers.First(),
                Answers = new System.Collections.ObjectModel.ObservableCollection<Answer>(questionAnswers.ToArray())
            };
            //questionAnswers.ToList().ForEach(a=>a.Question=question);
            Answers.AddRange(question.Answers);
            //this.SaveChanges();
            Questions.Add(question);
            //this.SaveChanges();

        }
        id++;
        this.BulkSaveChanges();
    }

}
static class ExtensionsClass
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