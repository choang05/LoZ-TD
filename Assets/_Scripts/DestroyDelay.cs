using UnityEngine;
using System.Collections;
using PathologicalGames;


public class DestroyDelay : MonoBehaviour
{
    public float DelaySeconds = 0;
    public string PoolName = "";

    //  PoolManager
    public void OnSpawned()
    {
        // Start the timer as soon as this instance is spawned.
        StartCoroutine(DespawnDelay());
    }
    public void OnDespawned()
    {
        // Handle destruction visuals, like explosions and sending damage
        // information to nearby objects
        // ...
        
    }

    IEnumerator DespawnDelay()
    {
        yield return new WaitForSeconds(DelaySeconds);
        PoolManager.Pools[PoolName].Despawn(this.transform);
    }
}
