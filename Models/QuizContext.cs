using MedicalQuiz.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MedicalQuiz.Models;

public class QuizContext : DbContext
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public string DbPath { get; }

    public QuizContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "quizes.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    public void PopulateDB(string pathQuestions,string pathAnswers)
    {
        string text = File.ReadAllText(pathQuestions);
        var questionsWithAnswers=System.Text.RegularExpressions.Regex.Split(pathQuestions, @"\d*\.");


    }
}
