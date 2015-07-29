using UnityEngine;
using System.Collections;

public abstract class ControlScript : MonoBehaviour {
    private Vector3 _prevDirection = Vector3.zero;
    private Quaternion _prevRotation = Quaternion.identity;
    private Vector3 _direction = Vector3.zero;
    private Quaternion _rotation = Quaternion.identity;

    /// <summary>
    /// Direction vector provided by Unit controller.
    /// </summary>
    public Vector3 direction
    {
        get
        {
            return _direction;
        }
        protected set
        {
            _direction = value;
        }
    }
    
    /// <summary>
    /// Rotation angle provided by Unit controller.
    /// </summary>
    public Quaternion rotation
    {
        get
        {
            return _rotation;
        }
        protected set
        {
            _rotation = value;
        }
    }

    /// <summary>
    /// Tell if the controls changed from the last time this fuction was called.
    /// </summary>
    public bool hasChanged()
    {
        if(_prevDirection != _direction || _prevRotation != _rotation)
        {
            _prevDirection = _direction;
            _prevRotation = _rotation;
            return true;
        }
        return false;
    }
}
