using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _distanceFromObject;
    [SerializeField] private float _speed;

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 positionToGo = _target.transform.position + _distanceFromObject;

        transform.position = Vector3.MoveTowards(transform.position, positionToGo, _speed * Time.deltaTime);
    }
}
