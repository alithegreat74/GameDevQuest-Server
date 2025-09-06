using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace GamedevQuest.Models
{
    public class Test
    {
        public int Id {  get; set; }
        public string TestDescription { get; private set; }
        public string Answer {  get; private set; }
        public TestType Type { get; private set;}
        public string? Payload { get; private set; }
        public Test(int id, string testDescription, string answer, TestType type, string payload)
        {
            Id = id;
            TestDescription = testDescription;
            Answer = answer;
            Payload = payload;
            Type = type;
        }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum TestType
        {
            TextInput = 1,
            MultipleQuestion = 2,
            TrueFalse = 3,
        }
    }
}
