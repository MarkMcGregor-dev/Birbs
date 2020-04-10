using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadialStatManager : MonoBehaviour
{
    public GameObject totalBirbsText;
    public GameObject aliveBirbsText;
    public GameObject deadBirbsText;

    int birbsTotal;
    int birbsAlive;
    int birbsDead;

    private void Start()
    {
        // get all birbs
        GameObject[] allBirbs = GameObject.FindGameObjectsWithTag("Birb");

        // count all birbs
        birbsTotal = allBirbs.Count();

        // count alive birbs
        birbsAlive = allBirbs.Where(birb => birb.GetComponent<BirbMovement>().isAlive).Count();

        // count dead birbs
        birbsDead = birbsTotal - birbsAlive;

        // add a listener to the birbsCountChanged event
        FindObjectOfType<BirbSpawner>().birbCountChanged += onBirbSpawned;

        updateTexts();
        updateFillAmount();

        Debug.Log("Birbs Total: " + birbsTotal);
        Debug.Log("Birbs Alive: " + birbsAlive);
        Debug.Log("Birbs Dead: " + birbsDead);
    }

    void onBirbSpawned(int birbCount)
    {
        birbsTotal = birbCount;
        birbsAlive++;

        updateFillAmount();
        updateTexts();
    }

    public void onBirbDied()
    {
        birbsAlive = birbsAlive -1;
        birbsDead++;

        updateFillAmount();
        updateTexts();
    }

    void updateTexts()
    {
        totalBirbsText.GetComponent<Text>().text = birbsTotal.ToString();
        aliveBirbsText.GetComponent<Text>().text = birbsAlive.ToString();
        deadBirbsText.GetComponent<Text>().text = birbsDead.ToString();
    }

    void updateFillAmount()
    {
        GetComponent<Image>().fillAmount = (float) birbsAlive / (float) birbsTotal;
    }
}
