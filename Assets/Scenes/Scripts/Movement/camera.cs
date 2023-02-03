using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject player;
    private Rigidbody rb;
    public float angleX;
    public float angleY;
    public float r;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        angleX += Input.GetAxis("MouseX");
        angleY = Mathf.Clamp(angleY -= Input.GetAxis("MouseY"), -85, 85);
        r = Mathf.Clamp(r -= Input.mouseScrollDelta.y, 1, 10);

        if (angleX > 360 || angleX < -360)
        {
            angleX = 0;
        }
        Vector3 orbit = Vector3.forward * r;
        orbit = Quaternion.Euler(angleY, angleX, 0) * orbit;
        transform.position = player.transform.position + orbit + offset;
        transform.LookAt (player.transform.position);
    }
}
