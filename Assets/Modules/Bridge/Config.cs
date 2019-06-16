using System;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Modules.Bridge {
    
    public class Config {

        // Singleton pattern
        private static Config instance;
        private static readonly object padlock = new object();
        
        // Defaults
        private static readonly string APP_PARENT_DIR = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static readonly string APP_NAME = "VReader";
        private static readonly string CONFIG_NAME = "config.yaml";

        public bool backupOnImport { get; private set; }
        public string libraryPath { get; private set; }
        public int linesPerPage { get; private set; }

        public Config() {
            if (instance != null){
                throw new NotSupportedException("Only one instance of Config is allowed. Access via Config.Instance");
            }
        }

        public static Config Instance {
            get {
                lock(padlock) {
                    if (instance == null) {
                        initialize();
                        instance = Deserialize();
                    }
                    return instance;
                }
            }
        }

        private static void initialize() {
            string appDir = Path.Combine(APP_PARENT_DIR, APP_NAME);
            if (!Directory.Exists(appDir)) {
                Directory.CreateDirectory(appDir);
            }

            string configPath = Path.Combine(appDir, "config.yaml");

            if (!File.Exists(configPath)) {
                var serializer = new SerializerBuilder()
                    .EmitDefaults()
                    .Build();                
                
                Config config = new Config();
                config.backupOnImport = true;
                config.libraryPath = Path.Combine(appDir, "library.yaml");
                config.linesPerPage = 27;
                
                var yaml = serializer.Serialize(config);
                File.WriteAllText(configPath, yaml);
            }
        }

        private static Config Deserialize() {
            
            string appDir = Path.Combine(APP_PARENT_DIR, APP_NAME);
            string configPath = Path.Combine(appDir, "config.yaml");
            
            string configContent;
            FileStream fileStream = new FileStream(configPath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                configContent = streamReader.ReadToEnd();
            }
            
            StringReader yamlInput = new StringReader(configContent);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();

            Config config = deserializer.Deserialize<Config>(yamlInput);
            return config;
        }
    }
}