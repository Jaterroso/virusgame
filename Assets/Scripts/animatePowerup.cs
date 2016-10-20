using UnityEngine;
using System.Collections;

public class animatePowerup : MonoBehaviour {

	//Animate the powerup, also provides a destroy RPC

	void Start () {
	
	}
	
	public float maxAmp;
	public float minAmp;
	
	public Vector3 scale;
	void Update (){
		for (int i = 0; i < 3; ++i) scale[i] = maxAmp + minAmp * Mathf.Sin(Time.time);
		transform.localScale = scale;
		transform.Rotate (0, 1, 0);
	}


	public void destroy(){
		GetComponent<PhotonView> ().RPC ("getDestroy", PhotonTargets.All);
	}

	[PunRPC]
	public void getDestroy(){
		Destroy (gameObject);
	}

}
