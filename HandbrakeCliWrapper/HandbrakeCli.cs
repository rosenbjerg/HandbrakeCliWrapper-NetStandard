using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandbrakeCLIwrapper
{
    /// <summary>
    ///     A wrapper for Handbrake CLI with awaitable asyncronous conversion and
    /// </summary>
    public class HandbrakeCli
    {
        private static readonly Regex HandbrakeOutputRegex =
            new Regex(
                "Encoding:.*?, (\\d{1,3}\\.\\d{1,2}) %( \\((\\d{1,4}\\.\\d{1,2}) fps, avg (\\d{1,4}\\.\\d{1,2}) fps, ETA (\\d{2}h\\d{2}m\\d{2}s)\\))?",
                RegexOptions.Compiled);

        //private static readonly Regex HandbrakeShortOutputRegex =
        //    new Regex("Encoding:.*?, (\\d{1,3}\\.\\d{1,2}) %", RegexOptions.Compiled);

        private readonly string _hbCliPath;
        private Process _process;
        private string _out;

        /// <summary>
        /// Constructor for 
        /// </summary>
        /// <param name="handbrakePath">Path to HandBrake CLI executeable. Defaults to 'HandBrakeCLI'</param>
        public HandbrakeCli(string handbrakePath = "HandBrakeCLI")
        {
            _hbCliPath = handbrakePath;
        }

        /// <summary>
        /// The status of the converter and the on ongoing conversion is accessible here.
        /// </summary>
        public ConversionStatus Status { get; } = new ConversionStatus();

        /// <summary>
        /// Starts converting the input file using the configuration passed for HandBrake CLI
        /// </summary>
        /// <param name="config">The configuration builder for the conversion</param>
        /// <param name="inputFile">The path of the input file</param>
        /// <param name="outputDirectory">The directory to place the output file</param>
        /// <param name="outputFilename">The filename to give the output file. Defaults to the same as input filename. Automatically sets extension</param>
        /// <param name="overwriteExisting">Whether to overwrite existing files in outputDirectory</param>
        /// <param name="removeOriginalAfterSuccessful">Whether to remove the input file after successful transcoding</param>
        /// <returns>Awaitable task for the conversion</returns>
        public async Task Transcode(HandbrakeCliConfigBuilder config, string inputFile, string outputDirectory,
            string outputFilename = null, bool overwriteExisting = false, bool removeOriginalAfterSuccessful = false)
        {
            if (!File.Exists(inputFile))
                throw new HandbrakeCliWrapperException($"The input file '{inputFile}' could not be found");
            if (Status.Converting)
                throw new HandbrakeCliWrapperException("A conversion is already running");

            Directory.CreateDirectory(outputDirectory);
            var ext = $".{config.Format.ToString().Substring(3)}";
            if (string.IsNullOrEmpty(outputFilename))
                outputFilename = Path.GetFileNameWithoutExtension(inputFile) + ext;
            else if (!outputFilename.EndsWith(ext))
                outputFilename = Path.GetFileNameWithoutExtension(outputFilename) + ext;
            
            inputFile = Path.GetFullPath(inputFile);
            outputFilename = Path.Combine(Path.GetFullPath(outputDirectory), outputFilename);
            if (File.Exists(outputFilename) && !overwriteExisting) 
                throw new HandbrakeCliWrapperException($"The file '{outputFilename}' already exists. Set overwriteExisting to true to overwrite");

            var arg = $"-i \"{inputFile}\" -o \"{outputFilename}\" " + config.Build();
            _process = new Process
            {
                StartInfo = new ProcessStartInfo(_hbCliPath, arg)
            };
            _process.OutputDataReceived += OnOutputDataReceived;

            StartedTranscoding(inputFile, outputFilename);

            bool success;
            try
            {
                success = await AwaitProcess(_process);
            }
            catch (Exception e)
            {
                throw new HandbrakeCliWrapperException("An error occured when starting the HandbrakeCLI process. See inner exception", e);
            }
            _process = null;
            if (success)
            {
                DoneTranscoding();
                if (removeOriginalAfterSuccessful)
                {
                    try
                    {
                        File.Delete(inputFile);
                    }
                    catch (Exception e)
                    {
                        throw new HandbrakeCliWrapperException($"Could not remove original '{inputFile}'", e);
                    }
                }
            }
            else
                ErrorTranscoding();
        }

        /// <summary>
        /// Stops the current trancode job by killing the HandbrakeCLI process and then deleting the incomplete outputfile
        /// </summary>
        public void StopTranscoding()
        {
            if (_process == null)
                return;

            if (!_process.HasExited)
                _process.Kill();

            if (!File.Exists(_out))
                return;

            try
            {
                File.Delete(_out);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Invoked when a conversion has been completed succesfully
        /// </summary>
        public event EventHandler<HandbrakeTranscodingEventArgs> TranscodingCompleted;
        /// <summary>
        /// Invoked when a conversion has been started
        /// </summary>
        public event EventHandler<HandbrakeTranscodingEventArgs> TranscodingStarted;
        /// <summary>
        /// Invoked when an error occurs during a conversion
        /// </summary>
        public event EventHandler<HandbrakeTranscodingEventArgs> TranscodingError;

        private void StartedTranscoding(string inputFile, string outputFilename)
        {
            _out = outputFilename;
            Status.Converting = true;
            Status.InputFile = Path.GetFileName(inputFile);
            Status.OutputFile = Path.GetFileName(outputFilename);
            Status.Percentage = 0;
            Status.CurrentFps = 0;
            Status.AverageFps = 0;
            Status.Estimated = TimeSpan.Zero;
            TranscodingStarted?.Invoke(this, new HandbrakeTranscodingEventArgs(Status.InputFile));
        }

        private void DoneTranscoding()
        {
            _out = "";
            var inp = Status.InputFile;
            Status.Converting = false;
            Status.InputFile = "";
            Status.OutputFile = "";
            Status.Percentage = 0;
            Status.CurrentFps = 0;
            Status.AverageFps = 0;
            Status.Estimated = TimeSpan.Zero;
            TranscodingCompleted?.Invoke(this, new HandbrakeTranscodingEventArgs(inp));
        }

        private void ErrorTranscoding()
        {
            _out = "";
            var inp = Status.InputFile;
            Status.Converting = false;
            Status.InputFile = "";
            Status.OutputFile = "";
            Status.Percentage = 0;
            Status.CurrentFps = 0;
            Status.AverageFps = 0;
            Status.Estimated = TimeSpan.Zero;
            TranscodingError?.Invoke(this, new HandbrakeTranscodingEventArgs(inp));
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (string.IsNullOrEmpty(dataReceivedEventArgs.Data))
                return;
            var match = HandbrakeOutputRegex.Match(dataReceivedEventArgs.Data);
            if (!match.Success)
                return;

            Status.Percentage =
                float.Parse(match.Groups[1].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            if (!match.Groups[2].Success)
                return;

            Status.CurrentFps =
                float.Parse(match.Groups[3].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            Status.AverageFps =
                float.Parse(match.Groups[4].Value, NumberStyles.Float, CultureInfo.InvariantCulture);
            Status.Estimated =
                TimeSpan.ParseExact(match.Groups[5].Value, "h\\hm\\ms\\s", CultureInfo.InvariantCulture);
        }

        private static async Task<bool> AwaitProcess(Process process)
        {
            var tcs = new TaskCompletionSource<bool>();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.StartInfo.CreateNoWindow = true;
            process.Exited += Exited;

            void Exited(object sender, EventArgs eventArgs)
            {
                process.Exited -= Exited;
                tcs.SetResult(process.ExitCode == 0);
            }

            process.Start();
            process.BeginOutputReadLine();
            return await tcs.Task;
        }
    }

    public class HandbrakeTranscodingEventArgs : EventArgs
    {
        public HandbrakeTranscodingEventArgs(string inputFilename)
        {
            InputFilename = inputFilename;
        }
        public string InputFilename { get; }
    }

    public class HandbrakeCliWrapperException : Exception
    {
        public HandbrakeCliWrapperException(string msg) : base(msg)
        {
        }
        public HandbrakeCliWrapperException(string msg, Exception inner) : base(msg, inner)
        {
        }
    }
}