using System.Text;
using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;

public class BackgroundService : IHostedService
{
    private readonly ILogger _logger;
    private readonly HttpClient _client;
    private Timer? _timer;

    private Timer? _timer2;
    public BackgroundService(ILogger<BackgroundService> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _client = clientFactory.CreateClient(); 
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background service has started.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        _timer2 = new Timer(DoWork2, null, TimeSpan.FromSeconds(45), TimeSpan.FromSeconds(60));

        return Task.CompletedTask;
        //0,30,60,90,120,150
        //45,105,165
    }

    private void DoWork(object? state)
    {
        _logger.LogInformation("Redis database refresh service started.");

        MakeApiCall().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private async Task MakeApiCall()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7051/User/Odradi");
        request.Content = new StringContent("{\"key\":\"value\"}", Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"API call failed with status code {response.StatusCode}");
        }
    }

    private void DoWork2(object? state)
    {
        _logger.LogInformation("Redis database refresh service started.");

        MakeApiCall2().ConfigureAwait(false).GetAwaiter().GetResult();
    }

    private async Task MakeApiCall2()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7051/User/Odradi2");
        request.Content = new StringContent("{\"key\":\"value\"}", Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"API call failed with status code {response.StatusCode}");
        }
    }


    public Task StopAsync(CancellationToken cancellationToken)
    {
            
        _logger.LogInformation("Background Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        _timer2?.Change(Timeout.Infinite, 0);


        return Task.CompletedTask;
    }
}
