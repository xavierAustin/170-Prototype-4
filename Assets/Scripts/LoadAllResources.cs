using UnityEngine;

public class LoadAllResources : MonoBehaviour
{
    GameObject player = Resources.Load<GameObject>("PlayerObject");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var temp = GameObject.Instantiate(player);
        temp.transform.position = Vector3.zero;
        Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
