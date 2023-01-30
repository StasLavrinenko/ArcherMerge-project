using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    private GameObject part;

    private void Start()
    {
        part = gameObject;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(part.name == "Tower1-d05")
        {
        part.SetActive(false);
        var destractPart = DestractionPart.FindObjectOfType<GameObject>();
        destractPart.SetActive(false);
        var destroyedPart = DestroyTower.FindObjectOfType<GameObject>();
        destroyedPart.SetActive(true);
        }
    }

    internal static void OnCollisionEnter()
    {
        throw new NotImplementedException();
    }
}
