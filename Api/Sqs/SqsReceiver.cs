using Amazon.SQS;
using Amazon.SQS.Model;

namespace Api.Sqs
{
    public class SqsReceiver : BackgroundService
    {
        private readonly ILogger<SqsReceiver> _logger;
        private readonly EpcisObjectEventHander epcisObjectEventHander;

        public event EventHandler<SqsReceiver> Receiver;

        public SqsReceiver(
            ILogger<SqsReceiver> logger,
            EpcisObjectEventHander epcisObjectEventHander)
        {
            this._logger = logger;
            this.epcisObjectEventHander = epcisObjectEventHander;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PollQueue(stoppingToken);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.ToString());
                }
                
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task PollQueue(CancellationToken stoppingToken)
        {
            string? accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
            string? secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string? sqsUrl = "https://sqs.eu-west-1.amazonaws.com/640202800658/tnt-core-ingest-test";

            Amazon.Runtime.BasicAWSCredentials? credentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            AmazonSQSClient amazonSQSClient = new(credentials, Amazon.RegionEndpoint.EUWest1);

            ReceiveMessageRequest receiveMessageRequest = new()
            {
                QueueUrl = sqsUrl,
                MaxNumberOfMessages = 1,
                WaitTimeSeconds = 1
            };

            ReceiveMessageResponse? response = await amazonSQSClient.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

            if (response.Messages.Any())
            {
                foreach (Message message in response.Messages)
                {
                    Console.WriteLine($"Message received");

                    epcisObjectEventHander.DoTheMagic(message.Body);

                    //Deleting message
                    DeleteMessageRequest? deleteMessageRequest = new DeleteMessageRequest(sqsUrl, message.ReceiptHandle);
                    await amazonSQSClient.DeleteMessageAsync(deleteMessageRequest, stoppingToken);

                    Console.WriteLine($"Message deleted");
                }
            }
        }
    }
}
