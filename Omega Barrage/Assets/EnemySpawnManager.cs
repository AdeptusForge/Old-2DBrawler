using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour {

    public bool spawnersActive = false;
    public float spawnTimer;
    public float maxSpawnTimer;
    public GameObject[] spawners = new GameObject[6];
    public GameObject enemyToSpawn;
    public GameObject enemyTarget;

    //public List<GameObject>() enemies = new List<GameObject>();





	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (spawnersActive)
        {
            if(spawnTimer <= 0)
            {
                GameObject enemyBeingSpawned = Instantiate(enemyToSpawn, spawners[Random.Range(0, spawners.Length)].transform) as GameObject;
                enemyBeingSpawned.transform.SetParent(enemyBeingSpawned.transform);
                //enemyToSpawn.GetComponent<EnemyFlyingDrone>().target = enemyTarget;
                spawnTimer = maxSpawnTimer;
            }
            if(spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
            }
        }
    }
}
