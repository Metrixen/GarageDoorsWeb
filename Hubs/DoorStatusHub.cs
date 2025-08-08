using Microsoft.AspNetCore.SignalR;

namespace GarageDoorsWeb.Hubs
{
    /// <summary>
    /// SignalR hub used to broadcast door status changes to all connected clients.  When a door
    /// is opened or closed, the server can call <c>DoorStatusHub</c> methods to push
    /// notifications to users in real time.  Clients should subscribe to the hub at
    /// <c>/doorHub</c> and listen for the <c>ReceiveDoorStatus</c> event.
    /// </summary>
    public class DoorStatusHub : Hub
    {
        /// <summary>
        /// Send a door status update to all connected clients.  The door ID and its current
        /// open state are transmitted to update the UI on the client side.
        /// </summary>
        /// <param name="doorId">The unique identifier of the door.</param>
        /// <param name="isOpen">Whether the door is open (true) or closed (false).</param>
        public async Task BroadcastDoorStatusAsync(int doorId, bool isOpen)
        {
            await Clients.All.SendAsync("ReceiveDoorStatus", doorId, isOpen);
        }
    }
}