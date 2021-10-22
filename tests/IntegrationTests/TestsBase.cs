using NUnit.Framework;
using System.Threading.Tasks;

namespace IntegrationTests
{
    public class TestsBase
    {
        [SetUp]
        public async Task ResetCheckpoint()
        {
            await TestsSetup.ResetCheckpoint();
        }
    }
}