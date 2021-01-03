using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesCounter : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text livesText;
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        PlayerStats.Instance.Time += Time.deltaTime;
        livesText.text = "Lives: " + PlayerStats.Instance.Lives + "\t Time: " + (PlayerStats.Instance.Time.ToString("000.00"));
    }
}
