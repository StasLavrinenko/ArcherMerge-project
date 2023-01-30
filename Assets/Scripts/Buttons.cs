using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public GameObject boosterBall;
    public GameObject _camera;
    public GameObject buttons;

    public Booster booster;
    public Transform boosterPos;
    public Spawner spawner;

    private GameObject currentAxe;

    //private Booster booster;

    public void ShowButtons()
    {
        buttons.SetActive(true);
    }

    public void onClickUse()
    {
        currentAxe = Spawner.Instance.currentAxe;
        currentAxe.SetActive(false);
        boosterBall.SetActive(true);
        _camera.SetActive(true);
        buttons.SetActive(false);
    }

}
