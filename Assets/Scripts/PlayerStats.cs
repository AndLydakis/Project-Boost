using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public int Lives { get; set; }
    public float Time { get; set; }
    public bool PermaDeath { get; set; }


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            print("Initializing singleton");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}