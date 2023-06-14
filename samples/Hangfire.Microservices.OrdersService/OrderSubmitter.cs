using System;

namespace Hangfire.Microservices.OrdersService
{
    [Queue("orders")]
    public class OrderSubmitter
    {
        [JobDisplayName("order {0} - status {1}")]
        public void Execute(long orderId, string status)
        {
            Console.WriteLine($"Submitting order {orderId} with status {status}");
        }
    }
}