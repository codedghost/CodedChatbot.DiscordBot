using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;

namespace CodedChatbot.DiscordBot.Helpers;

public static class InputHelper
{
    private static Regex whitespaceRegex = new Regex("[\u0000,\u200B]");

    public static string SanitiseInput(string input)
        => whitespaceRegex.Replace(input, "");
}