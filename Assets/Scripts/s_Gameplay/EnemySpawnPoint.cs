using UnityEngine;
using System.Collections;

public class EnemySpawnPoint : MonoBehaviour 
{
    private bool isReadyToSpawn = true;

    void SetToReady()
    {
        isReadyToSpawn = true;
    }

    public void SetToUsed()
    {
        isReadyToSpawn = false;
        Invoke("SetToReady", 5.0f);
    }

    public bool IsReadyToSpawn
    {
        get { return this.isReadyToSpawn; }
        //set { this.isReadyToSpawn = value; }
    }

}
