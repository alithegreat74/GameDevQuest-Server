namespace GamedevQuest.Models
{
    public class TestSolveAttempt
    {
        public int Id { get; private set; }
        public int TestId { get; private set; }
        public int UserId { get; private set; }
        public DateTime Date { get; private set; }
        public TestSolveAttempt(int userId, int testId, DateTime date)
        {
            UserId = userId;
            TestId = testId;
            Date = date;
        }
    }
}
