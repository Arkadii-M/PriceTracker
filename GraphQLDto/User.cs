﻿using HotChocolate.Execution.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDto
{
    public record UserQL
    {
        public long UserId { get; set; }
        public string Username { get; set; } = null!;
        public byte[] Password { get; set; } = null!;
        public string Salt { get; set; } = null!;

        public ICollection<SubscriptionQL> Subscriptions { get; set; }
    }
    public record LoginUserQLInput(string Username, string Password);
    public record LoginUserQLPayload(string Username, bool is_login,string token);
    public record CreateUserQLInput(string Username, string Password);
    public record UserQLInput(long UserId, string Username, byte[] Password);
    public record UserQLPayload(long UserId, string Username, ICollection<SubscriptionQL>? Subscriptions);
}
