using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Modules.Common {
    
    public static class FileUtils {

        public static string getFileExt(string path) {
            return Path.GetExtension(path);
        }

        public static string getFileNameFromPath(string path) {
            return Path.GetFileNameWithoutExtension(path);
        }
    }
}
