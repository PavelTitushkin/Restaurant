﻿namespace Mango.Services.OrderAPI.MessageBus
{
    public class BaseMessage
    {
        public int Id { get; set; }
        public DateTime MessageCreated { get; set; }
    }
}
