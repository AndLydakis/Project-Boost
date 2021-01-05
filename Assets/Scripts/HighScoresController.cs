using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScoresController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text scoreText;
    string title = "HighScores\n";
    string sep = "------------\n";
    string back = "Press Enter\nTo Return To Main Menu";

    void Start() {
        print("Formatting highscores");
        SortedList<float, string> score_list = new SortedList<float, string>();
        scoreText.text = title;
        scoreText.text += sep;
        string scores = PlayerPrefs.GetString("scores", "");
        print("Found Scores: " + scores);
        if (scores.Length > 0) {
            string[] name_score_pairs = scores.Split(',');

            foreach (string pair in name_score_pairs) {
                string[] pair_split = pair.Split('_');
                print("Pair Split: " + pair_split);
                if (score_list.ContainsKey(float.Parse(pair_split[1]))) {
                    score_list[float.Parse(pair_split[1])] = pair_split[0];
                }
                else {
                    score_list.Add(float.Parse(pair_split[1]), pair_split[0]);
                }

            }
            int cnt = 1;
            foreach (var pair in score_list) {
                scoreText.text += cnt + ". " + pair.Value + " " + pair.Key + "\n";
                cnt += 1;
            }
        }
        scoreText.text += sep;
        scoreText.text += back;

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(0);
    }
}
