using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EReader {
    
    public class Library {

        public List<Shelf> shelves { get; set; }
        
        public Library() {}

        public Library(List<Shelf> shelves) {
            this.shelves = shelves;
        }

        public void serialize() {
            var serializer = new SerializerBuilder()
                .WithTagMapping("!basicBook", typeof(BasicBook))
                .WithTagMapping("!basicPage", typeof(BasicPage))
                .EmitDefaults()
                .DisableAliases()
                .Build();            
            var yaml = serializer.Serialize(this);
            File.WriteAllText(Config.Instance.libraryPath, yaml);
        }

        public static Library Deserialize() {
            
            string libraryContent;
            FileStream fileStream = new FileStream(Config.Instance.libraryPath, FileMode.Open, FileAccess.Read);
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8)) {
                libraryContent = streamReader.ReadToEnd();
            }
            
            StringReader yamlInput = new StringReader(libraryContent);
            Deserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithTagMapping("!basicBook", typeof(BasicBook))
                .WithTagMapping("!basicPage", typeof(BasicPage))
                .Build();

            Library library = deserializer.Deserialize<Library>(yamlInput);
            return library;
        }
    }
}