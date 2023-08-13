using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSphereManagment : MonoBehaviour
{
    private int numSegments = 20; // nombre de segments utilisés pour approximer la courbe

    public static LaserSphereManagment instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Il y a plus d'une instance de LaserSphereManagment");
            return;
        }
        instance = this;
    }

    public void SetLaser(LineRenderer laser, Transform startPoint, Transform endPoint)
    {
        // Configure le lineRenderer
        laser.positionCount = numSegments + 1;

        // calculer la courbe en forme d'arc-en-ciel
        Vector3[] positions = new Vector3[numSegments + 1];
        float deltaTheta = Mathf.PI / numSegments;
        float theta = 0f;
        for (int i = 0; i <= numSegments; i++)
        {
            float x = Mathf.Lerp(startPoint.position.x, endPoint.position.x, (float)i / numSegments);
            float y = Mathf.Lerp(startPoint.position.y, endPoint.position.y, (float)i / numSegments);
            float z = Mathf.Lerp(startPoint.position.z, endPoint.position.z, (float)i / numSegments);
            Vector3 position = new Vector3(x, y + 2.5f * Mathf.Sin(theta), z); // multiplier par 2 * Mathf.Sin(theta) pour la courbe soit plus haute
            positions[i] = position;
            theta += deltaTheta;
        }

        // assigner les positions au LineRenderer
        laser.SetPositions(positions);
    }
}