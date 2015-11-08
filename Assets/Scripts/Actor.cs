using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {

    private const float X_CORRECTION = 90f, Y_CORRECTION = 90f;

    private PlayerController player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        // TODO: make this more efficient by only enforcing when the player is visible

        Vector3 target = player.transform.position;
        target.y = transform.position.y;
        transform.LookAt(target, transform.position.z > player.transform.position.z ? Vector3.right : Vector3.left);

        Vector3 rot = transform.eulerAngles;
        rot.x += X_CORRECTION;
        rot.y += Y_CORRECTION;

        transform.rotation = Quaternion.Euler(rot);
	}
}
