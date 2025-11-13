using UnityEngine;

public class Player : MonoBehaviour {
    //public
    public GameObject pCamera;
    public float gravity = 2.0f;
    public float fbSpd = 0.7f;
    public float lrSpd = 2.3f;
    //private
    string state = "default";
    bool[] input = new bool[6];
    Vector3 vel = new Vector3(0,0,0); //less verbose rigidbody.linearVelocity
    Rigidbody pRB = null;
    float t = 0.0f; //range from 0 to 1; for general purpose use in easing functions (eg: lerp)
    float cameraLocalRotY = 0.0f;
    float cameraLocalRotX = 0.0f;

    void Start() {
        pRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        t = Mathf.Clamp(t, 0.0f, 1.0f);
        vel = pRB.linearVelocity;
        
        PollInputs();

        switch(state){
            case ("default"):
                vel = Vector3.zero;
                DoCameraLookFixed();
            break;
            case ("move"):
                vel = ((input[0] ? transform.forward : Vector3.zero) - (input[2] ? transform.forward : Vector3.zero)) * fbSpd 
                    + ((input[3] ? transform.right : Vector3.zero) - (input[1] ? transform.right : Vector3.zero)) * lrSpd;
                DoCameraLookFixed();
            break;
            case ("fall"):
                vel += Vector3.down * gravity;
            break;
        }

        pRB.linearVelocity = vel;
    }

    void Update(){
        PollInputs();
        
        switch(state){
            default:
                Debug.Log($"No such state named \"{state}\". Assuming default state was intended.");
                state = "default";
            goto case "default"; //c# is actually such a stupid language
            case ("default"):
                if ((input[1] ^ input[3]) || (input[0] ^ input[2]))
                    state = "move";
                DoCameraLook();
            break;
            case ("move"):
                if (!((input[1] ^ input[3]) || (input[0] ^ input[2])))
                    state = "default";
                DoCameraLook();
            break;
            case ("fall"):
            break;
        }
    }

    void DoCameraLook(){
        //axis of rotation rather than mouse position
        cameraLocalRotY = Mathf.Clamp(cameraLocalRotY + Input.mousePositionDelta.x, -40.0f, 40.0f);
        cameraLocalRotX = Mathf.Clamp(cameraLocalRotX - Input.mousePositionDelta.y, -40.0f, 40.0f);
        pCamera.transform.localEulerAngles = Vector3.up*cameraLocalRotY + Vector3.right*cameraLocalRotX;
    }

    void DoCameraLookFixed(){
        if (input[4])
            return;
        var angDiff = Mathf.Clamp(cameraLocalRotY, -1.0f, 1.0f);
        transform.eulerAngles = (transform.eulerAngles.y + angDiff) * Vector3.up;
        Debug.Log(angDiff);
        cameraLocalRotY -= angDiff;
        pCamera.transform.localEulerAngles = Vector3.up*cameraLocalRotY + Vector3.right*cameraLocalRotX;
    }

    void PollInputs(){
        input[0] = (Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("i"));
        input[1] = (Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("j"));
        input[2] = (Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("k"));
        input[3] = (Input.GetKey("d") || Input.GetKey("right") || Input.GetKey("l"));

        input[4] = (Input.GetKey("space") || Input.GetKey("enter"));

        input[5] = Input.GetMouseButton(0);
    }
}