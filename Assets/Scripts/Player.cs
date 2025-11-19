using UnityEngine;

public class Player : MonoBehaviour {
    //public
    public GameObject pCamera;
    public float gravity = 2.0f;
    public float fbSpd = 0.4f;
    public float lrSpd = 2.3f;
    public float mouseSensitivity = 0.3f;
    //private
    string state = "default";
    enum I {
        forward,
        back,
        left,
        right,
        space,
        mb1,
        DONOTUSE
    }
    bool[] input = new bool[(int)I.DONOTUSE];
    Vector3 vel = new Vector3(0,0,0); //less verbose rigidbody.linearVelocity
    Rigidbody pRB = null;
    float tCameraRot = 0.0f; //range from 0 to 1
    float cameraLocalRotY = 0.0f;
    float cameraLocalRotX = 0.0f;
    Pickup currentPickup;

    void Start() {
        pRB = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        vel = pRB.linearVelocity;
        
        PollInputs();

        switch(state){
            case ("default"):
                vel = Vector3.zero;
                DoCameraLookFixed();
            break;
            case ("move"):
                vel = ((input[(int)I.right] ? transform.right : Vector3.zero) - (input[(int)I.left] ? transform.right : Vector3.zero)) * lrSpd;
                if (!input[(int)I.space])
                    vel += ((input[(int)I.forward] ? transform.forward : Vector3.zero) - (input[(int)I.back] ? transform.forward : Vector3.zero)) * fbSpd;
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
                if ((input[(int)I.left] ^ input[(int)I.right]) || ((input[(int)I.forward] ^ input[(int)I.back]) && !input[(int)I.space]))
                    state = "move";
                DoCameraLook();
            break;
            case ("move"):
                if (!((input[(int)I.left] ^ input[(int)I.right]) || ((input[(int)I.forward] ^ input[(int)I.back]) && !input[(int)I.space])))
                    state = "default";
                DoCameraLook();
            break;
            case ("fall"):
            break;
        }
        if (input[(int)I.mb1]) {
            TryGrab();
        }

    }
    void TryGrab(){
    if (currentPickup == null) return;

    if (!currentPickup.isHeld)
    {
        currentPickup.Grab();
    }
    else
    {
        currentPickup.Drop();
    }
    }


    void DoCameraLook(){
        var max = Mathf.Lerp(40.0f, 60.0f, tCameraRot);
        //axis of rotation rather than mouse position
        cameraLocalRotY = Mathf.Clamp(cameraLocalRotY + Input.mousePositionDelta.x * mouseSensitivity, -max, max);
        cameraLocalRotX = Mathf.Clamp(cameraLocalRotX - Input.mousePositionDelta.y * mouseSensitivity, -40.0f, 40.0f);
        pCamera.transform.localEulerAngles = Vector3.up*cameraLocalRotY + Vector3.right*cameraLocalRotX;
    }

    void DoCameraLookFixed(){
        //man cmon c# why doesnt (int)input[(int)I.space] work this looks so bad ;-;
        //tCameraRot = (tCameraRot + (input[(int)I.space] ? 1 : 0))/2;
        tCameraRot = Mathf.Clamp(tCameraRot + (input[(int)I.space] ? 0.2f : -0.2f), 0, 1);
        var angDiff = Mathf.Clamp(cameraLocalRotY, -1.5f, 1.5f);
        if (input[(int)I.space])
            angDiff *= (input[(int)I.forward] ^ input[(int)I.back]) ? (input[(int)I.back] ? 1 : -1) : 0;
        transform.eulerAngles = (transform.eulerAngles.y + angDiff) * Vector3.up;
        if (input[(int)I.space])
            return;
        cameraLocalRotY -= angDiff;
        pCamera.transform.localEulerAngles = Vector3.up*cameraLocalRotY + Vector3.right*cameraLocalRotX;
    }

    void PollInputs(){
        input[(int)I.forward] = (Input.GetKey("w") || Input.GetKey("up") || Input.GetKey("i"));
        input[(int)I.left] = (Input.GetKey("a") || Input.GetKey("left") || Input.GetKey("j"));
        input[(int)I.back] = (Input.GetKey("s") || Input.GetKey("down") || Input.GetKey("k"));
        input[(int)I.right] = (Input.GetKey("d") || Input.GetKey("right") || Input.GetKey("l"));

        input[(int)I.space] = (Input.GetKey("space") || Input.GetKey("enter"));

        input[(int)I.mb1] = Input.GetMouseButton(0);
    }
    void OnTriggerEnter(Collider other){
    if (other.CompareTag("Pickup"))
    {
        currentPickup = other.GetComponent<Pickup>();
    }
    }

    void OnTriggerExit(Collider other){
    if (other.GetComponent<Pickup>() == currentPickup)
    {
        currentPickup = null;
    }
    }

}