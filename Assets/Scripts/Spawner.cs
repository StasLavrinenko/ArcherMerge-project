using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    private Vector3 SpawnPos;

    public GameObject spawnObjectPrefab;
    public Transform SpawnPlaceholder;
    [field: SerializeField] public GameObject currentAxe { get; private set; }

    private float newSpawnDuration = 2f;

    #region Singleton;

    public static Spawner Instance;

    private void Awake()
    {
        SpawnNewObject();
        Instance = this;
    }

    #endregion

    public void SpawnNewObject()
    {
        SpawnPos = SpawnPlaceholder.position;
        currentAxe = Instantiate(spawnObjectPrefab, SpawnPos, Quaternion.Euler(-90, 0, 0));
        currentAxe.SetActive(true);
    }

    public void NewSpawnRequest()
    {
        Invoke(methodName:"SpawnNewObject", newSpawnDuration);
    }
}