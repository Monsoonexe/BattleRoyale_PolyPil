using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatArray : MonoBehaviour {

    private Vector2[] coordinates;//declaration

	// Use this for initialization
	void Start () {
        coordinates = new Vector2[6];//creates an array of 6 Vector2. each element will be null, until initl'd
        for(int i = 0; i < coordinates.Length; ++i)
        {
            coordinates[i] = new Vector2(Random.Range(-2f, 2.0f), Random.Range(-2f, 2.0f));//initialization

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
