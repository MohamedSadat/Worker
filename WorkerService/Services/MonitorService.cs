using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Config;
using WorkerService.Data;

namespace WorkerService.Services
{
    public class MonitorService : IMonitorService
    {

        public async Task<bool> CheckReachability(ServerModel address)
        {
            var ping = new Ping();
            try
            {
                string hostName = address.ServerIP;
                PingOptions options = new PingOptions
                {
                    DontFragment = true,

                };
                PingReply reply = await ping.SendPingAsync(hostName);
                Console.WriteLine($"Thread id {Thread.CurrentThread.ManagedThreadId}");


                 Console.WriteLine($"Ping status for ({hostName}): {reply.Status}");
                if (reply is { Status: IPStatus.Success })
                {
                    Console.WriteLine($"Address: {reply.Address}");
                    Console.WriteLine($"Roundtrip time: {reply.RoundtripTime}");
                    Console.WriteLine($"Time to live: {reply.Options?.Ttl}");
                    Console.WriteLine();
                    return true;
                }
                else if (reply is { Status: IPStatus.TimedOut })
                {
                    Console.WriteLine($"Timed out");
                    return false;
                }
                else if (reply is { Status: IPStatus.DestinationNetworkUnreachable })
                {
                    Console.WriteLine($"unreachable");
                    return false;
                }
                else
                    return false;
            }
            catch (PingException ex)
            {
                Console.WriteLine($"err :{address.ServerName} , {ex.Message}");
                GlobalConfiguration.logs.Add(new LogModel { Message = $"{address.ServerName} {ex.Message}", LogDate = DateTime.Now, ErrorCode = 505 });

                return false;
            }

        }
    }
}
