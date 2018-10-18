using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "New Level Data")]
public class LevelData : ScriptableObject {

    public int levelId;
    public Sprite levelSprite;
    public bool unlocked;
    public int gemsCollected;
    public int crystalsCollected;
    public float killY;


    


}
