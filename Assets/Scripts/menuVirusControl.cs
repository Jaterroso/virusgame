using UnityEngine;
using System.Collections;

public class menuVirusControl : MonoBehaviour {

	// Adds spin to the menu virus
	void Start () {
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 100);
	}
}
