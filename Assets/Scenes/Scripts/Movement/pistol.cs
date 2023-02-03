using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pistol : MonoBehaviour
{
    private Rigidbody rb;
    private Transform tr;

    public float timer1 = 0;
    public float timer2 = 0;
    public float timer3 = 0;

    public float regcooldown;
    public float chargecooldown;
    public float chargetime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timer1 += Time.deltaTime;
        timer2 += Time.deltaTime;
        if (Input.GetMouseButtonDown (0) && timer1 > regcooldown)
        {
            RaycastHit hit;
            if (Physics.Raycast(tr.position, tr.forward, out hit))
            {
                Debug.Log(hit.collider.gameObject);
                timer1 = 0;
            }
        }
        if (Input.GetMouseButton (1) && timer2 > chargecooldown)
        {
            timer3 += Time.deltaTime;
            if (timer3 > chargetime)
            {
                Debug.Log("Charged");
            }
        }
        if (Input.GetMouseButtonUp (1))
        {
            if (timer3 > chargetime)
            {
                RaycastHit hit;
                if (Physics.Raycast(tr.position, tr.forward, out hit))
                {
                    Debug.Log(hit.collider.gameObject);
                    timer2 = 0;
                    timer3 = 0;
                }
            }
            else
            {
                timer3 = 0;
            }
        }
    }
}
