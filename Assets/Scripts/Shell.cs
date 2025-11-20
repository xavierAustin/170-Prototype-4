using UnityEngine;

public class Shell : Pickup
{
    public int shellLevel;

    void Update()
    {
        if (!isHeld)
            return;
        Player p = GameObject.FindGameObjectWithTag("PlayerGrab").transform.parent.GetComponent<Player>();
        p.shellLevel = shellLevel;
        Destroy(gameObject);
        //do like an animation or somn
    }
}
