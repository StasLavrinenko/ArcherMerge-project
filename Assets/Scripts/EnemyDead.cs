using System.Collections;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject Ragdoll;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "DeadZone")
        {
            Enemy.SetActive(false);
            Ragdoll.SetActive(true);

            Instantiate(Ragdoll, transform.position, transform.rotation);
        }
    }
}
