using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbTeleporter : MonoBehaviour
{
    GameObject container;
    Vector3 containerSize;

    private void Start()
    {
        // get the container
        container = GameObject.FindGameObjectWithTag("Container");

        // initialize container scale values
        containerSize = container.transform.localScale;
    }

    private void Update()
    {
        // draw a line to the center of the container
        Vector3 toCenterOfContainer = getVectorToCenter(transform, GameObject.FindGameObjectWithTag("Container").transform);

        // get the quantized vector
        Vector3 toCenterQuantized = getQuantityVector3D(toCenterOfContainer);

        Color colorOfRay = Color.green;

        // determine if out of bounds
        if (isOutOfBounds(toCenterOfContainer))
        {
            Vector3 teleportVector = toCenterQuantized;
            teleportVector.Scale(new Vector3(1.99f, 1.99f, 1.99f));

            // teleport to other side
            addVectorToTransform(teleportVector);
            colorOfRay = Color.red;
        }

        // draw vectors
        //Debug.DrawRay(transform.position, toCenterOfContainer);
        //Debug.DrawRay(transform.position, toCenterQuantized, colorOfRay);
    }

    void addVectorToTransform(Vector3 destination)
    {
        transform.position += destination;
    }

    bool isOutOfBounds(Vector3 vectorToCenter)
    {
        if (Mathf.Abs(vectorToCenter.x) >= containerSize.x/2 ||
            Mathf.Abs(vectorToCenter.y) >= containerSize.y/2 ||
            Mathf.Abs(vectorToCenter.z) >= containerSize.z/2)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    Vector3 getVectorToCenter(Transform currentObject, Transform destinationObject)
    {
        return destinationObject.position - currentObject.position;
    }

    Vector3 getQuantityVector3D(Vector3 inputVector)
    {
        Vector3 quantityVector = new Vector3(1, 1, 1);

        float largestValue = 0;

        if (Mathf.Abs(inputVector.x) > largestValue)
        {
            quantityVector = new Vector3(1, 0, 0);
            largestValue = Mathf.Abs(inputVector.x);
        }
        if (Mathf.Abs(inputVector.y) > largestValue)
        {
            quantityVector = new Vector3(0, 1, 0);
            largestValue = Mathf.Abs(inputVector.y);
        }
        if (Mathf.Abs(inputVector.z) > largestValue)
        {
            quantityVector = new Vector3(0, 0, 1);
            largestValue = Mathf.Abs(inputVector.z);
        }

        // quantize the inputVector
        inputVector.Scale(quantityVector);
        return inputVector;
    }
}
