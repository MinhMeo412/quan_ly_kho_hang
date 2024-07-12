using System;

class Program
{
    static void Main()
    {
        string message = "Hello from C#!";
        string cowSpeech = GenerateCowsay(message);
        Console.WriteLine(cowSpeech);
    }

    static string GenerateCowsay(string message)
    {
        string bubbleTop = " " + new string('_', message.Length + 2) + "\n";
        string bubbleMiddle = $"< {message} >\n";
        string bubbleBottom = " " + new string('-', message.Length + 2) + "\n";

        string cowSays = bubbleTop + bubbleMiddle + bubbleBottom +
                          "        \\   ^__^\n" +
                          "         \\  (oo)\\_______\n" +
                          "            (__)\\       )\\/\\\n" +
                          "                ||----w |\n" +
                          "                ||     ||\n";

        return cowSays;
    }
}
