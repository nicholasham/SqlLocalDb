using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;

namespace SqlLocalDb.UnitTests
{
    public class AutoFixture
    {
        public AutoFixture()
        {
            Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        protected IFixture Fixture { get; set; }
    }
}