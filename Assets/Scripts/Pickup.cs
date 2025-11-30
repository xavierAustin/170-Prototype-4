using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isHeld = false;
    public bool isHeavy = false;
    protected Rigidbody rb;
    protected Transform grabPoint;
    protected int previousLayer;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        previousLayer = gameObject.layer;
    }

    public void Grab()
    {
        // grabPoint = the Player's GrabTrigger object
        grabPoint = GameObject.FindGameObjectWithTag("PlayerGrab").transform;

        isHeld = true;
        if (rb)
            rb.isKinematic = true;
        if (isHeavy)
            return;

        transform.SetParent(grabPoint);
        transform.localPosition = Vector3.forward * transform.localScale.z;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isHeld = false;

        transform.SetParent(null);
        if (rb)
            rb.isKinematic = false;
    }

    public void SetOutline(bool value){
        gameObject.layer = value ? LayerMask.NameToLayer("Outline") : previousLayer;
    }
}
