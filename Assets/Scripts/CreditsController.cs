using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text creditsText;
    void Start() {
        creditsText.text = ("Congratulations!\nYou beat Project Boost!\nYour time was " + PlayerStats.Instance.Time + "!\nThanks for Playing!\nPress Enter to Continue");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
