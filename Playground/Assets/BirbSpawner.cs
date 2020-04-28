using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbSpawner : MonoBehaviour
{
    public delegate void BirbCountDelegate(int birbCount);
    public event BirbCountDelegate birbCountChanged;

    public GameObject birbPrefab;
    public Transform birbSpawnParent;

    int birbCount;

    private void Start()
    {
        // get the starting number of birbs
        birbCount = GameObject.FindGameObjectsWithTag("Birb").Length;

        birbCountChanged(birbCount);
    }

    public void spawnBirb()
    {
        // get the container
        Transform container = GameObject.FindGameObjectWithTag("Container").transform;

        Vector3 newBirbPosition;
        bool positionFound = false;

        // loop until a position for the new birb is determined
        while (!positionFound)
        {
            // get container scale
            float containerScaleX = container.localScale.x;
            float containerScaleY = container.localScale.y;
            float containerScaleZ = container.localScale.z;

            // get a random position within the container
            Vector3 potentialBirbPosition = new Vector3(
                Random.Range(-1 * containerScaleX, containerScaleX),
                Random.Range(-1 * containerScaleY, containerScaleY),
                Random.Range(-1 * containerScaleZ, containerScaleZ));
            potentialBirbPosition = potentialBirbPosition + container.position;

            // determine if there is something already at the potential spawn position
            if (!Physics.SphereCast(new Ray(potentialBirbPosition, new Vector3(1, 0, 0)), 0.5f, 0.1f))
            {
                // a new position is found
                positionFound = true;
                newBirbPosition = potentialBirbPosition;
            }
        }

        // spawn a new birb at the found location
        Instantiate(birbPrefab, birbSpawnParent);

        birbCount++;

        // fire a new birbCountChange event
        birbCountChanged(birbCount);
    }
}
