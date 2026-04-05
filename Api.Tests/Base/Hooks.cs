using Microsoft.Extensions.DependencyInjection;
using Reqnroll;
using System;
using System.Threading;

namespace API.Tests.Base
{
    [Binding]
    public sealed class Hooks
    {
        private static readonly AsyncLocal<IServiceScope?> Scope = new();

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            Scope.Value = BaseTest.Provider.CreateScope();
        }

        [AfterScenario(Order = 100)]
        public void AfterScenario()
        {
            Scope.Value?.Dispose();
            Scope.Value = null;
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            BaseTest.DisposeRootProvider();
        }

        internal static IServiceProvider Provider =>
            Scope.Value?.ServiceProvider
            ?? throw new InvalidOperationException("Scenario scope has not been initialized.");
    }
}