using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallStation : MonoBehaviour {

    public bool active;
    ParticleSystem partSystem;

    private void Start() {

        partSystem = GetComponent<ParticleSystem>();
        partSystem.Stop();

    }

    private void Update() {
        
        if (active) {

            if (Input.GetKeyDown(KeyCode.D)) {

                GameController.player.CycleBall(true);

            }

            if (Input.GetKeyDown(KeyCode.A)) {

                GameController.player.CycleBall(false);

            }

            if (Input.GetKeyDown(KeyCode.Space)) {

                SelectedBall();

            }

        }

    }

    private void FixedUpdate() {
        
        if (active) {

            GameController.player.rb.velocity = ((transform.position + Vector3.up) - GameController.player.transform.position) * 3f;

        }

    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.transform.CompareTag("Player")) {

            if (!active) {
                active = true;
                partSystem.Play();
            }

        }

    }

    public void SelectedBall() {

        active = false;
        partSystem.Stop();

    }





}
