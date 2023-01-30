using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimManager : MonoBehaviour
{
    public Spawner spawner;

    public void AxeThrow()
    {
        var axe = Spawner.Instance.currentAxe;
        axe.GetComponent<Animator>().SetTrigger("Throwing");
    }
}