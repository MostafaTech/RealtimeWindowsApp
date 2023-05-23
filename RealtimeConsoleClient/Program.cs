using Microsoft.AspNetCore.SignalR.Client;

namespace RealtimeConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://127.0.0.1:5142/realtime")
                .WithAutomaticReconnect()
                .Build();

            // starting the connection
            connection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("failed to connect to the signalr server: " + task.Exception?.GetBaseException()?.Message);
                }
                else
                {
                    Console.WriteLine("Connected with connectionId: " + connection.ConnectionId);
                }
            }).Wait();

            // start app if connection established
            if (connection.State == HubConnectionState.Connected)
            {
                Console.Write("Branch id: ");
                var branchId = Console.ReadLine();

                // register the branch
                connection.SendAsync("Register", branchId).Wait();
                Console.WriteLine("Branch Registered. Waiting for orders... (Press any key to exit)");

                // handle order received event
                connection.On<string>("print", (foodName) =>
                {
                    Console.WriteLine("New order received. food name: " + foodName);
                });

                // wait for any key to exit
                Console.Read();

                // stop the connection
                connection.StopAsync().ContinueWith(task =>
                {
                    Console.WriteLine("Disconnected");
                }).Wait();
            }

            Console.WriteLine("Bye");
        }
    }
}
