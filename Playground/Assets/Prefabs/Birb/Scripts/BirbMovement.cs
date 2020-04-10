using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BirbMovement : MonoBehaviour
{
    public Vector3 initialMovement;
    public float minimumDistanceApart;
    public float rotationSpeed;

    public bool isAlive;
    public Material deadMaterial;

    Rigidbody rb;

    List<Vector3> escapeDirections;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = initialMovement;
        escapeDirections = new List<Vector3>();
        isAlive = true;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            // do collision avoidance with current escape vectors
            doCollisionAvoidance(escapeDirections);

            // rotate to face direction of travel
            applyTransformRotation();

            // reset collision avoidance escape vectors
            escapeDirections.Clear();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // change material color
        GetComponent<MeshRenderer>().material = deadMaterial;

        isAlive = false;

        // apply gravity
        rb.useGravity = true;

        FindObjectOfType<RadialStatManager>().onBirbDied();
    }

    private void OnTriggerStay(Collider other)
    {
        if (isAlive)
        {
            if (other.tag == "Obstacle" || (other.tag == "Birb" && getDistanceToOther(other.transform) < minimumDistanceApart))
            {
                // determine escape vector from the obstacle and add it to the array
                escapeDirections.Add(determineEscapeVectorFromCollider(other));
            }

            else if (other.tag == "Birb" || other.GetComponent<BirbMovement>().isAlive)
            {
                // do flocking
                //doFlocking(other.gameObject);
            }
        }
    }

    float getDistanceToOther(Transform other)
    {
        return Vector3.Distance(transform.position, other.position);
    }

    void doFlocking(GameObject otherBirb)
    {
        // get the other birb's velocity 
        Vector3 otherVelocity = otherBirb.GetComponent<Rigidbody>().velocity;

        // get the other birb's position
        Vector3 toOtherPosition = (otherBirb.transform.position - transform.position).normalized;

        // change to match velocity
        rotateTowardsWithSpeed(otherVelocity);

        // draw rays
        Debug.DrawRay(transform.position, otherVelocity, Color.green);
    }

    Vector3 determineEscapeVectorFromCollider(Collider other)
    {
        // get vector to closest point of the other object
        Vector3 closestPointToOther = GetComponent<CapsuleCollider>().ClosestPointOnBounds(other.ClosestPoint(transform.position));
        Vector3 vectorToClosestPoint = closestPointToOther - transform.position;

        Vector3 vectorToEscape = (transform.position - closestPointToOther).normalized;

        // draw debug rays
        Debug.DrawRay(transform.position, vectorToClosestPoint, Color.red);
        Debug.DrawRay(transform.position, vectorToEscape, Color.cyan);

        // get vector away from the object
        return vectorToEscape;
    }

    void doCollisionAvoidance(List<Vector3> escapeVectors)
    {
        // do not do collision avoidance if there are no obstacles to avoid
        if (escapeVectors.Count != 0)
        {
            Vector3 allVectorsAdded = transform.position;

            // add each escape vector
            foreach (Vector3 currentEscapeVector in escapeVectors)
            {
                allVectorsAdded = allVectorsAdded + currentEscapeVector;
            }

            // normalize to get vector to escape
            Vector3 vectorToEscape = (allVectorsAdded - transform.position).normalized;

            // rotate momentum to face away vector
            rotateTowardsWithSpeed(vectorToEscape);

            // draw rays
            Debug.DrawRay(transform.position, vectorToEscape, Color.blue);
        }
    }

    void rotateTowardsWithSpeed(Vector3 destination)
    {
        rb.velocity = Vector3.RotateTowards(rb.velocity, destination, Time.deltaTime * rotationSpeed, 0.0f);
    }

    void applyTransformRotation()
    {
        transform.LookAt(transform.position - rb.velocity);
    }
}
