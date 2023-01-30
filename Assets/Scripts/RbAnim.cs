using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbAnim : MonoBehaviour
{
    public GameObject Axe;

    private Rigidbody rb;
    private Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            rb.isKinematic = true;
            anim.SetBool("Freezing", true);
        }
    }
}
