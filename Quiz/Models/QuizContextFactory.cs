using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public static class QuizContextFactory
    {
        static QuizContext quizContext = null;

        public static async Task<QuizContext> GetContextAsync()
        {
            if (quizContext is not null) return quizContext;
            quizContext = new QuizContext();
            await quizContext.PopulateDB(@"Questions.txt", @"GoodAnswers.txt", 15);
            return quizContext;
        }

        public static QuizContext GetContext(string v)
        {
            quizContext ??= new QuizContext(v);
            return quizContext;
        }
    }
}
