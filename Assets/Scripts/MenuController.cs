using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int numLives = 5;
    [SerializeField] bool permaDeath = true;

    [SerializeField] Text newGameText;
    [SerializeField] Text livesText;
    [SerializeField] Text permaText;

    PlayerStats playerStats = new PlayerStats();

    enum State { NewGame, Lives, Perma };
    State curState = State.NewGame;

    Vector3 livesPos;
    Vector3 permaPos;
    void Start() {
        livesPos = livesText.transform.position;
        permaPos = permaText.transform.position;
        newGameText.text = "* New Game";
    }

    void HandleLives() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            newGameText.text = "* New Game";
            livesText.text = " " + livesText.text.Substring(1);
            curState = State.NewGame;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            permaText.text = "*  Permadeath: " + (permaDeath ? "[ON]" : "[OFF]");
            livesText.text = " " + livesText.text.Substring(1);
            curState = State.Perma;
            return;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            numLives = Mathf.Max(numLives - 1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            numLives = Mathf.Min(numLives + 1, 10);
        }
        livesText.text = "*  Lives: [" + numLives + "]";
        livesText.transform.position = livesPos;
    }

    void HandlePermaDeath() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            permaText.text = "  Permadeath: " + (permaDeath ? "[ON]" : "[OFF]");
            livesText.text = "*  Lives: [" + numLives + "]";
            curState = State.Lives;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            permaText.text = "  Permadeath: " + (permaDeath ? "[ON]" : "[OFF]");
            newGameText.text = "* New Game";
            curState = State.NewGame;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            permaDeath = !permaDeath;
            permaText.text = ("* Permadeath: " + (permaDeath ? "[ON]" : "[OFF]")).Replace("\n", ""); ;
            permaText.transform.position = permaPos;
        }
    }

    void HandleNewGame() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            newGameText.text = "  New Game";
            permaText.text = "* Permadeath: " + (permaDeath ? "[ON]" : "[OFF]");
            curState = State.Perma;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            newGameText.text = "  New Game";
            livesText.text = "*  Lives: [" + numLives + "]";
            curState = State.Lives;
        }
        else if (Input.GetKeyDown(KeyCode.Return)) {
            //Store Score
            PlayerStats.Instance.Time = 0f;
            PlayerStats.Instance.Lives = numLives;
            PlayerStats.Instance.PermaDeath = permaDeath;
            SceneManager.LoadScene(1);
        }
    }


    // Update is called once per frame
    void Update() {
        switch (curState) {
            case State.NewGame:
                HandleNewGame();
                break;
            case State.Perma:
                HandlePermaDeath();
                break;
            case State.Lives:
                HandleLives();
                break;
        }

    }
}
