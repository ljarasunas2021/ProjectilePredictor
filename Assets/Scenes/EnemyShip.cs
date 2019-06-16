using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour {
	// Use this for initialization
	void Start () {
        // create initial random rotation
        float xRot = Random.Range(0, 360);
        float yRot = Random.Range(0, 360);
        float zRot = Random.Range(0, 360);
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(xRot, yRot, zRot));
    }
}
