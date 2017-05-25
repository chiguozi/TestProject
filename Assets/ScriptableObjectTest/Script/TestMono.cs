using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnumTest
{
    Test1,
    Test2,
}
public class TestMono : MonoBehaviour {

    public EnumTest e = EnumTest.Test1;
    public string stringField;
	// Use this for initialization
	void Start () {
        Debug.LogError(e.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
