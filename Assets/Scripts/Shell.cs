using UnityEngine;

public class Shell : Pickup
{
    public int shellLevel;

    public new void Grab()
    {
        Player p = GameObject.FindGameObjectWithTag("PlayerGrab").transform.parent.GetComponent<Player>();
        p.shellLevel = shellLevel;
        Destroy(gameObject);
        //do like an animation or somn
    }
}
