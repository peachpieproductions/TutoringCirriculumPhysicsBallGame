using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour {

    public GameObject particleBurst;
    public AudioClip pickupSound;
    bool finished;
    float finishTimer = 3;

    private void OnEnable() {
        finishTimer = 3;
    }

    private void Update() {

        if (finished) {
            transform.Rotate(0, 10, 0);
            if (finishTimer > 0) finishTimer -= Time.deltaTime;
            else {
                GameController.inst.FinishLevel();
                finished = false;
            }
        }
        else transform.Rotate(0, 1, 0);

    }

    private void OnTriggerEnter(Collider other) {

        if (other.transform.CompareTag("Player")) {

            GameController.player.PlayOneShotAudio(pickupSound);
            finished = true;
            Instantiate(particleBurst, transform.position + Vector3.up * .4f, Quaternion.identity);
            GameController.player.rb.isKinematic = true;

        }

    }
}
