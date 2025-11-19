using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isHeld = false;
    Rigidbody rb;
    Transform grabPoint;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Grab()
    {
        // grabPoint = the Player's GrabTrigger object
        grabPoint = GameObject.FindGameObjectWithTag("PlayerGrab").transform;

        isHeld = true;
        rb.isKinematic = true;

        transform.SetParent(grabPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isHeld = false;

        transform.SetParent(null);
        rb.isKinematic = false;
    }
}
