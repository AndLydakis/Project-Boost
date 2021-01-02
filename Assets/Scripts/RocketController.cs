using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start() {
        rocket = GetComponent<Rigidbody>();
        rocket.mass = mass;
        thrusterAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        bool thrusterOn = Thrust() | Rotate();
        if (thrusterOn && !thrusterAudio.isPlaying) {
            thrusterAudio.Play();

        }
        else if (!thrusterOn && thrusterAudio.isPlaying) {
            thrusterAudio.Stop();
        }
    }

    void OnCollisionEnter(Collision collision) {
        switch (collision.gameObject.tag) {
            case "Death":
                print("Collided with obstacle, game over");
                break;
            case "Friendly":
                break;
            case "Fuel":
                print("Fuel picked up");
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
