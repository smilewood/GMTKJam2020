using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector2 BoundsH, BoundsV;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        Vector3 newPos = this.transform.position + move;
        newPos = Vector3.Min(newPos, new Vector3(BoundsH.x, newPos.y, BoundsV.x));
        newPos = Vector3.Max(newPos, new Vector3(BoundsH.y, newPos.y, BoundsV.y));
        this.transform.position = newPos;
    }
}
