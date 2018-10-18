using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Public variables
    public Transform cameraOrigin;
    public Transform playerCam;
    public float playerSpeed = 1f;
    public AudioSource oneShotAudioSource;
    public AudioSource ballRollingAudio;
    public AudioClip[] playerSounds;
    public Rigidbody rb;
    public GameObject currentBall;
    public BallData currentBallData;
    public int currentBallId;
    float ballSize = 1;
    bool canJump;
    bool canWallJump;
    bool touchingSurface;
    Vector3 wallJumpVector;


    private void Awake() {

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
            camDistance = -hit.distance + .5f;
        }

        //Lerp camera to distance position
        float lerpSpeed = 25f;
        if (camDistance < playerCam.localPosition.z) lerpSpeed = 1.5f;
        playerCam.localPosition = Vector3.Lerp(playerCam.localPosition, new Vector3(0, 0, camDistance), Time.deltaTime * lerpSpeed);


            //JUMP

        if (Physics.Raycast(transform.position, Vector3.down, out hit, .6f * ballSize)) {
            if (!canJump && !touchingSurface) PlayOneShotAudio(playerSounds[0], false);
            canJump = true;
        } else canJump = false;

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (canJump) {
                rb.velocity += Vector3.up * 5f;
                if (currentBallData.type == BallData.Type.Bucky) rb.velocity += Vector3.up * 3f;
                PlayOneShotAudio(playerSounds[1]);
            } else {
                if (canWallJump) {
                    rb.velocity += (wallJumpVector + Vector3.up).normalized * 5;
                    PlayOneShotAudio(playerSounds[1]);
                }
            }
        }

            //BALL AUDIO

        if (touchingSurface) {
            ballRollingAudio.volume = rb.velocity.magnitude * .1f;
            ballRollingAudio.pitch = rb.velocity.magnitude * .1f;
        } else {
            ballRollingAudio.volume = 0;
        }

            //ATOM BALL

        if (currentBallData.type == BallData.Type.Atom) {

            if (Input.GetKey(KeyCode.E)) {
                if (currentBall.transform.localScale.x < 2) currentBall.transform.localScale *= 1.02f;
                ballSize = currentBall.transform.localScale.x;
            }

            if (Input.GetKey(KeyCode.Q)) {
                if (currentBall.transform.localScale.x > .5f) currentBall.transform.localScale *= .98f;
                ballSize = currentBall.transform.localScale.x;
            }

        }

    }

    private void FixedUpdate() {

        //PLAYER MOVEMENT

        //Normalize Camera facing vectors, so that the X rotation of the Camera doesn't affect forward or sideways movement
        Vector3 camForward = new Vector3(playerCam.forward.x, 0, playerCam.forward.z).normalized;
        Vector3 camRight = new Vector3(playerCam.right.x, 0, playerCam.right.z).normalized;

        //Adjust speed if needed
        var spd = playerSpeed;
        if (currentBallData.type == BallData.Type.Wheel) spd *= 1.6f;

        //If W or S key is pressed, add velocity to the rigidbody in the direction the camera is facing (input is -1 to 1)
        rb.velocity += camForward * Input.GetAxis("Vertical") * spd * Time.deltaTime;

        //If A or D key is pressed, add velocity to the rigidbody in the direction to the right of the camera (input is -1 to 1)
        rb.velocity += camRight * Input.GetAxis("Horizontal") * spd * Time.deltaTime;

        //Control speed with Shift key
        if (Input.GetKey(KeyCode.LeftShift)) {
            var horizontalVel = rb.velocity;
            horizontalVel.y = 0;
            if (horizontalVel.magnitude > 2.5f) {
                horizontalVel = horizontalVel.normalized * 2.5f;
                rb.velocity = new Vector3(horizontalVel.x, rb.velocity.y, horizontalVel.z);
            }
        }

    }

    public void CycleBall(bool forward) {

        if (forward) {
            currentBallId++;
        } else {
            currentBallId--;
        }

        if (currentBallId == GameController.inst.ballsUnlocked.Count) currentBallId = 0;
        if (currentBallId < 0) currentBallId = GameController.inst.ballsUnlocked.Count - 1;
        Destroy(currentBall);
        currentBall = Instantiate(GameController.inst.ballsUnlocked[currentBallId].ballPrefab, transform.position, transform.rotation, transform);
        currentBallData = GameController.inst.ballsUnlocked[currentBallId];
        ballSize = 1;

    }

    public void PlayOneShotAudio(AudioClip audioClip, bool randomPitch = true) {

        if (randomPitch) oneShotAudioSource.pitch = Random.Range(.8f, 1.2f);
        else oneShotAudioSource.pitch = 1;
        oneShotAudioSource.PlayOneShot(audioClip);

    }

    private void OnCollisionStay(Collision collision) {
        touchingSurface = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (currentBallData.type == BallData.Type.Metal) {
            canWallJump = true;
            wallJumpVector = collision.contacts[0].normal;
        }
    }

    private void OnCollisionExit(Collision collision) {
        touchingSurface = false;
        canWallJump = false;
    }


}
