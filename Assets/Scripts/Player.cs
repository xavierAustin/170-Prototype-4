using UnityEngine;

public class Player : MonoBehaviour {
    //public
    public GameObject pCamera;
    public float gravity = 2.0f;
    //private
    string state = "default";
    bool[] input = new bool[7];
    Vector3 vel = new Vector3(0,0,0); //less verbose rigidbody.linearVelocity
    Rigidbody pRB = null;
    float t = 0; //range from 0 to 1; for general purpose use

    void Start() {
        pRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        vel = pRB.linearVelocity;
        
        input[0] = (Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("i"));
        input[1] = (Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("j"));
        input[2] = (Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("k"));
        input[3] = (Input.GetKey("d") || Input.GetKey("right") || Input.GetKey("l"));

        input[4] = (Input.GetKey("space") || Input.GetKey("enter"));
        input[5] = (Input.GetKeyUp("space") || Input.GetKeyUp("enter"));

        input[6] = Input.GetMouseButton(0);

        switch(state){
            default:
                Debug.Log($"No such state named \"{state}\". Assuming default state was intended.");
                state = "default";
            goto case "default"; //c# is actually such a stupid language
            case ("default"):
                if (input[1] ^ input[3])
                    state = "strafe";
            break;
            case ("strafe"):
                if (!(input[1] ^ input[3]))
                    state = "default";
                vel = (input[3] ? 1 : -1) * transform.right;
            break;
            case ("falling"):
                vel += Vector3.down * gravity;
            break;
        }

        pRB.linearVelocity = vel;
    }
}