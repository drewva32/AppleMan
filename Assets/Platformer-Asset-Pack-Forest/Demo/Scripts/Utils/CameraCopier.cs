using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppleBoy
{
    public class CameraCopier : MonoBehaviour
    {
        public Camera cameraToCopy;

        private new Camera camera;

        private void Start()
        {
            camera = GetComponent<Camera>();
        }
        public void Update()
        {
            camera.rect = cameraToCopy.rect;
            camera.orthographicSize = cameraToCopy.orthographicSize;
        }
    }
}
