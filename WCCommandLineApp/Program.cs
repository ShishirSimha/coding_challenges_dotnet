using System;
using System.IO;
using System.Linq;
using System.Text;

/// <summary>
/// CCWC is a command-line utility that mimics the basic functionality of the Unix 'wc' (word count) command.
/// It can count lines, words, bytes, and characters in a file or from piped input.
/// </summary>
class CCWC
{
    /// <summary>
    /// Entry point for the CCWC application.
    /// Usage: ccwc [-c|-l|-w|-m] &lt;filename&gt;
    /// If input is piped, reads from standard input.
    /// Flags:
    ///   -c : Print the byte count
    ///   -l : Print the line count
    ///   -w : Print the word count
    ///   -m : Print the character count
    /// If no flag is provided, prints line, word, and byte counts.
    /// </summary>
    /// <param name="args">Command-line arguments</param>
    static void Main(string[] args)
    {
        string input = "";
        bool fromPipe = Console.IsInputRedirected;
        string path = "";
        string flag = "";

        if (fromPipe)
        {
            // Read all input from standard input if redirected (piped)
            input = Console.In.ReadToEnd();
        }
        else if (args.Length > 0)
        {
            // Parse command-line arguments for flag and file path
            if (args[0].StartsWith("-"))
            {
                flag = args[0];
                path = args[1];
            }
            else
            {
                path = args[0];
            }

            // Read all text from the specified file
            input = File.ReadAllText(path);
        }
        else
        {
            // Print usage if no input is provided
            Console.WriteLine("Usage: ccwc [-c|-l|-w|-m] <filename>");
            return;
        }

        // Count bytes, lines, words, and characters
        int byteCount = Encoding.UTF8.GetByteCount(input);
        int lineCount = input.Split('\n').Length;
        int wordCount = input.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        int charCount = input.Length;

        // Format output based on the flag
        string result = flag switch
        {
            "-c" => $"{byteCount} {(fromPipe ? "" : path)}",
            "-l" => $"{lineCount} {(fromPipe ? "" : path)}",
            "-w" => $"{wordCount} {(fromPipe ? "" : path)}",
            "-m" => $"{charCount} {(fromPipe ? "" : path)}",
            _ => $"{lineCount} {wordCount} {byteCount} {(fromPipe ? "" : path)}"
        };

        // Print the result, trimming any extra spaces
        Console.WriteLine(result.Trim());
    }
}
