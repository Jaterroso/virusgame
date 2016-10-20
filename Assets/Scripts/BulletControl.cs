using UnityEngine;
using System.Collections;

public class BulletControl : MonoBehaviour {
	public float speed;
	public GameObject explodeEffect;
	public PhotonPlayer shooter;
	public GameObject shooterScript;

    public bulletPool bp;

	// Moves the bullet forward
	void Update () {
		transform.position += transform.forward * (speed *Time.deltaTime);
	}

    // on collision spawn the explode effect, kill the zombie, add score, and put the bullet back in the pool
	void OnCollisionEnter(Collision c){
		Debug.Log ("Collision");
		Vector3 e = transform.rotation.eulerAngles;
		GameObject t = (GameObject)Instantiate (explodeEffect, transform.position, Quaternion.Euler (new Vector3 (e.x, e.y + 180, e.z)));
		t.GetComponent<ParticleSystem>().startColor = GetComponent<Renderer> ().material.color;
        if (c.gameObject.tag == "Zombie")
        {
            ScoreExtensions.AddScore(shooter, 1);
        }
        bp.Destroy (gameObject);
	}
}
