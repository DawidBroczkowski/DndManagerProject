using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static DndManager.Network.Utility.Enums;

namespace DndManager.Utility
{
    public class Message
    {
        public string MessageType { get; set; } = string.Empty;
        public string JsonPayload { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.Now;

        public Message(string messageType, string jsonPayload)
        {
            MessageType = messageType;
            JsonPayload = jsonPayload;
        }

        public Message(string messageType)
        {
            MessageType = messageType;
        }

        public Message()
        {
        }

        // Method to serialize the message to JSON
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        // Static method to deserialize JSON back to MessageWrapper
        public static Message FromJson(string json)
        {
            return JsonSerializer.Deserialize<Message>(json)!;
        }

        // Method to deserialize the JSON payload to a specific type
        public T GetPayload<T>()
        {
            return JsonSerializer.Deserialize<T>(JsonPayload)!;
        }

        public void SetPayload(object payload)
        {
            JsonPayload = JsonSerializer.Serialize(payload);
        }
    }
}
