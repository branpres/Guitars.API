using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public abstract class TestsBase
    {
        [SetUp]
        public async Task ResetCheckpoint()
        {
            await TestsFixture.ResetCheckpoint();
        }
    }
}