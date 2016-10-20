using UnityEngine;
using System.Collections.Generic;

public class bulletPool : MonoBehaviour {
    public GameObject poolee;
    public int prePooled = 10;
    public Stack<GameObject> inactive;

	// Very basic bullet pool script
	void Start () {
        inactive = new Stack<GameObject>();
	    for(int i = 0; i < prePooled; i++)
        {
            GameObject t = (GameObject)Instantiate(poolee, Vector3.zero, Quaternion.identity);
            t.SetActive(false);
            inactive.Push(t);
        }
	}

    public GameObject Spawn(Vector3 pos, Quaternion rot)
    {
        GameObject t = inactive.Pop();
        t.SetActive(true);
        t.transform.position = pos;
        t.transform.rotation = rot;
        t.GetComponent<BulletControl>().bp = this;
        return t;
    }

    public void Destroy(GameObject g)
    {
        g.SetActive(false);
        inactive.Push(g);
    }
}
