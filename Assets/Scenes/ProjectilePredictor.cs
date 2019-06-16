using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePredictor : MonoBehaviour {
    public GameObject turret;
    public GameObject projectile;
    public float projectileSpeed;
    public float secondsInBetweenShots;
    public GameObject enemyShip;
    public float enemyShipSpeed;

    private Rigidbody enemyShipRigidBody;

    private void Start() {
        enemyShipRigidBody = enemyShip.GetComponent<Rigidbody>();
        StartCoroutine(ShootProjectile());
    }

    // set velocity of ship to be its local right direction times its speed
    private void FixedUpdate()
    {
        enemyShipRigidBody.velocity = enemyShip.transform.right * enemyShipSpeed;
    }

    private IEnumerator ShootProjectile() {
        yield return new WaitForSeconds(secondsInBetweenShots);

        // Math behind the projectiles
        // ok so imagine a triangle with 3 vertices: the turret position, the current enemy ship position, and the future enemy ship position
        // using the law of sins
        // sin(currentShipPosAngle) / distance(turretPos, futureShipPos) = sin(futureShipPosAngle) / distance(shipPos,turretPos) = sin(180 - currentShipPosAngle - futureShipPosAngle) / distance(currentShipPos, futureShipPos)

        // from the law of sins we can get the following
        // distance(turretPos, futureShipPos) = distance(currentShipPos,turretPos) * sin(currentShipPosAngle) / sin(futureShipPosAngle)
        // distance(currentShipPos, futureShipPos) = distance(shipPos, turretPos) * sin(180 - currentShipPosAngle - futureShipPosAngle) / sin(futureShipPosAngle)

        // so the question is how can we make it so that the ship's and projectile's trajectories intersect at the same time
        // since distance = speed * time, time = distance / speed so:
        // distance(turretPos, futureShipPos) / turretVelo = distance(currentShipPos, futureShipPos) / shipVelo
        // using TurretVelo as speed of projectile fired by turret and shipVelo as speed of ship

        // by simplifying and plugging in stuff:
        // distance(shipPos,turretPos) * sin(currentShipPosAngle) / sin(futureShipPosAngle) * turretVelo = distance(shipPos, turretPos) * sin(180 - currentShipPosAngle - futureShipPosAngle) / sin(futureShipPosAngle) * shipVelo
        // turretVelo^(-1) * sin(currentShipPosAngle) / sin(futureShipPosAngle) = shipVelo^(-1) * sin(180 - currentShipPosAngle - futureShipPosAngle) / sin(futureShipPosAngle) 
        // sin(180 - currentShipPosAngle - futureShipPosAngle) = shipVelo * sin(currentShipPosAngle) / turretVelo
        // 180 - currentShipPosAngle - futureShipPosAngle = invSin(shipVelo * sin(currentShipPosAngle) / turretVelo)
        // futureShipPosAngle = 180 - currentShipPosAngle - invSin(shipVelo * sin(currentShipPosAngle) / turretVelo)
        // now we can find the futureShipPosAngle using measurements that we know
        // this will be implemented below

        // fakeFutureEnemyPosPoint: if the currentShipPos is the origin, fake future enemy pos point is a directional vector of where the future position will be
        Vector3 fakeFutureEnemyPosPoint = enemyShipRigidBody.velocity;
        // fakeTurretPos: turret position if the currentShipPos is the origin
        Vector3 fakeTurretPos = turret.transform.position - enemyShip.transform.position;
        // find the currentShipPosAngle using https://www.analyzemath.com/stepbystep_mathworksheets/vectors/vector3D_angle.html formula
        float currentShipPosAngle = Mathf.Acos(Vector3.Dot(fakeFutureEnemyPosPoint, fakeTurretPos) / (fakeFutureEnemyPosPoint.magnitude * fakeTurretPos.magnitude));

        // following is in radians due to Unity's obsession with radians
        // using formula found above, find futureShipPosAngle
        float futureShipPosAngle = Mathf.PI - currentShipPosAngle - Mathf.Asin(enemyShipSpeed * Mathf.Sin(currentShipPosAngle) / projectileSpeed);
        // using shipLength or distance(currentShipPos, futureShipPos), we can find it using equation found above
        float shipLength = Vector3.Distance(enemyShip.transform.position, turret.transform.position) * Mathf.Sin(Mathf.PI - currentShipPosAngle - futureShipPosAngle) / Mathf.Sin(futureShipPosAngle);
        // using shipLength and currentShipPos we can find the position where the projectile and it will intersect
        // since shipLength = distance & enemyShipSpeed = speed, shipLength / enemyShipSpeed is time:
        Vector3 realEndPoint = enemyShip.transform.position + enemyShipRigidBody.velocity * shipLength / enemyShipSpeed;

        // instantiate projectile
        // make it look at realEndPoint
        // set it along its velocity
        GameObject projectile1 = Instantiate(projectile, turret.transform.position, Quaternion.Euler(new Vector3(0,0,0)));
        projectile1.transform.LookAt(realEndPoint);
        projectile1.GetComponent<Rigidbody>().velocity = projectile1.transform.forward * projectileSpeed;

        // shoot another projectile
        StartCoroutine(ShootProjectile());
    }
}
