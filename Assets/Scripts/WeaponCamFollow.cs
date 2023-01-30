//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class WeaponCamFollow : MonoBehaviour
{
    public Transform target;
    [Range(1f,40f)] public float laziness = 10f;
    public bool lookAtTarget = true;
    public bool takeOffsetFromInitialPos = true;
    public Vector3 generalOffset;
    Vector3 whereCameraShouldBe;
    bool warningAlreadyShown = false;

    private void Start() {
        var _target = Spawner.Instance.currentAxe;
        if (takeOffsetFromInitialPos && _target != null) generalOffset = transform.position - _target.transform.position;
    }

    void FixedUpdate()
    {
        var _target = Spawner.Instance.currentAxe;
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
