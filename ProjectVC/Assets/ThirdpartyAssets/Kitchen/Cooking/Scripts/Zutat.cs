using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Zutat : MonoBehaviour
{

    public string Sprache;
    public string Name;
    public string OrginalName;
    public int count;
    public GameObject ZutatPerfab;
    public GameObject ZutatBox;
    public bool inFridge;

    // Method to create (spawn) a number of ingredient objects
    public void ZutatErstellen(int count)
    {
        // Ensure the ZutatBox (the spawn container or point) is active
        ZutatBox.SetActive(true);

        // If ingredient is NOT in the fridge, spawn with varying height (stacked)
        if (!inFridge)
        {
            for (int i = 0; i < count; i++)
            {
                // Get current position; stack upwards for each instance
                var localPos = ZutatBox.transform.position;
                localPos.y = i; // Increase the y-position to stack items vertically
                Instantiate(ZutatPerfab, localPos, Quaternion.identity);
            }
        }
        // If inFridge is true, spawn all ingredients at the same position
        else
        {
            for (int i = 0; i < count; i++)
            {
                // All instances are spawned at exactly the box's position
                Instantiate(ZutatPerfab, ZutatBox.transform.position, Quaternion.identity);
            }
        }
    }

    private void Start()
    {
        ZutatErstellen(count);
    }
}
