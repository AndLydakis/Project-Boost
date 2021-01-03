using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Rotator : MonoBehaviour
{
    [SerializeField] bool Clockwise = true;
    [SerializeField] float period = 2.0f;
    // Start is called before the first frame update
    Vector3 dir = Vector3.forward;
    void Start() {

        if (!Clockwise) dir = -Vector3.forward;
    }

    // Update is called once per frame
    void Update() {
        if (period <= Mathf.Epsilon) return;
        transform.Rotate(dir * (period * Time.deltaTime));
    }
}