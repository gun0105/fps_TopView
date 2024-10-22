using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5.0f;

    float h;
    float v;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis ("Horizontal") * Time.deltaTime;
        v = Input.GetAxis("Vertical") * Time.deltaTime;
        transform.Translate(h * Speed, 0, v * Speed, Space.World);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100)) 
        {
            Vector3 pos = hit.point;
            pos.y = transform.position.y;
            transform.LookAt(pos);
        }
    }
}
