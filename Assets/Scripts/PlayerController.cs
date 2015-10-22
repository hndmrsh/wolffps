using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    [Range(1, 100)]
    public float speed = 10f;

    [Range(50, 1000)]
    public float rotateSpeed = 30f;

    public float openRange = 1f;

    private LayerMask doorLayerMask = 9;

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

        if (Input.GetButton("Open"))
        {
            TryOpenDoor();
        }
    }

    private void TryOpenDoor()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        Debug.DrawRay(transform.position, transform.forward, Color.red, openRange);

        if (Physics.Raycast(ray, out hit, openRange, ~doorLayerMask.value))
        {
            Door door = hit.transform.GetComponentInChildren<Door>();
            if (door != null) 
            {
                door.OpenClose();
            }
        }
    }

}
