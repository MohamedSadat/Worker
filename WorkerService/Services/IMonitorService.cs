using WorkerService.Data;

namespace WorkerService.Services
{
    public interface IMonitorService
    {
        Task<bool> CheckReachability(ServerModel address);
    }
}