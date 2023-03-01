using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    // 3d Boid with target

    [SerializeField] private GameObject target = null;

    private float speed = 0.05f;
    private float rotationSpeed = 0.1f;
    private float neighbourDistance = 5f;
    private float avoidanceDistance = 3f;
    private float avoidanceWeight = 10f;
    private float alignmentWeight = 2f;
    private float cohesionWeight = 2f;
    private float targetWeight = 0.1f;


    private Vector3 direction;
    private Vector3 newDirection;
    private Vector3 avoidanceDirection;
    private Vector3 alignmentDirection;
    private Vector3 cohesionDirection;
    private Vector3 targetDirection;

    private List<Transform> neighbors = new List<Transform>();


    // Start is called before the first frame update
    void Start() {
        direction = transform.forward;
    }

    // Update is called once per frame
    void Update() {
        FindNeighbors();
        CalculateDirection();
        Move();
    }

    private void FindNeighbors() {
        neighbors.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, neighbourDistance);
        foreach (Collider collider in colliders) {
            if (collider.gameObject != gameObject) {
                neighbors.Add(collider.transform);
            }
        }
    }

    private void CalculateDirection() {
        newDirection = direction;
        if (neighbors.Count > 0) {
            CalculateAvoidanceDirection();
            CalculateAlignmentDirection();
            CalculateCohesionDirection();

            if (target != null) {
                CalculateTargetDirection();
                newDirection += avoidanceDirection + alignmentDirection + cohesionDirection + targetDirection;
                newDirection /= 4;
            } else {
                newDirection += avoidanceDirection + alignmentDirection + cohesionDirection;
                newDirection /= 3;
            }
        }
    }

    private void CalculateAvoidanceDirection() {
        int nAvoid = 0;
        avoidanceDirection = Vector3.zero;
        foreach (Transform neighbor in neighbors) {
            if (Vector3.Distance(transform.position, neighbor.position) < avoidanceDistance) {
                nAvoid++;
                avoidanceDirection += (transform.position - neighbor.position);
            }
        }
        if (nAvoid > 0) {
            avoidanceDirection /= nAvoid;
            avoidanceDirection *= avoidanceWeight;
        }
    }

    private void CalculateAlignmentDirection() {
        Vector3 alignmentDirection = Vector3.zero;
        foreach (Transform neighbor in neighbors) {
            alignmentDirection += neighbor.forward;
        }
        alignmentDirection /= neighbors.Count;
        newDirection += alignmentDirection * alignmentWeight;
    }

    private void CalculateCohesionDirection() {
        Vector3 cohesionDirection = Vector3.zero;
        foreach (Transform neighbor in neighbors) {
            cohesionDirection += neighbor.position;
        }
        cohesionDirection /= neighbors.Count;
        cohesionDirection -= transform.position;
        newDirection += cohesionDirection * cohesionWeight;
    }

    private void CalculateTargetDirection() {
        targetDirection = target.transform.position - transform.position;
        newDirection += targetDirection * targetWeight;
    }

    public void SetTarget(GameObject newTarget) {
        target = newTarget;
    }

    private void Move() {
        direction = Vector3.Lerp(direction, newDirection, rotationSpeed * Time.deltaTime);
        transform.position += direction * speed * Time.deltaTime;
        transform.forward = direction;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, neighbourDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidanceDistance);
    }
}
