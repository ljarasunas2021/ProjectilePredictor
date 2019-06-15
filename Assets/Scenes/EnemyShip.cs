using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour {

    // speed of enemy ship
    public float speed;
    // rigidbody of enemy ship
    private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
        // get rigidbody
        rigidbody = GetComponent<Rigidbody>();

        // create initial random rotation
        float xRot = Random.Range(0, 360);
        float yRot = Random.Range(0, 360);
        float zRot = Random.Range(0, 360);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
    }

    private void FixedUpdate() {
        // set the rigidbodies' velocity to its local transform.right axis times speed
        // means the rigidbody will travel to its right at a given speed
        rigidbody.velocity = transform.right * speed;
    }
}
