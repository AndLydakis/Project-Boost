using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody starboardBooster;
    Rigidbody portBooster;
    Rigidbody rocket;
    AudioSource thrusterAudio;

    [SerializeField] float rotationVel = 250.0f;
    [SerializeField] float thrustVel = 50.0f;
    [SerializeField] float mass = 0.1f;
    [SerializeField] bool debug = true;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    void Start() {
        rocket = GetComponent<Rigidbody>();
        rocket.mass = mass;
        thrusterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (state != State.Alive) {
            thrusterAudio.Stop();
            return;
        }
        bool thrusterOn = Thrust() | Rotate();
        if (thrusterOn && !thrusterAudio.isPlaying) {
            thrusterAudio.Play();

        }
        else if (!thrusterOn && thrusterAudio.isPlaying) {
            thrusterAudio.Stop();
        }
    }

    private void LoadFirstLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLeve() {
        int y = SceneManager.GetActiveScene().buildIndex; ;
        SceneManager.LoadScene(y + 1);
    }


    void OnCollisionEnter(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Death":
                print("Collided with obstacle, game over");
                if (!debug) {
                    state = State.Dying;
                    Invoke("LoadFirstLevel", 0.5f);
                }
                break;
            case "Friendly":
                break;
            case "Landing":
                print("Landing Pad Reached");
                state = State.Transcending;
                Invoke("LoadNextLeve", 0.5f); //TODO parametrize level loading
                break;
            default:
                break;
        }
    }

    private bool Rotate() {
        rocket.freezeRotation = true;
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * (float)(Time.deltaTime * rotationVel));
            return true;
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * (float)(Time.deltaTime * rotationVel));
            return true;
        }
        rocket.freezeRotation = false;
        return false;
    }

    private bool Thrust() {
        if (Input.GetKey(KeyCode.Space)) {
            rocket.AddRelativeForce(Vector3.up * (float)(Time.deltaTime * thrustVel));
            return true;
        }
        return false;
    }
}
