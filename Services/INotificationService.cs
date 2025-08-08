namespace GarageDoorsWeb.Services
{
    /// <summary>
    /// Abstraction for sending push notifications when garage doors change state.
    /// Implementations can deliver notifications via email, SMS, mobile push, etc.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Sends a notification indicating that a door has been opened.
        /// </summary>
        /// <param name="doorName">Human readable name of the door.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendDoorOpenedAsync(string doorName, string openedBy);

        /// <summary>
        /// Sends a notification indicating that a door has been closed.
        /// </summary>
        /// <param name="doorName">Human readable name of the door.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendDoorClosedAsync(string doorName, string closedBy);

        /// <summary>
        /// Sends a notification indicating that a door has been open for too long.
        /// </summary>
        /// <param name="doorName">Human readable name of the door.</param>
        /// <param name="durationMinutes">How long the door has been open, in minutes.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendDoorOpenTooLongAsync(string doorName, int durationMinutes);
    }
}