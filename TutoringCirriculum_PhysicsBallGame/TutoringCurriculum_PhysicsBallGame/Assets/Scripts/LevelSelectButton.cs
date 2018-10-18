using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectButton : MonoBehaviour {

    public int levelId;
    public LevelData levelData;
    public GameObject LevelAssetParent;
    public Image levelImage;
    public TextMeshProUGUI levelNameText;
    public TextMeshProUGUI bestTimeText;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI crystalsText;

    private void OnEnable() {
        GetComponent<Button>().interactable = levelData.unlocked;
        gemsText.text = "Gems: " + levelData.gemsCollected + " / 30";
        crystalsText.text = "Crystals: " + levelData.crystalsCollected + " / 3";
        if (LevelAssetParent) {
            LevelAssetParent.SetActive(false);
        }
    }

    private void Start() {
        GetComponent<Button>().interactable = levelData.unlocked;
    }

    private void OnValidate() {

        levelId = transform.GetSiblingIndex();

        if (levelData) {
            levelData.levelId = levelId;
            GetComponent<Button>().interactable = levelData.unlocked;
            if (levelNameText) {
                levelNameText.text = levelData.name;
            }
            if (levelImage) {
                levelImage.sprite = levelData.levelSprite;
            }
        }

    }

    public void LoadLevel() {

        LevelAssetParent.SetActive(true);
        foreach(Pickup p in LevelAssetParent.GetComponentsInChildren<Pickup>(true)) {
            p.gameObject.SetActive(true);
        }
        GameController.inst.currentLevel = levelData;
        GameController.inst.StartLevel();

    }



}
