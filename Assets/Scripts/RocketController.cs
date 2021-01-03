using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rocket;
    AudioSource thrusterAudio;

    [SerializeField] float rotationVel = 250.0f;
    [SerializeField] float thrustVel = 50.0f;
    [SerializeField] float mass = 0.1f;
    [SerializeField] float levelLoadDelay = 0.5f;
    [SerializeField] bool debug = true;
    [SerializeField] bool collisionsOn = true;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelComplete;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    void Start() {
        rocket = GetComponent<Rigidbody>();
        rocket.mass = mass;
        thrusterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (Debug.isDebugBuild) CheckDebugInputs();

        if (state != State.Alive) {
            return;
        }
        bool thrusterOn = Thrust() | Rotate();
        if (thrusterOn && !thrusterAudio.isPlaying) {
            print("Starting Engine Audio and Particles");
            thrusterAudio.PlayOneShot(mainEngine);
            mainEngineParticles.Play();

        }
        else if (!thrusterOn && thrusterAudio.isPlaying) {
            print("Stopping Engine Audio and Particles");
            thrusterAudio.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void LoadFirstLevel() {
        if (PlayerStats.Instance.PermaDeath) {
            SceneManager.LoadScene(0);
            return;
        }

        if ((--PlayerStats.Instance.Lives) <= 0) {
            SceneManager.LoadScene(0);
            return;
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
    }

    private void LoadNextLeve() {
        int y = SceneManager.GetActiveScene().buildIndex; ;
        SceneManager.LoadScene(y + 1);
    }

    private void CheckDebugInputs() {
        if (Input.GetKeyDown(KeyCode.C)) collisionsOn = !collisionsOn;
        else if (Input.GetKeyDown(KeyCode.L)) {
            HandleLevelEnd();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (!collisionsOn) return;
        if (state != State.Alive) {
            return;
        }
        switch (collision.gameObject.tag) {
            case "Death":
                print("Collided with obstacle, game over");
                HandleDeath();
                break;
            case "Friendly":
                break;
            case "Landing":
                HandleLevelEnd();
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

    private void HandleDeath() {
        if (!debug) {
            print("Handle Death");
            state = State.Dying;
            thrusterAudio.Stop();
            thrusterAudio.PlayOneShot(death);
            deathParticles.Play();
            Invoke("LoadFirstLevel", levelLoadDelay);
        }
    }

    private void HandleLevelEnd() {
        print("Landing Pad Reached");
        thrusterAudio.Stop();
        thrusterAudio.PlayOneShot(levelComplete);
        successParticles.Play();
        state = State.Transcending;
        Invoke("LoadNextLeve", levelLoadDelay); //TODO parametrize level loading
    }
}
