using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]

/*
 * Ce code doit être optimisé, mais il est fonctionnel.
 */
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    Unit unit;
    [SerializeField]
    LayerMask layermask;
    [SerializeField]
    float maxCameraDistance = 20.0F;
    [SerializeField]
    float maxAngle = 88.0F;

    float _cameraDesiredDistance;

    public float cameraDesiredDistance
    {
        get { return _cameraDesiredDistance; }
        set 
        {
            if (value > maxCameraDistance)
                _cameraDesiredDistance = maxCameraDistance;
            else if (value < 0.0f)
                _cameraDesiredDistance = 0.0f;
            else
                _cameraDesiredDistance = value;
        }
    }
    Vector3 _cameraDistance = Vector3.zero;

    public Vector3 cameraDistance
    {
        get { return _cameraDistance; }
        set { _cameraDistance = value; }
    }

    Quaternion _cameraAngle;

    public Quaternion cameraAngle
    {
        get { return _cameraAngle; }
        set { _cameraAngle = value; }
    }

    float x_input
    {
        get 
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                return -Input.GetAxis("Mouse Y");
            else
                return 0.0f;
        }
    }

    void Start()
    {
        cameraAngle = transform.rotation;
    }
    void Update() // this code is called once per frame
    {
        //float x_input = Input.GetMouseButton(0) || Input.GetMouseButton(1) ? -Input.GetAxis("Mouse Y") : 0.0f;
        float y_input = Input.GetMouseButton(0) && !Input.GetMouseButton(1) ? Input.GetAxis("Mouse X") : 0.0f;

        cameraAngle = Quaternion.Euler(cameraAngle.eulerAngles.x + x_input, cameraAngle.eulerAngles.y + y_input, 0.0f);
        Debug.Log(cameraAngle.eulerAngles);
        cameraDistance = cameraAngle * Vector3.back * 20.0f + Vector3.up * unit.charactercontroller.height;
        transform.localRotation = cameraAngle;
        transform.localPosition = cameraDistance;
    }
}