using MedicalQuiz.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace MedicalQuiz.Models;

public class QuizContext : DbContext
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

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
        DbPath = System.IO.Path.Join(path, "quizes.db");
        Database.EnsureCreated();
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    public void PopulateDB(string pathQuestions, string pathAnswers)
    {
        string text = File.ReadAllText(pathQuestions);
        var questionsWithAnswers = System.Text.RegularExpressions.Regex.Split(text, @"\n\d+\.").Take(50);
        int id = 1;
        var correctAnswers = File.ReadAllText(pathAnswers).Split('\n')
            .Take(53)
            .Select(line => line.Trim().Split(" "));
        Dictionary<int, string> correctAnswersDic = new Dictionary<int, string>();
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
            Question question = new Question()
            {
                //Id = id.ToString(),
                Text = answers.First(),
            };
            Answers.AddRange(
            answers.Skip(1)
            .Select(answer => new Answer
            {
                //AnswerId=++id,
                Question = question,
                Text = answer,
                Correct = correctAnswersDic[id].Contains(answer.First())
            }));
            Questions.Add(question);
            
        }
        id++;
        this.SaveChanges();
    }
}