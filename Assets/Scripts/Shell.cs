using UnityEngine;
using System.Collections;

public class Shell : Pickup
{
    public int shellHP;

    void Awake()
    {
        isHeavy = true;
    }

    void Update()
    {
        if (!isHeld)
            return;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().UpdateShellHP(shellHP);
        if (rb)
            rb.isKinematic = true;
        Destroy(gameObject);
    }
}
