using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingScript : MonoBehaviour {

    PostProcessProfile profile;

    private void Start() {

        profile = GetComponent<PostProcessVolume>().profile;

    }

    private void Update() {

        DepthOfField dof;
        profile.TryGetSettings(out dof);

        if (dof) {
            dof.focusDistance.value = -GameController.player.playerCam.transform.localPosition.z + .75f;
            profile.isDirty = true;
        }

    }



}
