using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBallData", menuName = "New Ball Data")]
public class BallData : ScriptableObject {

    public enum Type { Basic, Metal, Wooden, Bucky, Atom, Wheel, Spike, Bomb }

    public GameObject ballPrefab;
    public Type type;
    [TextArea(2,5)]
    public string description;





}
