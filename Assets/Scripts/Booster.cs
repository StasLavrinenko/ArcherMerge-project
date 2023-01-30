using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] List<GameObject> partsTower;
    [SerializeField] List<GameObject> destractrionPartsTower;

    public GameObject character1;
    public GameObject character2;
    public GameObject character3;
    public GameObject character4;
    public GameObject character5;
    public GameObject character6;
    public GameObject character7;
    public GameObject character8;
    public GameObject ragdoll1;
    public GameObject ragdoll2;
    public GameObject ragdoll3;
    public GameObject ragdoll4;
    public GameObject ragdoll5;
    public GameObject ragdoll6;
    public GameObject ragdoll7;
    public GameObject ragdoll8;

    public GameObject boosterBall;
    public GameObject tower;
    public GameObject towerRb;
    public GameObject destroyTower;

    public Transform cameraPos;
    public GameObject _camera;

    public ParticleSystem explosionParticle;
    private DestroyTower destroyTowerScript;

    public float radius;
    public float force;
    public float delay;


    private void Start()
    {
        explosionParticle.Stop();
    }

    //Destroying tower
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.TryGetComponent<DestractionPart>(out DestractionPart destractionPart))
        {
            Explosion();
            explosionParticle.transform.position = boosterBall.transform.position;
            explosionParticle.Play();

            character1.SetActive(false); ragdoll1.SetActive(true);
            character2.SetActive(false); ragdoll2.SetActive(true);
            character3.SetActive(false); ragdoll3.SetActive(true);
            character4.SetActive(false); ragdoll4.SetActive(true);
            character5.SetActive(false); ragdoll5.SetActive(true);
            character6.SetActive(false); ragdoll6.SetActive(true);
            character7.SetActive(false); ragdoll7.SetActive(true);
            character8.SetActive(false); ragdoll8.SetActive(true);

            var part = destractionPart.gameObject;
            part.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    //Explosion bombs
    public void Explosion()
    {
        Collider[] overlappedColliders = Physics.OverlapSphere(transform.position, radius);

        for (int i = 0; i < overlappedColliders.Length; i++)
        {
            Rigidbody rigidbody = overlappedColliders[i].attachedRigidbody;
            if(rigidbody)
            {
                rigidbody.AddExplosionForce(force, transform.position, radius);
            }
        }
        Destroy(_camera.gameObject, delay);
        boosterBall.SetActive(false);
    }
}
