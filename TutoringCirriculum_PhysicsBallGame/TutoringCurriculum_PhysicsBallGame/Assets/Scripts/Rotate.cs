using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public Vector3 spinSpeed;
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {

        if (rb) rb.angularVelocity = spinSpeed;
        else transform.Rotate(spinSpeed * Time.deltaTime * 60, Space.World);

    }



}
