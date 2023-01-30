using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject _camera;
    public GameObject battlePosCamera;
    public float duration;
    public WeaponCamFollow weaponCamFollow;
    public GameObject mergeButton;

    private void Start()
    {
        weaponCamFollow.enabled = false;
        mergeButton.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            mergeButton.SetActive(false);
            _camera.transform.DOMove(battlePosCamera.transform.position, duration);
            _camera.transform.DORotateQuaternion(battlePosCamera.transform.rotation, duration);
        }

        if (_camera.transform.position == battlePosCamera.transform.position)
        {
            weaponCamFollow.enabled = true;
        }
    }
}
