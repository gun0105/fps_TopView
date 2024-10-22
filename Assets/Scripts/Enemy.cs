using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public static Enemy enemy;

    public float speed = 1.0f;
    public float speedRotate = 100.0f;
    public float maxHp = 50.0f;
    public float Hp;
    public float point = 10.0f;
    float rotationAngle = 0;
    public GameObject prefabParticle;
    //public GameObject prefabParticle;

    private void Awake()
    {
        if (Enemy.enemy == null)
        {
            Enemy.enemy = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Hp = maxHp;
        transform.localScale = Vector3.one * Random.Range(1,2);
        Vector3 pos = transform.position;
        pos.y = transform.localScale.y / 2;
        transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.Find("Player");
        transform.LookAt(player.transform);

        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        rotationAngle += speedRotate * Time.deltaTime;
        Quaternion rot = Quaternion.Euler(rotationAngle, 0, 0);
        transform.rotation = transform.rotation * rot;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Hp -= 10;
            if (Hp <= 0)
            {
                Destroy(gameObject);
                GameObject obj = Instantiate(prefabParticle, transform.position, Quaternion.identity);
                Destroy(obj, 1f);
            }
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Hp -= 10;
            if (Hp <= 0)
            {
                Destroy(gameObject);
                GameObject obj = Instantiate(prefabParticle, transform.position, Quaternion.identity);
                Destroy(obj, 1f);
            }
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Bullet")) 
        {
            Destroy(gameObject);
            for (int i = 0; i < 10; i++) 
            {
                GameObject obj = Instantiate(prefabParticle, transform.position, Quaternion.identity);
                Destroy(obj,1);
            }
        }
    }*/
}
