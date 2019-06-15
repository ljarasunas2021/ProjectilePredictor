using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePredictor : MonoBehaviour {
    // projectile you want to mark
    public GameObject projectile;
    // a marker which shows where the projectile will be in secondsToPredict seconds
    public GameObject marker;
    // amount of second script should look ahead to see where it will be
    public float secondsToPredict;

    private void Start() {
        StartCoroutine(CreateMarker());
    }

    // create a marker
    private IEnumerator CreateMarker() {
        // every 1 seconds create a marker
        yield return new WaitForSeconds(1);
        // find the next position
        // **** what you will want to implement in the project****
        Vector3 nextPos = projectile.transform.position + projectile.GetComponent<Rigidbody>().velocity * secondsToPredict;
        // instantiate a new marker at the position the projectile will be in secondsToPredict seconds
        GameObject newMarker = Instantiate(marker, nextPos, Quaternion.Euler(new Vector3(0, 0, 0)));
        // destroy the marker in secondsToPredict seconds or when the projectile should be at that position
        Destroy(newMarker, secondsToPredict);
        // redo the courutine
        StartCoroutine(CreateMarker());
    }
}
