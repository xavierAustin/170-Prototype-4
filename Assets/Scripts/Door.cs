using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public Button[] buttons;
    float yScale;
    Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        yScale = transform.localScale.y;
        pos = transform.position;
        StartCoroutine(DoorLogic());
    }

    IEnumerator DoorLogic(){
        while (true){
            yield return new WaitUntil(() => DoorCheck());
            while (Mathf.Abs(transform.localScale.y) > 0.05f && DoorCheck()){
                transform.localScale = new Vector3(transform.localScale.x,transform.localScale.y/2,transform.localScale.z);
                transform.position += transform.localScale.y * transform.up / 2;
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitUntil(() => !DoorCheck());
            while (Mathf.Abs(transform.localScale.y) < yScale && !DoorCheck()){
                transform.localScale = new Vector3(transform.localScale.x,(transform.localScale.y + yScale)/2,transform.localScale.z);
                transform.position = (transform.position + pos) / 2;
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    bool DoorCheck(){
        for (int i = 0; i < buttons.Length; i++){
            if (!buttons[i].pressed)
                return false;
        }
        return true;
    }
}
