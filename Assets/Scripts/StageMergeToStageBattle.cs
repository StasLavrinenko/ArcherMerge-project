using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMergeToStageBattle : MonoBehaviour
{
    public WeaponCamFollow weaponCamFollow;
    public GameObject spawner;
    public GameObject _camera;
    public GameObject battlePosCamera;

    private void Update()
    {
        if (_camera.transform.position == battlePosCamera.transform.position)
        {
            spawner.SetActive(true);
        }
    }
}