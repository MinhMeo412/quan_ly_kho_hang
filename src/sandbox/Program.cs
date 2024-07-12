using Figgle;

class Program
{
    static void Main()
    {

        string time = GetTime();
        Console.WriteLine(time);

        for (int i = 0; i < GetStringWidth(time); i++)
        {
            Console.Write("-");
        }
        Console.WriteLine();

        Console.WriteLine(GetStringWidth(time));
    }

    public static string GetTime()
    {
        DateTime now = DateTime.Now;

        // Format the time as HH:mm
        string currentTime = now.ToString("HH:mm");
        string timeWithSpaces = "";
        foreach (char letter in currentTime)
        {
            timeWithSpaces += $"{letter} ";
        }
        timeWithSpaces = timeWithSpaces.Trim();

        string asciiArt = FiggleFonts.Mini.Render(timeWithSpaces).TrimEnd();

        return asciiArt;
    }

    public static int GetStringWidth(string input)
    {
        List<string> phrases = input.Split('\n').ToList();
        int maxWidth = 0;
        foreach (string phrase in phrases)
        {
            if (phrase.Length > maxWidth)
            {
                maxWidth = phrase.Length;
            }
        }
        return maxWidth;
    }

    public static int GetStringHeight(string input)
    {
        List<string> phrases = input.Split('\n').ToList();
        int height = phrases.Count;

        return height;
    }
}
