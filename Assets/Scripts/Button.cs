using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject buttonFace;
    public bool pressed = false;
    int objsEntered = 0;

    void OnTriggerEnter(Collider other){
        if (buttonFace && objsEntered == 0)
            buttonFace.transform.localScale -= Vector3.up * 0.15f;
        objsEntered ++;
        pressed = true;
        //pressed = pressed || (!other.GetComponent<Enemy>() && !other.GetComponent<Shell>() && (other.GetComponent<Pickup>() || other.GetComponent<Player>()));
    }

    void OnTriggerExit(Collider other){
        objsEntered --;
        if (buttonFace && objsEntered == 0)
            buttonFace.transform.localScale += Vector3.up * 0.15f;
        pressed = (objsEntered != 0);
    }
}
