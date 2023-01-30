using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorManager : MonoBehaviour
{
    [SerializeField] private DragAndShot dragAndShot;
    private Vector3 mouseReleasePos;
    public float forceMultiplier = 3f;
    public float deviation;
    private Camera cameraMain;

    public GameObject ring1; public GameObject ringGreen1;
    public GameObject ring2; public GameObject ringGreen2;
    public GameObject ring3; public GameObject ringGreen3;
    public GameObject ring4; public GameObject ringGreen4;
    public GameObject ring5; public GameObject ringGreen5;
    public GameObject ring6; public GameObject ringGreen6;

    Vector3 etalonSpeed = new Vector3(45.5f, 54f);


    private void Start()
    {
        cameraMain = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mouseReleasePos = Input.mousePosition;

            float enter;
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            new Plane(-Vector3.forward, transform.position).Raycast(ray, out enter);
            Vector3 mouseInWorld = ray.GetPoint(enter);
            Vector3 speed = (transform.position - mouseInWorld) * (forceMultiplier / 2);

            if(Vector3.Distance(etalonSpeed, speed) <= deviation)
            {
                ring1.SetActive(false); ringGreen1.SetActive(true); ring2.SetActive(false); ringGreen2.SetActive(true);
                ring3.SetActive(false); ringGreen3.SetActive(true); ring4.SetActive(false); ringGreen4.SetActive(true);
                ring5.SetActive(false); ringGreen5.SetActive(true); ring6.SetActive(false); ringGreen6.SetActive(true);
            }
            else
            {
                ring1.SetActive(true); ringGreen1.SetActive(false); ring2.SetActive(true); ringGreen2.SetActive(false);
                ring3.SetActive(true); ringGreen3.SetActive(false); ring4.SetActive(true); ringGreen4.SetActive(false);
                ring5.SetActive(true); ringGreen5.SetActive(false); ring6.SetActive(true); ringGreen6.SetActive(false);
            }
        }
    }
}
