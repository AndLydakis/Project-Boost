using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameInputController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text nameText;
    [SerializeField] int maxNameLen = 3;
    string playerName = "AAA";
    void Start() {

    }

    void setName() {
        if (playerName == "") playerName = "AAA";
        PlayerStats.Instance.Name = playerName;
        SortedList<float, string> score_list = new SortedList<float, string>();
        string scores = PlayerPrefs.GetString("scores", "");
        print("Loaded scores: " + scores);
        if (scores.Length > 0) {
            print("Adding new Score");
            scores += "," + playerName + "_" + PlayerStats.Instance.Time + ",";
        }
        else {
            PlayerPrefs.SetString("scores", playerName + "_" + PlayerStats.Instance.Time + ",");
            scores = PlayerPrefs.GetString("scores", "");
            print("Set first score: " + scores);
        }
        string scoresText = "";
        if (scores.Length > 0) {
            string[] name_score_pairs = scores.Split(',');
            print("Setting Scores:");
            foreach (string score_pair in name_score_pairs) {
                print(score_pair);
            }
            print("----------");
            if (name_score_pairs.Length > 5) {
                foreach (string pair in name_score_pairs) {
                    if (pair == "") continue;
                    string[] pair_split = pair.Split('_');
                    print(pair_split);
                    if (score_list.ContainsKey(float.Parse(pair_split[1]))) {
                        score_list[float.Parse(pair_split[1])] = pair_split[0];
                    }
                    else {
                        score_list.Add(float.Parse(pair_split[1]), pair_split[0]);
                    }

                }
                int cnt = 0;
                foreach (var pair in score_list) {
                    scoresText += pair.Value + "_" + pair.Key + ",";
                    cnt += 1;
                    if (cnt == 5) break;
                }
            }
            else {
                scoresText = scores;
            }
        }
        print("Saving score text: " + scoresText);
        scoresText = scoresText.Remove(scoresText.Length - 1);
        PlayerPrefs.SetString("scores", scoresText);

        SceneManager.LoadScene("HighScores");
    }
    // Update is called once per frame
    void Update() {
        nameText.text = playerName;
        if (Input.GetKeyDown(KeyCode.Return)) setName();
        if (Input.GetKeyDown(KeyCode.Underscore)) return;
        else if (Input.GetKeyDown(KeyCode.Backspace)) {
            if (playerName.Length > 0) playerName = playerName.Remove(playerName.Length - 1);
        }
        else {
            string in_ = Input.inputString;
            if (playerName.Length == maxNameLen) return;
            if (in_.Length > 0) in_ = in_.Substring(in_.Length - 1, in_.Length);
            playerName += in_;
        }
    }
}
