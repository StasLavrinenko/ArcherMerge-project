using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamFollow : MonoBehaviour
{
    private GameObject target;
    [Range(1f,40f)] public float laziness = 10f;
    public bool lookAtTarget = true;
    public bool takeOffsetFromInitialPos = true;
    public Vector3 generalOffset;
    Vector3 whereCameraShouldBe;
    bool warningAlreadyShown = false;

    private void Start() 
    {
        target = DragAndShot._enemyBall;
        var _target = target;
        if (takeOffsetFromInitialPos && _target != null) generalOffset = transform.position - _target.transform.position;
    }

    void FixedUpdate()
    {
        target = DragAndShot._enemyBall;
        var _target = target;
        if (_target != null) {
            whereCameraShouldBe = _target.transform.position + generalOffset;
            transform.position = Vector3.Lerp(transform.position, whereCameraShouldBe, 1 / laziness);

            if (lookAtTarget) transform.LookAt(_target.transform);
        } else {
            if (!warningAlreadyShown) {
                Debug.Log("Warning: You should specify a target in the simpleCamFollow script.", gameObject);
                warningAlreadyShown = true;
            }
        }
    }
}
