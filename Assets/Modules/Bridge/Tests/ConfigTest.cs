using NUnit.Framework;

namespace Modules.Bridge.Tests {
    public class ConfigTest {
        
        [Test]
        public void ConfigAccess() {
            Config config = Config.Instance;
        }
        
    }
}