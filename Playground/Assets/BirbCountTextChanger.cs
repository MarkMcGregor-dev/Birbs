using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BirbCountTextChanger : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<BirbSpawner>().birbCountChanged += changeBirbCountText;
    }

    void changeBirbCountText(int newCount)
    {
        GetComponent<Text>().text = newCount.ToString();
    }
}
