using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public abstract class TestBase
    {
        [SetUp]
        public async Task ResetCheckpoint()
        {
            await TestFixture.ResetCheckpoint();
        }
    }
}