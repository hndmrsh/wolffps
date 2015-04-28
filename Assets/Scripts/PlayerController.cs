using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [Range(1, 100)]
    public float speed = 10f;

    [Range(50, 1000)]
    public float rotateSpeed = 30f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = (Input.GetAxis("Horizontal") > 0.99f ? 1f : (Input.GetAxis("Horizontal") < -0.99f ? -1f : 0f));
        float v = (Input.GetAxis("Vertical") > 0.99f ? 1f : (Input.GetAxis("Vertical") < -0.99f ? -1f : 0f));

        transform.Rotate(0, h * rotateSpeed * Time.deltaTime, 0);
        
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = speed * Time.deltaTime * v;
        controller.Move(forward * curSpeed);
    }

}
