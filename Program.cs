using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        string downloadUrl = "http://speedtest.tele2.net/1MB.zip";
        string uploadUrl = "http://httpbin.org/post";

        Console.WriteLine("إجراء اختبار سرعة التحميل...");
        double downloadSpeed = await MeasureDownloadSpeed(downloadUrl);
        Console.WriteLine($"سرعة التحميل: {downloadSpeed:F2} ميغابت في الثانية");

        Console.WriteLine("إجراء اختبار سرعة الرفع...");
        double uploadSpeed = await MeasureUploadSpeed(uploadUrl);
        Console.WriteLine($"سرعة الرفع: {uploadSpeed:F2} ميغابت في الثانية");
    }

    static async Task<double> MeasureDownloadSpeed(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            Stopwatch sw = Stopwatch.StartNew();
            byte[] data = await client.GetByteArrayAsync(url);
            sw.Stop();

            double bytesPerSecond = data.Length / sw.Elapsed.TotalSeconds;
            double bitsPerSecond = bytesPerSecond * 8;
            double megabitsPerSecond = bitsPerSecond / (1024 * 1024);
            return megabitsPerSecond;
        }
    }

    static async Task<double> MeasureUploadSpeed(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            byte[] data = new byte[1024 * 1024]; // 1MB of data
            MultipartFormDataContent content = new MultipartFormDataContent();
            content.Add(new ByteArrayContent(data), "file", "testfile");

            Stopwatch sw = Stopwatch.StartNew();
            HttpResponseMessage response = await client.PostAsync(url, content);
            sw.Stop();

            double bytesPerSecond = data.Length / sw.Elapsed.TotalSeconds;
            double bitsPerSecond = bytesPerSecond * 8;
            double megabitsPerSecond = bitsPerSecond / (1024 * 1024);
            return megabitsPerSecond;
        }
    }
}

