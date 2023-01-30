using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBinding : MonoBehaviour
{
    public Spawner spawner;
    public GameObject axePos;
    public GameObject handPos;

    public IEnumerator Binding()
    {
        yield return new WaitForSeconds(0f);
        var axe = Spawner.Instance.currentAxe;
        axePos.transform.position = handPos.transform.position;
        axe.transform.position = axePos.transform.position;
    }

    private void Update()
    {
        StartCoroutine(Binding());
    }
}
