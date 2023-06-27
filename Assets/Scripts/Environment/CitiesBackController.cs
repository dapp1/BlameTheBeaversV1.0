using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CitiesBackController : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float _lagAmount = 0.3f;

    [SerializeField]
    private float Paralax => 1f - _lagAmount;

    private Transform _camera;
    private Vector3 _previousCameraPosition;

    private void Awake()
    {
        _camera = Camera.main.transform;
        _previousCameraPosition = _camera.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var movement = CameraMovement;
        if (movement == Vector3.zero) return;

        var targetPosition = new Vector3(transform.position.x + movement.x * Paralax, transform.position.y, transform.position.z);
        transform.position = targetPosition;
    }

    Vector3 CameraMovement
    {
        get
        {
            var movement = _camera.position - _previousCameraPosition;
            _previousCameraPosition = _camera.position;
            return movement;
        }
    }
}
