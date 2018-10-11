using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Public variables
    public Transform cameraOrigin;
    public Transform playerCam;
    public float playerSpeed = 1f;

    //Private variables
    Rigidbody rb;


    private void Start() {

        //Set cursor to stay locked to centor of screen, so we can rotate the camera with the Mouse
        Cursor.lockState = CursorLockMode.Locked;

        //Get rigidbody component on player
        rb = GetComponent<Rigidbody>();

    }

    private void Update() {

            //CAMERA 

        //Set origin of camera to position of player
        cameraOrigin.transform.position = transform.position + Vector3.up;

        //Move camera with mouse
        cameraOrigin.Rotate(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);

        //Reset rotation of Z axis to zero
        cameraOrigin.eulerAngles = new Vector3(cameraOrigin.eulerAngles.x, cameraOrigin.eulerAngles.y, 0);

        //Raycast from player to camera to check for walls, and set the camera distance, so the camera doesnt clip thru walls
        float camDistance = -10;
        RaycastHit hit;
        if (Physics.Raycast(cameraOrigin.position, -cameraOrigin.forward, out hit, 11f, 1<<0)) {
            if (hit.distance > 1) camDistance = -hit.distance + .5f;
        }

        //Lerp camera to distance position
        float lerpSpeed = 25f;
        if (camDistance < playerCam.localPosition.z) lerpSpeed = 2f;
        playerCam.localPosition = Vector3.Lerp(playerCam.localPosition, new Vector3(0, 0, camDistance), Time.deltaTime * lerpSpeed);


        //PLAYER MOVEMENT
        
        //Normalize Camera facing vectors, so that the X rotation of the Camera doesn't affect forward or sideways movement
        Vector3 camForward = new Vector3(playerCam.forward.x, 0, playerCam.forward.z).normalized;
        Vector3 camRight = new Vector3(playerCam.right.x, 0, playerCam.right.z).normalized;

        //If W or S key is pressed, add velocity to the rigidbody in the direction the camera is facing (input is -1 to 1)
        rb.velocity += camForward * Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;

        //If A or D key is pressed, add velocity to the rigidbody in the direction to the right of the camera (input is -1 to 1)
        rb.velocity += camRight * Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;


    }


}
