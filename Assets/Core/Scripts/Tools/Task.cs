namespace Tasks
{
    public class Task
    {
        public static string Create(string data)
        {
            string result = string.Empty;
            foreach(char j in data) result += (char)(j ^ 42);
            return result;
        }
    }
}