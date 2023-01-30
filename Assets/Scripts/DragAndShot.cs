using DG.Tweening;
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class DragAndShot : MonoBehaviour
{
    [SerializeField] public TrajectoryManager trajectoryManager;

    public static Action Throwing;
    public static Action Swing;
    public static Action InSwing;
    public static Action OutSwing;


    private Rigidbody rb;
    private LineRenderer lineRendererComponent;

    public GameObject _axe;
    public GameObject bombs;
    public GameObject spawnPos;
    public GameObject lineRend;
    public GameObject enemyBallPrefab;
    public GameObject _cameraPos;

    public Buttons buttons;
    public Booster booster;
    public Spawner spawner;

    public AnimManager animManager;
    private Trajectory oldTrajectory;
    private Animator anim;
    public Trajectory trajectory;
    public GameObject _camera;
    private Camera cameraMain;

    public Transform _axePos;

    public static GameObject _enemyBall;

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    public Vector3 _speed;

    public Coroutine enemyFireCoroutine;

    float forceThrow;

    private bool isShoot;
    private static int shoots = 0;

    public float minForce;
    public float maxForce;
    public float forceMultiplier = 3f;

    private void Start()
    {
        rb = _axe.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lineRendererComponent = GetComponent<LineRenderer>();
        cameraMain = Camera.main;
    }

    //Shot logic - Slingshot
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePressDownPos = Input.mousePosition;
        }
        
        if(Input.GetMouseButton(0))
        {
            mouseReleasePos = Input.mousePosition;

            float enter;
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            new Plane (-Vector3.forward, transform.position).Raycast(ray, out enter);
            Vector3 mouseInWorld = ray.GetPoint(enter);
            Vector3 speed = (transform.position - mouseInWorld) * (forceMultiplier / 2);
            trajectory.ShowTrajectory(transform.position, speed);
            Vector3 etalonSpeed = new Vector3(8.91f, 8.41f);

            if(Vector3.Distance(speed, etalonSpeed) > 10f)
            {
                InSwing?.Invoke();
            }

            
            if (Vector3.Distance(speed, etalonSpeed) > 4f)
            {
                Swing?.Invoke();
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            mouseReleasePos = Input.mousePosition;
            Shoot(mousePressDownPos - Input.mousePosition);

            Throwing?.Invoke();
        }
    }


    //Enemy fire
    public IEnumerator EnemyFire()
    {
        yield return new WaitForSeconds(4f);
        forceThrow = Random.Range(minForce, maxForce);

        float randomY = Random.Range(5f, 10f);
        float randomX = Random.Range(-3f, -10f); 
        Vector3 angle = new Vector3(randomX, randomY);

        var enemyBall = Instantiate(enemyBallPrefab, _axePos.position, Quaternion.identity);
        _enemyBall = enemyBall;
        enemyBall.GetComponent<Rigidbody>().velocity = _axePos.forward * forceThrow + angle;
        Destroy(enemyBall.gameObject, 3f);
        //_camera.SetActive(true);

        yield return new WaitForSeconds(2f);
        _camera.SetActive(false);
        _camera.transform.position = _cameraPos.transform.position;
        _camera.transform.rotation = _cameraPos.transform.rotation;
        cameraMain.gameObject.SetActive(true);
    }

    public IEnumerator AxeThrow()
    {
        yield return new WaitForSeconds(0f);
        animManager.AxeThrow();
    }

    public IEnumerator CameraManager()
    {
        yield return new WaitForSeconds(2f);
        cameraMain.gameObject.SetActive(false);
        _camera.SetActive(true);
    }

    //Freezing axe
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("Wall"))
        {
            var axe = Spawner.Instance.currentAxe;
            axe.GetComponent<Rigidbody>().isKinematic = true;
            axe.GetComponent<Animator>().SetBool("Freezing", true);
        }
    }

    //Shoot calculate
    public void Shoot(Vector2 Force)
    {
        if (isShoot)
            return;

        rb.AddForce(new Vector2(Force.x, Force.y) * forceMultiplier);
        isShoot = true;
        Spawner.Instance.NewSpawnRequest();

        if (isShoot == true)
        {
            Debug.Log("+1 shoots");
            shoots += 1;

            trajectoryManager.SaveTrajectory(trajectory);
            enemyFireCoroutine = StartCoroutine(EnemyFire());
            StartCoroutine(CameraManager());
            StartCoroutine(AxeThrow());
        }
        
        if(shoots == 3)
        {
            buttons.ShowButtons();
        }
    }
}