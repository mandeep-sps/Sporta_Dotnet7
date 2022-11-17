using Microsoft.AspNetCore.SignalR;
using Sporta.WebApp.Models.ViewModel;
using System.Threading.Tasks;

namespace Sporta.WebApp.Hubs
{
    public class DashboardHub : Hub
    {
        public async Task NewDriveCreated(DriveRequestModel model)
        {
            await Clients.All.SendAsync("NewDriveCreated", model);
        }
    }
}
