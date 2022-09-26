namespace Receiver.Services.Connection
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IReceiveMsgService _processor;

        public Worker(IConfiguration config, ILogger<Worker> logger, IReceiveMsgService processor, IHostApplicationLifetime applicationLifetime)
        {
            _logger = logger;
            _processor = processor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("I'm working : {time}", DateTimeOffset.Now);

                await _processor.ReceiveMessageAsync();
                
                await Task.Delay(4000, stoppingToken);
            }
        }
    }
}
