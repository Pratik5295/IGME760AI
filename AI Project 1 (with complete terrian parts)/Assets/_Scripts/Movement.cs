using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public float m_Speed;
    public float m_Rotatespeed;
    public float m_JumpSpeed;
    Rigidbody m_Rigidbody;
    private bool grounded = true;
    // Use this for initialization
    void Start()
    {
        //Fetch the Rigidbody component you attach from your GameObject
        m_Rigidbody = GetComponent<Rigidbody>();
        //Set the speed of the GameObject
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
         {
             //Move forward
             m_Rigidbody.transform.Translate(new Vector3(0, 0, 1) * m_Speed * Time.deltaTime);
         }

         if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
         {
             //Move backward
             m_Rigidbody.transform.Translate(new Vector3(0, 0, -1) * m_Speed * Time.deltaTime);
         }

         if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
         {
             //Rotate right
             transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * m_Rotatespeed, Space.World);
         }

         if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
         {
             //Rotate left
             transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * m_Rotatespeed, Space.World);
         }
         */
      BasicMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Jump
            Jump();
        }
    }

    private void BasicMovement()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        movement.x = Input.GetAxis("Horizontal") * Time.deltaTime * m_Speed;
        movement.z = Input.GetAxis("Vertical") * Time.deltaTime * m_Speed;

        this.transform.Translate(movement);
    }

    private void Jump()
    {
        if (this.grounded)
        {
            m_Rigidbody.velocity = new Vector3(0, 10f, 0);
            grounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       // m_Rigidbody.velocity = new Vector3(0, 0, 0);
        grounded = true;
    }
}

