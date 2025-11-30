using UnityEngine;
using System.Collections;

public class Shell : Pickup
{
    public int shellLevel;

    void Awake()
    {
        isHeavy = true;
    }

    void Update()
    {
        if (!isHeld)
            return;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().shellLevel = shellLevel;
        if (rb)
            rb.isKinematic = true;
        Destroy(gameObject);
    }
}
