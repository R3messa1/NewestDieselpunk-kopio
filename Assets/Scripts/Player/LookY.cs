using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookY : MonoBehaviour
{
    [SerializeField]
    private float _mouseYSensitivity = 1f;
    [SerializeField]
    private float maxRotation = 100f;
    [SerializeField]
    private float verticalRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        verticalRotation = Mathf.Clamp(verticalRotation + mouseY * _mouseYSensitivity, -maxRotation, maxRotation);
        transform.localEulerAngles = Vector3.left * verticalRotation;
    }
}
