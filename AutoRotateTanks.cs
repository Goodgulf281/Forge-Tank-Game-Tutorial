using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotateTanks : MonoBehaviour {

    public float speed = 5f; //degrees per second

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localRotation *= Quaternion.AngleAxis(speed * Time.deltaTime, Vector3.up);
    }
}
