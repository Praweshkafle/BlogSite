using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Helpers
{
    public class AlertHelper
    {
        public static void setMessage(Controller controller, string message, messageType message_type = messageType.success)
        {
            Alert alert = new Alert();
            alert.message = message;
            alert.message_type = message_type;
            controller.TempData["message"] = JsonConvert.SerializeObject(alert);
        }
    }
}
