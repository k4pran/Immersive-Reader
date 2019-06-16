using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Modules.Common {
    
    public static class FileUtils {

        public static String getFileExt(String path) {
            return Path.GetExtension(path);
        }
    
    }
}
