using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFirstPerson : MonoBehaviour
{
    public GameObject player;
    public float Xmod;
    public float Ymod;

    private Rigidbody rb;
    private Transform tr;
    private float angleX;
    private float angleY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = player.GetComponent<Rigidbody>();
        tr = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        angleX += Input.GetAxis("MouseX");

        angleX %= 360;

        angleY -= Input.GetAxis("MouseY");

        angleX *= Xmod;
        angleY *= Ymod;

        angleY = Mathf.Clamp(angleY, -90, 90);

        transform.position = player.transform.position;
        transform.rotation = Quaternion.Euler(angleY, angleX, 0);

        tr.rotation = Quaternion.Euler(0, angleX, 0);
    }
}
