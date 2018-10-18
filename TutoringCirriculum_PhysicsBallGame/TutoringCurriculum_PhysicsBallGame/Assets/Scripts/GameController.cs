using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour {

    public static GameController inst;
    public static Player player;
    public bool deletePlayerPrefsOnRun;
    public int gemsCollected;
    public GameObject mainMenu;
    public Transform spawnPoint;
    public List<LevelData> levels = new List<LevelData>();
    public LevelData currentLevel;
    public List<BallData> ballsUnlocked = new List<BallData>();
    public TextMeshProUGUI gemsCollectedText;
    public TextMeshProUGUI instructionsText;
    float instructionsTextTimer;

    private void Awake() {

        if (deletePlayerPrefsOnRun) PlayerPrefs.DeleteAll();

        inst = this;
        player = FindObjectOfType<Player>();
        player.gameObject.SetActive(false);
        gemsCollectedText.gameObject.SetActive(false);
        instructionsText.gameObject.SetActive(false);

        foreach (LevelSelectButton lev in mainMenu.transform.GetComponentsInChildren<LevelSelectButton>()) {
            if (lev.levelData) {
                levels.Add(lev.levelData);
                lev.levelData.gemsCollected = 0;
                if (lev.levelData.levelId != 0) lev.levelData.unlocked = false;
                if (PlayerPrefs.GetInt("levelUnlocked" + lev.levelData.name, 0) == 1) lev.levelData.unlocked = true;
            }
        }

    }

    private void Update() {

        //DEBUG CHEAT
        if (Input.GetKeyDown(KeyCode.Insert)) gemsCollected = 30;

        if (instructionsTextTimer > 0) {
            instructionsTextTimer -= Time.deltaTime;
            if (instructionsTextTimer <= 0) instructionsText.gameObject.SetActive(false);
        }

        //Player fall off level
        if (currentLevel) {

            if (player.transform.position.y < currentLevel.killY) {
                FinishLevel(true);
            }

        }
    }

    public void StartLevel() {

        player.gameObject.SetActive(true);
        player.rb.isKinematic = false;
        player.rb.velocity = Vector3.zero;
        player.transform.position = spawnPoint.position;
        mainMenu.SetActive(false);
        gemsCollected = 0;
        gemsCollectedText.text = "0 / 30";
        gemsCollectedText.gameObject.SetActive(true);
        instructionsText.gameObject.SetActive(true);
        instructionsTextTimer = 5;

        //Set cursor to stay locked to centor of screen, so we can rotate the camera with the Mouse
        Cursor.lockState = CursorLockMode.Locked;

        var i = 0;
        foreach (Pickup p in FindObjectsOfType<Pickup>()) {
            if (p.type == Pickup.Type.Gem) {
                p.transform.Rotate(0, i * 30, 0);
                i++;
            }
        }

    }

    public void FinishLevel(bool died = false) {

        if (!died) {
            currentLevel.gemsCollected = gemsCollected;
            if (gemsCollected >= 22) {
                if (levels.Count > currentLevel.levelId + 1) {
                    levels[currentLevel.levelId + 1].unlocked = true;
                    PlayerPrefs.SetInt("levelUnlocked" + levels[currentLevel.levelId + 1].name, 1);
                }
            }
        }
        currentLevel = null;
        mainMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gemsCollectedText.gameObject.SetActive(false);

    }

    public void PickUpGem() {

        gemsCollected++;
        gemsCollectedText.text = gemsCollected + " / 30";

    }


    public static float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

}
