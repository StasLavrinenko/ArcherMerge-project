using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDestruction : MonoBehaviour
{
    public GameObject bombs;

    [SerializeField] private Booster booster;

    [SerializeField] List<GameObject> partsTower;
    [SerializeField] List<GameObject> destractrionPartsTower;
}
