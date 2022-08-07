﻿using HandbrakeCliWrapper.Models;
using HandbrakeCliWrapper.Models.Exceptions;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandbrakeCliWrapper {
    /// <summary>
    /// A wrapper for Handbrake CLI with awaitable asyncronous conversion and
    /// </summary>
    public class Handbrake {
        private readonly Regex HandbrakeOutputRegex;
        private readonly string _hbCliPath;

        private Process _process;
        private string _out;

        /// <summary>
        /// The status of the converter and the on ongoing conversion is accessible here.
        /// </summary>
        public HandbrakeConversionStatus Status { get; } = new HandbrakeConversionStatus();

        /// <summary>
        /// Invoked when a conversion has been completed successfully
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

        /// <summary>
        /// Constructor for Handbrake wrapper
        /// </summary>
        /// <param name="handbrakePath">Path to HandBrake CLI executeable. Defaults to './HandBrakeCLI'</param>
        public Handbrake(string handbrakePath = "./HandBrakeCLI") {
            _hbCliPath = handbrakePath;
            HandbrakeOutputRegex = new Regex("Encoding:.*?, (\\d{1,3}\\.\\d{1,2}) %( \\((\\d{1,4}\\.\\d{1,2}) fps, avg (\\d{1,4}\\.\\d{1,2}) fps, ETA (\\d{2}h\\d{2}m\\d{2}s)\\))?", RegexOptions.Compiled);
        }

        /// <summary>
        /// Starts converting the input file using the configuration passed for HandBrake CLI
        /// </summary>
        /// <param name="config">The configuration for the conversion</param>
        /// <param name="inputFile">The path of the input file</param>
        /// <param name="outputDirectory">The directory to place the output file</param>
        /// <param name="outputFilename">The filename to give the output file. Defaults to the same as input filename. Automatically sets extension</param>
        /// <param name="overwriteExisting">Whether to overwrite existing files in outputDirectory</param>
        /// <param name="removeOriginalAfterSuccessful">Whether to remove the input file after successful transcoding</param>
        /// <returns>Awaitable task for the conversion</returns>
        public async Task Transcode(HandbrakeConfiguration config, string inputFile, string outputDirectory, string outputFilename = null, bool overwriteExisting = false, bool removeOriginalAfterSuccessful = false) {
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

            if (!File.Exists(_hbCliPath)) {
                throw new FileNotFoundException("No HandbrakeCLI executable found", _hbCliPath);
            }

            var arg = $"-i \"{inputFile}\" -o \"{outputFilename}\" {config}";
            _process = new Process {
                StartInfo = new ProcessStartInfo(_hbCliPath, arg)
            };
            _process.OutputDataReceived += OnOutputDataReceived;

            StartedTranscoding(inputFile, outputFilename);

            bool success;
            try {
                success = await AwaitProcess(_process);
            }
            catch (Exception e) {
                throw new HandbrakeCliWrapperException("An error occured when starting the HandbrakeCLI process. See inner exception", e);
            }

            _process = null;
            if (success) {
                DoneTranscoding();
                if (removeOriginalAfterSuccessful) {
                    try {
                        File.Delete(inputFile);
                    }
                    catch (Exception e) {
                        throw new HandbrakeCliWrapperException($"Could not remove original '{inputFile}'", e);
                    }
                }
            }
            else {
                ErrorTranscoding();
            }
        }

        /// <summary>
        /// Stops the current transcode job by killing the HandbrakeCLI process and then deleting the incomplete outputfile
        /// </summary>
        public void StopTranscoding() {
            if (_process == null)
                return;

            if (!_process.HasExited)
                _process.Kill();

            if (!File.Exists(_out))
                return;

            try {
                File.Delete(_out);
            }
            catch { }
        }

        private void SetStatus(string inputFile = "", string outputFilename = "") {
            _out = outputFilename;
            Status.Converting = !string.IsNullOrEmpty(inputFile);
            Status.InputFile = !string.IsNullOrEmpty(inputFile) ? Path.GetFileName(inputFile) : "";
            Status.OutputFile = !string.IsNullOrEmpty(outputFilename) ? Path.GetFileName(outputFilename) : "";
            Status.Percentage = 0;
            Status.CurrentFps = 0;
            Status.AverageFps = 0;
            Status.Estimated = TimeSpan.Zero;
        }

        private void StartedTranscoding(string inputFile, string outputFile) {
            SetStatus(inputFile, outputFile);
            TranscodingStarted?.Invoke(this, new HandbrakeTranscodingEventArgs(Status.InputFile));
        }

        private void DoneTranscoding() {
            SetStatus();
            TranscodingCompleted?.Invoke(this, new HandbrakeTranscodingEventArgs(Status.InputFile));
        }

        private void ErrorTranscoding() {
            SetStatus();
            TranscodingError?.Invoke(this, new HandbrakeTranscodingEventArgs(Status.InputFile));
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs) {
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

        private static async Task<bool> AwaitProcess(Process process) {
            var tcs = new TaskCompletionSource<bool>();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.StartInfo.CreateNoWindow = true;
            process.Exited += Exited;

            void Exited(object sender, EventArgs eventArgs) {
                process.Exited -= Exited;
                tcs.SetResult(process.ExitCode == 0);
            }

            process.Start();
            process.BeginOutputReadLine();
            return await tcs.Task;
        }
    }
}