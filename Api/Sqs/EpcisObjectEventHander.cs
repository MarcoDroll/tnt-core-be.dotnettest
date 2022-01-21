namespace Api.Sqs
{
    public class EpcisObjectEventHander
    {
        public void DoTheMagic(string body)
        {
            Console.WriteLine($"Doing the magic with MessageBody: {body}");
        }
    }
}