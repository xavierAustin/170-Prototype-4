using UnityEngine;
using System.Collections;

public class EnemyParticleBehavior : MonoBehaviour
{
    void Start(){
        StartCoroutine(Lifetime());
    }

    IEnumerator Lifetime(){
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }
}
