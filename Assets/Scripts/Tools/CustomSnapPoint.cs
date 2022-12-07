using UnityEngine;

public class CustomSnapPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        float radius = 0.1f;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
