using System.Diagnostics;
using System.Text;

namespace MyApi
{
    public class BashService
    {
        private readonly Process _process;

        public BashService()
        {
            _process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
            _process.Start();
        }

        public async Task<string> RunCommand(string input)
        {
            _process.StandardInput.WriteLine(input);
            _process.StandardInput.WriteLine("echo END_OF_COMMAND");

            var output = new StringBuilder();
            string? line;
            Console.WriteLine(input);

            while ((line = await _process.StandardOutput.ReadLineAsync()) != "END_OF_COMMAND")
            {
                output.AppendLine(line);
            }

            Console.WriteLine(output.ToString());
            return output.ToString();
        }
    }
}