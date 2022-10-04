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

        public static QuizContext GetContext()
        {
            quizContext ??= new QuizContext();
            return quizContext;
        }

        public static QuizContext GetContext(string v)
        {
            quizContext ??= new QuizContext(v);
            return quizContext;
        }
    }
}
