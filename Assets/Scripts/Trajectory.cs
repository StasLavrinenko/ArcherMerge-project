using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class Trajectory : MonoBehaviour
{
    private LineRenderer lineRendererComponent;

    private void Start()
    {
        lineRendererComponent = GetComponent<LineRenderer>();
    }

    //Calculate trajectory
    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[15];
        lineRendererComponent.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;

            points[i] = origin + speed * time + Physics.gravity * time * time / 2;

            if (points[i].y < 0)
            {
                lineRendererComponent.positionCount = i;
                break;
            }

        }
        lineRendererComponent.SetPositions(points);
    }
}