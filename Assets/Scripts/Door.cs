using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private const float TIME_TO_OPEN = 0.8f;

    private bool doorOpen = false;
    private bool animating = false;

    private Vector3 initialPos, targetPos;
    private float animatingTime = 0f;

    // Use this for initialization
    void Start()
    {
        initialPos = transform.localPosition;
        targetPos = transform.localPosition + (Vector3.left * 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (animating)
        {
            Vector3 startPos = doorOpen ? targetPos : initialPos;
            Vector3 endPos = doorOpen ? initialPos : targetPos;

            animatingTime += Time.deltaTime;
            if (animatingTime >= TIME_TO_OPEN)
            {
                animating = false;
                doorOpen = !doorOpen;
            }

            transform.localPosition = Vector3.Lerp(startPos, endPos, animatingTime / TIME_TO_OPEN);
        }
    }


    public void OpenClose()
    {
        if (!animating)
        {
            Debug.Log((doorOpen ? "Close " : "Open ") + "door");
            animating = true;
            animatingTime = 0f;
        }
    }
}
