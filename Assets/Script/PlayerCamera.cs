using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

    public Transform player;
    public float sphereCastRadius = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit[] hits;
        //hits = Physics.RaycastAll(transform.position, transform.forward, Vector3.Magnitude(transform.position - player.position));
        hits = Physics.SphereCastAll(transform.position, sphereCastRadius, ((transform.position - player.position + player.GetComponent<Seeker>().orientation.ToVector())*-1).normalized, Vector3.Magnitude(transform.position - player.position)-sphereCastRadius);
        print((transform.position - player.position).normalized);


        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            
            if (hit.transform.GetComponent<Block>())
            {
                Block block = hit.transform.GetComponent<Block>();
                block.BecomeTransparent();
            }
        }
    }
}
