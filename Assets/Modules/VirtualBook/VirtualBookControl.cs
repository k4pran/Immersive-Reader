using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.VirtualBook {
    
    public class VirtualBookControl : MonoBehaviour {

        public GameObject bookWrapper;
    
        void Start() {
            GameObject virtualBook = BookCreateUtils.GetVirtualBookPrefab();
            virtualBook.transform.parent = bookWrapper.transform;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
