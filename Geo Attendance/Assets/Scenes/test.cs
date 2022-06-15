using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Rigidbody rb;
    public float jumpForce;
    void Start()
    {
      rb=GetComponent<Rigidbody>();
    }

    void Update()
    {
       if(Input.GetMouseButtonDown(0)){
        rb.AddForce(transform.up*jumpForce  ,ForceMode.Impulse);
       }
    }

}
