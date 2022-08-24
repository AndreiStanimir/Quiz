using NUnit.Framework;
using Quiz.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xappium.UITest;
using Xappium.UITest.Pages;
namespace QuizTests.UI
{
    internal class UITests : XappiumTestBase
    {
        [Test]
        public void AppLaunches()
        {
            new SelectQuizPage();
        }
        [Test]
        public void Test()
        {
            SelectQuizPage selectQuizPage = new SelectQuizPage();
            Engine.Tap("quiz0");
            
        }
    }
}
