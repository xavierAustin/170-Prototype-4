using UnityEngine.SceneManagement;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if (other.gameObject.GetComponent<Player>())
            SceneManager.LoadScene("WinScreen");
    }
}
