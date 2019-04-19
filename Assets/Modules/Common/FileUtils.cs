using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Common {
    
    public class FileUtils {

        public static String getFileExt(String path) {
            return Path.GetExtension(path);
        }
    
    }
}
