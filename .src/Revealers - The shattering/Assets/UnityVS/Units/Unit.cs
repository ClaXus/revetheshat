using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(CharacterController))]

public class Unit : MonoBehaviour {
    [SerializeField]
    Transform _transform;
    [SerializeField]
    CharacterController _CharacterController;
    [SerializeField]
	Player _ControlScript;

    public Transform transform {get { return _transform; }set { _transform = value; }}
    public CharacterController charactercontroller{get { return _CharacterController; }set { _CharacterController = value; }}
	public Player controlscript {get { return _ControlScript; }set { _ControlScript = value; }}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        _RefreshControls();
        _Move();
	}

    // Called 
    void FixedUpdate()
    {
        _RefreshControls();
    }

    #region Controls

    Vector3 _controlvector = Vector3.zero;

    /// <summary>
    /// RefreshControls collect input controller evolutions and transmit it to movement routine. Network compatible.
    /// </summary>
    public void _RefreshControls()
    {
        if (controlscript && controlscript.enabled)
        {
            _controlvector = controlscript.direction;
            _controlvector = transform.TransformDirection(_controlvector);
            _rotation = controlscript.rotation;
            /*if (networkview)
            {
                networkview.RPC("_NetworkRefeshControls", RPCMode.Others, _controlvector, _rotation);
            }*/
        }
    }

    /// <summary>
    /// NetworkRefreshControls is designed to be a remote procedure call to refresh controls and create Network prevention on other clients.
    /// </summary>
    /// <param name="control">Move input control. XZ for plan move, Y for jump/crouch</param>
    /// <param name="rot">Angle of rotation it is a Quaternion who represent the 3D angle of rotation.</param>
    /*[RPC]
    void _NetworkRefeshControls(Vector3 control, Quaternion rot)
    {
        _controlvector = control;
        _rotation = rot;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 sendpos = this.transform.position;
            stream.Serialize(ref sendpos);
        }
        else
        {
            Vector3 recpos = new Vector3();
            stream.Serialize(ref recpos);
            this.transform.position = recpos;
        }
    }*/
    #endregion Controls

    #region Details

    [SerializeField]
    string              _name = "Default";
    [SerializeField]
    string              _clan = "NoClan";
    [SerializeField]
    string              _title = "Title";

    #endregion Details

    #region Movement

    Quaternion          _rotation = Quaternion.Euler(0, 0, 0);
    Vector3             _movevector = Vector3.zero;
    /// <summary>
    /// Define if unit is sensible to gravity.
    /// </summary>
    [SerializeField]
    unittype _UnitType = unittype.Ground;
    /// <summary>
    /// Define if unit is able to move.
    /// </summary>
    [SerializeField]
    movetype _MoveType = movetype.Movable;
    /// <summary>
    /// Speed percent while walking on the ground.
    /// </summary>
    [SerializeField]
    float _Speed = 100;
    /// <summary>
    /// The force of the jump in percent.
    /// </summary>
    [SerializeField]
    float _JumpHeight = 100;
    /// <summary>
    /// Speed while moving through the air. useless for ground units.
    /// </summary>
    [SerializeField]
    int _FlightSpeed = 100;

    public enum unittype
    {
        Ground,
        Fly
    };
    public enum movetype
    {
        Movable,
        Stationary
    };

    public unittype UnitType{get { return _UnitType; }set { _UnitType = value; }}
    public movetype MoveType{get { return _MoveType; }set { _MoveType = value; }}
    public float Speed{get { return _Speed; }set { _Speed = value; }}
    public float JumpHeight{get { return _JumpHeight; }set { _JumpHeight = value; }}
    public int FlightSpeed{get { return _FlightSpeed; }set { _FlightSpeed = value; }}


    /// <summary>
    /// Move is the method who manage all move behaviour of Unit. Any Movement, from input or even forces or event, should be managed by Move.
    /// </summary>
    public void _Move()
    {
        if(_MoveType == movetype.Movable) // IF CAN MOVE
        {
            if(_UnitType == unittype.Ground) // IF GROUND UNIT
            {
                transform.Rotate(Vector3.up, _rotation.eulerAngles.y);
                if(_CharacterController.isGrounded)
                {
                    _movevector = Vector3.Normalize(new Vector3(_controlvector.x, 0, _controlvector.z));
                    _movevector = _movevector * 20.0f * (_Speed / 100);
                    _movevector.y = _controlvector.y * 20.0f * (_JumpHeight / 100);
                }
                _movevector.y -= 60.0f * Time.deltaTime;
            }
            else if(_UnitType == unittype.Fly) // IF FLY UNIT
            {
                transform.Rotate(Vector3.up, _rotation.eulerAngles.y);
                if(_CharacterController.isGrounded)
                {
                    _movevector = Vector3.Normalize(new Vector3(_controlvector.x, 0, _controlvector.z));
                    _movevector = _movevector * 20.0f * (_Speed / 100);
                    _movevector.y = _controlvector.y * 20.0f * (_JumpHeight / 100);
                }
                else
                {
                    _movevector = Vector3.Normalize(new Vector3(_controlvector.x, _controlvector.y, _controlvector.z));
                    _movevector = _movevector * 20.0f * (_FlightSpeed / 100);
                }
            }
            _CharacterController.Move(_movevector * Time.deltaTime);
        }
    }

    #endregion Movement

    #region Combat

    [SerializeField]
    bool _IsDead = false;

    public bool IsDead{get { return _IsDead; }}

    [SerializeField]
    int _MaxHp = 100;

    public int MaxHp
    {
        get { return _MaxHp; }
        set { if (value >= 0) _MaxHp = value; CheckIsDead(); }
    }

    [SerializeField]
    int _Hp = 100;

    public int Hp
    {
        get { return _Hp; }
        set { if (value > _MaxHp) _Hp = _MaxHp; else _Hp = value; CheckIsDead(); }
    }

    [SerializeField]
    int _HpRegen = 0;
    [SerializeField]
    int _Shield = 0;

    [SerializeField]
    Unit _Target;

    public Unit Target {get { return _Target; }set { _Target = value; }}

    [SerializeField]
    Spell abilities;

    void LimitHp()
    {
        if (Hp > _MaxHp)
            Hp = _MaxHp;
    }

    void CheckIsDead()
    {
        if (_Hp <= 0)
            _IsDead = true;
        else
            _IsDead = false;
    }

    void TakeDamage(int damages)
    {
        if (damages < 0)
            return;
        Hp -= damages;
    }

    #endregion Combat
}
