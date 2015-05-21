using System;
using Should;
using Xunit;

namespace SqlLocalDb.UnitTests
{
    public class AssemblyResourceTests
    {
        [Fact]
        public void FromAssembly_CreatesAResourceWhenAnEmbeddedResourceExistsInAssemblyWithAMatchingName()
        {
            GetType().Assembly.GetResource("TestResource.txt").ShouldNotBeNull();
        }

        [Fact]
        public void FromAssembly_ThrowsWhenTheEmbeddedResourceCanNotBeFound()
        {
            Assert.Throws<ArgumentException>(() => GetType().Assembly.GetResource("ResourceThatDoesNotExist"));
        }
    }
}