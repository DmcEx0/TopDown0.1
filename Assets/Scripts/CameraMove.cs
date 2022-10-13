using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Vector3 _distanceFromObject;
    [SerializeField] private float _speed;

    private void LateUpdate()
    {
        Vector3 positionToGo = _player.transform.position + _distanceFromObject;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, positionToGo, _speed);
        transform.position = smoothPosition;
    }
}
