using System;

namespace Hangfire.Microservices.OrdersService
{
    public class OrderSubmitter
    {
        public void Execute(long orderId, string status)
        {
            Console.WriteLine($"Submitting order {orderId} with status {status}");
        }
    }
}