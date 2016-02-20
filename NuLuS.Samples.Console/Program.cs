using System.Web.Script.Serialization;
using NuLuS.Api.Client;
using static System.Console;

namespace NuLuS.Samples.Console
{
    internal class Program
    {
        private static readonly string Separator = new string('-', 50);

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            while (true)
            {
                Write("Enter number: ");
                var number = ReadLine();
                Write("Enter product (mnp, hlr, or empty for default): ");
                var product = ReadLine();

                WriteLine(Separator);
                WriteLineWithSeparator("DNS lookup:");
                WriteLineWithSeparator(new JavaScriptSerializer().Serialize(NuLuSApiClientAsync.Instance.LookupAsync(product, number).GetAwaiter().GetResult()).PrettyPrintJSON());
                WriteLineWithSeparator("HTTP lookup:");
                WriteLineWithSeparator(new JavaScriptSerializer().Serialize(new NuLuSApiClientAsync(QueryChannel.HttpRest).LookupAsync(product, number).GetAwaiter().GetResult()).PrettyPrintJSON());
                WriteLine();
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private static void WriteLineWithSeparator(string line)
        {
            WriteLine(line);
            WriteLine(Separator);
        }
    }
}
