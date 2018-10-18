using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	public enum Type { Gem, Crystal }

    public Type type;
    public GameObject particleBurst;
    public AudioClip pickupSound;


    private void Update() {

        transform.GetChild(0).Rotate(0, 1, 0, Space.World);

    }

    private void OnTriggerEnter(Collider other) {
        
        if (other.transform.CompareTag("Player")) {

            GameController.player.PlayOneShotAudio(pickupSound);
            Instantiate(particleBurst, transform.position + Vector3.up * .4f, Quaternion.identity);
            if (type == Type.Gem) GameController.inst.PickUpGem();
            gameObject.SetActive(false);

        }

    }



}
