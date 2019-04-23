using NUnit.Framework;

namespace EReader.Tests {
    public class ConfigTest {
        
        [Test]
        public void ConfigAccess() {
            Config c = Config.Instance;
        }
        
    }
}