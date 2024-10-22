using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject Enemy;
    public float spawnTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("EnemySpawn", 0, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void EnemySpawn() 
    {
        float random = Random.Range(-30, 30);

        Instantiate(Enemy, new Vector3(random, 1, random), Enemy.transform.rotation.normalized);
    }

}
