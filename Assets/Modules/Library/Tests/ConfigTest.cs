using NUnit.Framework;

namespace Modules.Library.Tests {
    public class ConfigTest {
        
        [Test]
        public void ConfigAccess() {
            Config config = Config.Instance;
        }
        
    }
}