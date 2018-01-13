using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour {

    float moveHor;
    float moveVer;
    Rigidbody rb;

    public int speed;

	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	void Update () {

	}

    void FixedUpdate()
    {
        moveHor = Input.GetAxis("Horizontal");
        moveVer = Input.GetAxis("Vertical");
        rb.AddForce(new Vector3(moveHor, 0, moveVer) * speed);
    }

}
