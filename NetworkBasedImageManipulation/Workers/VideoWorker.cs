using System.Diagnostics;
using System.Drawing;
using System;

namespace NetworkBasedImageManipulation.Workers
{
    public class VideoWorker
    {
        private const string defaultScreenshot = "D:\\stream_shot.png";

        private Process ffplay;
        private Process ffmpeg;
        private string connectionString;
        private string ffplayBinPath = "C:\\FFMPEG\\bin\\ffplay.exe";
        private string ffmpegBinPath = "C:\\FFMPEG\\bin\\ffmpeg.exe";
        private int playerHeight = 480;
        private int playerWidth = 640;

        public VideoWorker(string udpStreamSource = "127.0.0.1", int udpStreamIp = 12345)
        {
            connectionString = $"udp://{udpStreamSource}:{udpStreamIp}";
        }

        public void showStream()
        {
            ffplay = new Process();
            ffplay.StartInfo.FileName = ffplayBinPath;
            ffplay.StartInfo.Arguments = $"{connectionString} -x {playerWidth} -y {playerHeight}";
            ffplay.StartInfo.CreateNoWindow = true;
            ffplay.StartInfo.RedirectStandardOutput = true;
            ffplay.StartInfo.UseShellExecute = false;

            ffplay.EnableRaisingEvents = true;
            ffplay.OutputDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffplay");
            ffplay.ErrorDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffplay");
            ffplay.Exited += (o, e) => Debug.WriteLine("Exited", "ffplay");
            ffplay.Start();
        }

        public void closeStream()
        {
            try
            {
                ffplay.Kill();
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception occurred:\n{e.ToString()}", "ffplay");
            }
        }

        public Bitmap GetFrame(string fileName = defaultScreenshot)
        {
            if (!ffplay.HasExited)
                ffplay.Kill();
            ffplay.WaitForExit();

            ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = ffmpegBinPath;
            ffmpeg.StartInfo.Arguments = $"-y -i {connectionString} -vf scale=iw*sar:ih,setsar=1 -vframes 1 -q:v 1 {fileName}";
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.UseShellExecute = false;

            ffmpeg.EnableRaisingEvents = true;
            ffmpeg.OutputDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffmpeg");
            ffmpeg.ErrorDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffmpeg");
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            return new Bitmap(Image.FromFile(fileName));
        }
    }
}
