using UnityEngine;
using System.Collections;

public class EnemySprite : MonoBehaviour 
{
    public Camera enemyCam;

    void Update()
    {
        transform.LookAt(transform.position + enemyCam.transform.rotation * Vector3.forward,
        enemyCam.transform.rotation * Vector3.up);
    }
}
