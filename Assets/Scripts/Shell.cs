using UnityEngine;

public class Shell : Pickup
{
    public int shellLevel;

    void Update()
    {
        if (!isHeld)
            return;
        Player p = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log(p);
        p.shellLevel = shellLevel;
        Destroy(gameObject);
        //do like an animation or somn
    }
}
