using Microsoft.Extensions.Logging;

namespace GarageDoorsWeb.Services
{
    /// <summary>
    /// Default implementation of <see cref="INotificationService"/> which logs notifications to
    /// the application logger.  This provides a simple placeholder that can be easily
    /// extended to integrate with email, SMS or push notification providers such as
    /// Twilio, SendGrid or Firebase Cloud Messaging.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ILogger<NotificationService> logger)
        {
            _logger = logger;
        }

        // Twilio configuration keys.  Populate these values in appsettings.json or environment
        // variables.  If missing, the service will simply log the notification instead of
        // sending an SMS.
        private readonly string? _twilioAccountSid;
        private readonly string? _twilioAuthToken;
        private readonly string? _twilioFromNumber;
        private readonly string? _twilioToNumber;

        public NotificationService(ILogger<NotificationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            // Read Twilio configuration from the "Twilio" section of appsettings.json
            _twilioAccountSid = configuration["Twilio:AccountSid"];
            _twilioAuthToken = configuration["Twilio:AuthToken"];
            _twilioFromNumber = configuration["Twilio:FromNumber"];
            // If a per-user notification number is supplied, prefer it, otherwise read from config
            _twilioToNumber = configuration["Twilio:ToNumber"];
        }

        private async Task SendSmsAsync(string body)
        {
            if (string.IsNullOrWhiteSpace(_twilioAccountSid) || string.IsNullOrWhiteSpace(_twilioAuthToken) || string.IsNullOrWhiteSpace(_twilioFromNumber) || string.IsNullOrWhiteSpace(_twilioToNumber))
            {
                // Missing configuration; log the message instead of sending
                _logger.LogInformation("SMS notification (mock): {Body}", body);
                return;
            }
            try
            {
                using var client = new HttpClient();
                var byteArray = System.Text.Encoding.ASCII.GetBytes($"{_twilioAccountSid}:{_twilioAuthToken}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "To", _twilioToNumber },
                    { "From", _twilioFromNumber },
                    { "Body", body }
                });
                var response = await client.PostAsync($"https://api.twilio.com/2010-04-01/Accounts/{_twilioAccountSid}/Messages.json", content);
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("SMS sent successfully: {Message}", body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SMS notification.");
            }
        }

        public async Task SendDoorOpenedAsync(string doorName, string openedBy)
        {
            var message = $"Garage door '{doorName}' was opened by {openedBy}.";
            _logger.LogInformation("Notification: {Message}", message);
            await SendSmsAsync(message);
        }

        public async Task SendDoorClosedAsync(string doorName, string closedBy)
        {
            var message = $"Garage door '{doorName}' was closed by {closedBy}.";
            _logger.LogInformation("Notification: {Message}", message);
            await SendSmsAsync(message);
        }

        public async Task SendDoorOpenTooLongAsync(string doorName, int durationMinutes)
        {
            var message = $"Garage door '{doorName}' has been open for {durationMinutes} minutes.";
            _logger.LogWarning("Notification: {Message}", message);
            await SendSmsAsync(message);
        }
    }
}