using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float speedMod;
    public float jumpMod;
    public float slideMod;
    public float airSpeedMod;
    public float groundPoundMod;
    public float dashMod;

    public float sprintTime;
    public float timer;
    public bool GPCooldown;

    public bool landed = false;
    public bool priority = true;

    private Transform tr;
    private Rigidbody rb;
    private Vector3 move;

    public bool dirgetter = true;
    private float rotY;
    private float eightdirmod = 0;

    public float xcap;
    public float zcap;

    public bool colliding = false;

    public float walljumpmod;
    public float vertwalljumpmod;

    private Vector3 lastspeed;
    private float degreeturn = 0;
    private Collider collidedCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown (KeyCode.Space) && colliding && !landed)
        {
            RaycastHit hit;
            if (Physics.Raycast(tr.position, lastspeed, out hit))
            {
                if (hit.collider == collidedCollider)
                {
                    degreeturn = Mathf.Acos(1 / hit.distance);
                }
            }
            float newx = lastspeed.x * Mathf.Cos(degreeturn) - lastspeed.z * Mathf.Sin(degreeturn);
            float newz = lastspeed.x * Mathf.Sin(degreeturn) + lastspeed.z * Mathf.Cos(degreeturn);
            if (!Physics.Raycast(tr.position, new Vector3 (newx, 0, newz), out hit, 1.0001f))
            {
                newx = lastspeed.x * Mathf.Cos(-degreeturn) - lastspeed.z * Mathf.Sin(-degreeturn);
                newz = lastspeed.x * Mathf.Sin(-degreeturn) + lastspeed.z * Mathf.Cos(-degreeturn);
            }
            Vector3 walljump = new Vector3(newx, -vertwalljumpmod, newz) * -walljumpmod;
            rb.AddForce(walljump);
            degreeturn = 0;
        }
        if (Input.GetKey(KeyCode.LeftControl) && priority)
        {
            if (dirgetter)
            {
                Direction();
                dirgetter = false;
            }
            Ctrl(rotY);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                priority = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) || timer != 0)
        {
            if (timer < sprintTime)
            {
                timer += Time.deltaTime;
                if (dirgetter)
                {
                    Direction();
                    dirgetter = false;
                }
                Dash(rotY);
            }
            else
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0);

                timer = 0;
                dirgetter = true;
            }

            if (landed && Input.GetKeyDown(KeyCode.Space))
            {
                timer = 0;
                dirgetter = true;
            }
        }
        else
        {
            Move();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            priority = true;
            dirgetter = true;
        }
    }


    void Move ()
    {
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (move.magnitude > 1)
        {
            move = move.normalized;
        }
        if (landed)
        {
            rb.velocity = tr.TransformDirection(move.x * speedMod, rb.velocity.y, move.z * speedMod);
        }
        else
        {
            if (Mathf.Abs (rb.velocity.x) < xcap || rb.velocity.x * move.x < 0)
            {
                rb.velocity += tr.TransformDirection(move.x, 0, 0) * airSpeedMod;
            }
            if (Mathf.Abs(rb.velocity.z) < zcap || rb.velocity.z * move.z < 0)
            {
                rb.velocity += tr.TransformDirection(0, 0, move.z) * airSpeedMod;
            }
        }
    
    }
    void Dash (float rotY)
    {
        rb.velocity = new Vector3(Mathf.Sin(rotY), 0, Mathf.Cos(rotY)) * dashMod;
    }

    void Ctrl (float rotY)
    {
        if (landed)
        {
            rb.velocity = new Vector3(Mathf.Sin(rotY), 0, Mathf.Cos(rotY)) * slideMod;
        }
        else if (!GPCooldown)
        {
            rb.velocity = new Vector3(0, -groundPoundMod, 0);
            priority = false;
        }
    }

    void Direction ()
    {
        rotY = transform.rotation.eulerAngles.y * Mathf.PI / 180;
        if ((Input.GetAxisRaw("Horizontal")) == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            eightdirmod = 0;
        }
        else
        {
            eightdirmod = Mathf.Atan(Input.GetAxisRaw("Horizontal") / Input.GetAxisRaw("Vertical"));
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                eightdirmod += Mathf.PI;
            }
        }
        rotY += eightdirmod;
    }

    void Jump ()
    {
        if (landed)
        {
            rb.AddForce(Vector3.up * jumpMod);
            landed = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Jumpable")
        {
            landed = true;
            GPCooldown = true;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            collidedCollider = collision.collider;
            lastspeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).normalized;
            colliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Jumpable")
        {
            GPCooldown = false;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            colliding = false;
        }
    }
}