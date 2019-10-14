using Xunit;

namespace Tests.Integration
{
    [Collection(Name)]
    public class IntegrationTestFixture : IClassFixture<DatabaseFixture>
    {
        public const string Name = nameof(IntegrationTestFixture);
        public readonly DatabaseFixture _fixture;

        public IntegrationTestFixture(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _fixture.ClearDataBase();
        }
    }
}
