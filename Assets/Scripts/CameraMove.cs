using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _distanceFromObject;
    [SerializeField] private float _speed;

    private void FixedUpdate()
    {
        if (_target == null)
            return;

        Vector3 positionToGo = _target.transform.position + _distanceFromObject;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, positionToGo, _speed * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
