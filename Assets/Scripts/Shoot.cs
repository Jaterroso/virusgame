using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	public GameObject shootPoint;
	public GameObject shootEffect;
	public bool powerup = false;
	public float powerupTimer = 5.0f;
    public float shootCooldown = 0.3f;
	public GameObject bullet;
	public float scale = 1.0f;
	public Transform[] SpawnPositions;

	// deals with color and set up
	void Start () {
		Color c = new Color (Random.Range(0,1F),Random.Range(0,1F),Random.Range(0,1F));
		transform.GetChild(1).GetComponent<Renderer> ().material.color = c;
		GetComponent<PhotonView> ().RPC ("setColor", PhotonTargets.OthersBuffered, new Vector3(c.r, c.g, c.b));
		SpawnPositions = GameObject.FindGameObjectWithTag ("Controller").GetComponent<OnJoinedInstantiate> ().SpawnPositions;
	}

    //set color rpc
	[PunRPC]
	void setColor(Vector3 c){
		GetComponent<Renderer> ().material.color = new Color(c.x, c.y, c.z);
	}
	
	// calculates player rotation and some unused (in this build) powerup code
	void Update () {
		if (GetComponent<PhotonView> ().isMine) {
            if(shootCooldown > 0)
            {
                shootCooldown -= Time.deltaTime;
            }
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10f;
			
			Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
			mousePos.x = mousePos.x - objectPos.x;
			mousePos.y = mousePos.y - objectPos.y;
			
			float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (new Vector3 (0, -angle + 90, 0));
			if (Input.GetMouseButtonDown (0) && shootCooldown <= 0 && scale >= 0.6f) {
                GetComponent<AudioSource>().Play();
				GetComponent<PhotonView>().RPC("shoot", PhotonTargets.All, shootPoint.transform.position, 
				                               Quaternion.Euler (new Vector3 (0, -angle + 90, 0)), 
				                               GetComponent<PhotonView>().owner);
				scale -= 0.01f;
				GetComponent<MoveByKeys>().Speed = GetComponent<MoveByKeys>().startSpeed / scale;
				if(powerup){
					float[] test = {0.1f, angle};
					StartCoroutine("shootAgain", test);
				}
                shootCooldown += 0.3f;
			}
			transform.localScale = new Vector3(scale, transform.localScale.y, scale);
		}
	}

    //Shoot RPC, sets bullet color and particle color
	[PunRPC]
	void shoot(Vector3 pos, Quaternion rot, PhotonPlayer owner){
        
        GameObject temp = GetComponent<bulletPool>().Spawn(pos, rot);
		temp.GetComponent<Renderer> ().material.color = transform.GetChild(1).GetComponent<Renderer> ().material.color;
		temp.GetComponent<BulletControl>().shooter = GetComponent<PhotonView>().owner;
		GameObject p = (GameObject)Instantiate (shootEffect, pos, rot);
		p.GetComponent<ParticleSystem>().startColor = transform.GetChild(1).GetComponent<Renderer> ().material.color;
	}

    //Used for multishoot pwoerup, ultimately removed
	IEnumerator shootAgain(float[] variables){
		yield return new WaitForSeconds (variables[0]);
		GetComponent<PhotonView>().RPC("shoot", PhotonTargets.All, shootPoint.transform.position, 
		                               Quaternion.Euler (new Vector3 (0, -variables[1] + 90, 0)), 
		                               GetComponent<PhotonView>().owner);
	}

	public Transform SpawnPosition;
	public float PositionOffset = 5.0f;
	
    //deals with scaling on collision
	void OnCollisionEnter(Collision c){
		if(c.gameObject.tag == "Bullet"){
			if (scale < 2f) {
				scale += 0.2f;
			} else {
				scale = 2f;
			}
			GetComponent<MoveByKeys>().Speed = GetComponent<MoveByKeys>().startSpeed / scale;
		}
		if(c.gameObject.tag == "Zombie"){
			if(scale < 0.5f){
				Respawn();
			} else {
				scale -= 0.2f;
				GetComponent<MoveByKeys>().Speed = GetComponent<MoveByKeys>().startSpeed / scale;
			}
		}
		if (c.gameObject.tag == "Powerup") {
			if(scale > 1.5f){
				scale = 2.0f;
			} else {
				scale += 0.5f;
			}
			GetComponent<MoveByKeys>().Speed = GetComponent<MoveByKeys>().startSpeed / scale;
			c.gameObject.GetComponent<animatePowerup> ().destroy ();
			StartCoroutine("disablePowerup");
		}
	}

    //respawns slightly offset the spawn pos
	void Respawn(){
		int r = Random.Range(0, SpawnPositions.Length);
		SpawnPosition = SpawnPositions[r];
		Vector3 spawnPos = Vector3.up;
		if (this.SpawnPosition != null)
		{
			spawnPos = this.SpawnPosition.position;
		}

		Vector3 random = Random.insideUnitSphere;
		random.y = 0;
		random = random.normalized;
		Vector3 itempos = spawnPos + this.PositionOffset * random;
		scale = 1.0f;

		GetComponent<MoveByKeys>().Speed = GetComponent<MoveByKeys>().startSpeed / scale;
		transform.position = itempos;
	}

    //disable the powerup if it has a time effect, ultimately unused
	IEnumerator disablePowerup(){
		yield return new WaitForSeconds (powerupTimer);
        PhotonNetwork.Instantiate("Powerup", new Vector3(0, 0.5f, 0), Quaternion.identity,0);
		powerup = false;
	}

    //destroy self on leaving
	void OnLeftRoom(){
		PhotonNetwork.Destroy (gameObject);
	}
}
