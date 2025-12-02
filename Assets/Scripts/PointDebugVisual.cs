using UnityEngine;

public class PointDebugVisual : MonoBehaviour
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position+Vector3.up/5,transform.position+Vector3.down/5);
        Gizmos.DrawLine(transform.position+Vector3.left/5,transform.position+Vector3.right/5);
        Gizmos.DrawLine(transform.position+Vector3.forward/5,transform.position+Vector3.back/5);
    }
}
