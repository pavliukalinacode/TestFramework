using System;
using System.Collections.Generic;
using System.Linq;
using UI.Apps.SauceDemo.Auth;

namespace UI.Apps.SauceDemo
{
    public sealed class SauceDemoOptions
    {
        public string BaseUrl { get; init; } = string.Empty;
        public List<SauceDemoUser> Users { get; init; } = [];

        public SauceDemoUser GetUser(SauceDemoUserType type)
        {
            var user = Users.FirstOrDefault(x => x.Type == type);

            return user ?? throw new InvalidOperationException(
                $"No Sauce Demo user configured for type '{type}'.");
        }
    }
}