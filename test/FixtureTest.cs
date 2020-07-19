using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Doublel.EntityAccessor.Tests
{
    public abstract class FixtureTest<TFixture> : IClassFixture<TFixture> where TFixture : class
    {
        protected FixtureTest(TFixture fixture) => Fixture = fixture;

        protected TFixture Fixture { get; }
    }
}
