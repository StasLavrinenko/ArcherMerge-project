using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    [SerializeField] private int numberTrajectory;

    List<Trajectory> savesTrajectory = new List<Trajectory>();
    public void SaveTrajectory(Trajectory newtrajectory)
    {
        var trajectory = Instantiate(newtrajectory);
        savesTrajectory.Add(trajectory);

        if(savesTrajectory.Count >= numberTrajectory)
        {
            var oldtrajectory = savesTrajectory[0];
            savesTrajectory.RemoveAt(0);
            Destroy(oldtrajectory.gameObject);
        }
    }
}
