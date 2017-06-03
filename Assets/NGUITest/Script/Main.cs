using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGUITest
{
    public class Main : MonoBehaviour
    {

        Camera _camera;
        Transform _cube;
        // Use this for initialization
        void Start()
        {
            _camera = FindObjectOfType<Camera>();
            _cube = GameObject.Find("Cube").transform;
            _camera.GetSidesTest(Mathf.Lerp(_camera.nearClipPlane, _camera.farClipPlane, 0.5f), _cube);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.inputString == "@")
                _camera.GetSidesTest(Mathf.Lerp(_camera.nearClipPlane, _camera.farClipPlane, 0.5f), _cube);
        }
    }
}
