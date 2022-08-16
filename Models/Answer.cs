namespace MedicalQuiz.DatabaseModels
{
    public class Answer
    {
        int QuizId;
        public string Text { get; }
        public bool Correct { get; }
    }
}