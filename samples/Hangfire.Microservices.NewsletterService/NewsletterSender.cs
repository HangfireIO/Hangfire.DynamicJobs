using System;

namespace Hangfire.Microservices.NewsletterService
{
    [Queue("newsletter")]
    public sealed class NewsletterSender
    {

        [JobDisplayName("Newsletter {0}")]
        public static void Execute(long campaignId)
        {
            Console.WriteLine($"Processing newsletter '{campaignId}'");
        }
    }
}