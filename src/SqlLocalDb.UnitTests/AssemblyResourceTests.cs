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
            AssemblyResource.FromAssembly(GetType().Assembly, "TestResource.txt").ShouldNotBeNull();
        }

        [Fact]
        public void FromAssembly_ThrowsWhenTheEmbeddedResourceCanNotBeFound()
        {
            Assert.Throws<ArgumentException>(() => AssemblyResource.FromAssembly(GetType().Assembly, "ResourceThatDoesNotExist"));
        }
    }
}