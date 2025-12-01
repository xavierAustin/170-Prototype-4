using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject buttonFace;

    bool pressed = false;

    void OnTriggerEnter(Collider other){
        if (buttonFace && !pressed)
            buttonFace.transform.localScale -= Vector3.up * 0.15f;
        pressed = true;
        //pressed = pressed || (!other.GetComponent<Enemy>() && !other.GetComponent<Shell>() && (other.GetComponent<Pickup>() || other.GetComponent<Player>()));
    }

    void OnTriggerExit(Collider other){
        if (buttonFace && pressed)
            buttonFace.transform.localScale += Vector3.up * 0.15f;
        pressed = false;
    }
}
