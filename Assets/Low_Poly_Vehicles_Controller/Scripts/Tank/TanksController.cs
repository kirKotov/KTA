using UnityEngine;
using Mirror;

[System.Serializable]
public class Track
{
	public bool enable;
	public WheelCollider[] wheelsCollider;
	public Transform[] wheelsTransform;
	public Transform[] wheelsBones;
}

public class TanksController : NetworkBehaviour
{
	public bool controlVehicle;

	public Vector3 centerOfMass = new Vector3(0, -1, 0);

	public Track leftTrack;
	public Track rightTrack;

	public float maxBrakeTorque = 1000f;
	public float motorForce = 1500;
	public float topSpeed = 20;
	public float currentSpeed;

	public Material rightTracksMat;
	public Material leftTracksMat;
	public float trackSpeed = 50;

	Rigidbody rb;
	private float m_horizontalInput;
	private float m_verticalInput;
	private float leftTrackSpeed;
	private float rightTrackSpeed;
	private AudioSource aSource;

    private TechSelection _techSelection;

    private GameObject _floatingTextObject;

    private void Start()
	{
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = centerOfMass;
		aSource = GetComponent<AudioSource>();

        _techSelection = transform.root.GetComponent<TechSelection>();

        leftTrack.enable = true;
		rightTrack.enable = true;

        if (!isLocalPlayer)
            _floatingTextObject = _techSelection.floatingInfo.GetChild(0).gameObject;
    }

	private void Update()
	{
        if (!isLocalPlayer)
        {
            if (_floatingTextObject == null)
                _floatingTextObject = _techSelection.floatingInfo.GetChild(0).gameObject;

            if (StaticZVariables.playerCamera != null)
                _floatingTextObject.transform.LookAt(_floatingTextObject.transform.position - (StaticZVariables.playerCamera.transform.position - _floatingTextObject.transform.position));

            return;
        }

        if (_techSelection != null)
        {
            _techSelection.floatingInfo.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 4.5f, gameObject.transform.position.z);
        }

        if (controlVehicle)
		{
			GetComponent<AudioSource>().enabled = true;
			GetComponent<GunsController>().gunsActive = true;
			GetComponent<TurretController>().turretActive = true;
		}
		else
		{
			GetComponent<AudioSource>().enabled = false;
			GetComponent<GunsController>().gunsActive = false;
			GetComponent<TurretController>().turretActive = false;
		}

		if (!controlVehicle)
			return;

		Movements();
        CmdEngineSound();
	}

	private void FixedUpdate()
	{
        if (!isLocalPlayer)
            return;

        UpdateWheelPoses();

		if (!controlVehicle)
			return;

		GetInput();
	}

	public void GetInput()
	{
		m_horizontalInput = Input.GetAxis("Horizontal");
		m_verticalInput = Input.GetAxis("Vertical");
	}

	private void Movements()
	{
		//float torque;
		float brake;

		currentSpeed = rb.velocity.magnitude * 3.6f;

		if (Input.GetKey(KeyCode.Space))
		{
			leftTrackSpeed = 0;
			rightTrackSpeed = 0;
			brake = maxBrakeTorque;
		}
		else
		{
			if (m_horizontalInput != 0)
			{
				leftTrackSpeed = motorForce * (m_horizontalInput * 2);
				rightTrackSpeed = motorForce * (-m_horizontalInput * 2);
			}
			else
			{
				leftTrackSpeed = motorForce * m_verticalInput;
				rightTrackSpeed = motorForce * m_verticalInput;
			}

			brake = 0;
		}


		for (int i = 0; i < leftTrack.wheelsCollider.Length; i++)
		{
			if(leftTrack.enable)
			{
				if (currentSpeed < topSpeed)
					leftTrack.wheelsCollider[i].motorTorque = leftTrackSpeed;
				else
					leftTrack.wheelsCollider[i].motorTorque = 0;

				leftTrack.wheelsCollider[i].brakeTorque = brake;
			}
			else
			{
				leftTrack.wheelsCollider[i].motorTorque = 0;
				leftTrack.wheelsCollider[i].brakeTorque = maxBrakeTorque;
			}
		}

		for (int i = 0; i < rightTrack.wheelsCollider.Length; i++)
		{
			if (rightTrack.enable)
			{
				if(currentSpeed < topSpeed)
					rightTrack.wheelsCollider[i].motorTorque = rightTrackSpeed;
				else
					rightTrack.wheelsCollider[i].motorTorque = 0;

				rightTrack.wheelsCollider[i].brakeTorque = brake;
			}
			else
			{
				rightTrack.wheelsCollider[i].motorTorque = 0;
				rightTrack.wheelsCollider[i].brakeTorque = maxBrakeTorque;
			}
		}

		rightTracksMat.mainTextureOffset = new Vector2(1, rightTrack.wheelsCollider[1].rpm * Time.deltaTime);
		leftTracksMat.mainTextureOffset = new Vector2(1, leftTrack.wheelsCollider[1].rpm * Time.deltaTime);

	}

	private void UpdateWheelPoses()
	{
		for (int i = 0; i < leftTrack.wheelsCollider.Length; i++)
		{
			UpdateWheelPose(leftTrack.wheelsCollider[i], leftTrack.wheelsTransform[i], leftTrack.wheelsBones[i]);
		}

		for (int i = 0; i < rightTrack.wheelsCollider.Length; i++)
		{
			UpdateWheelPose(rightTrack.wheelsCollider[i], rightTrack.wheelsTransform[i], rightTrack.wheelsBones[i]);
		}
	}

	private void UpdateWheelPose(WheelCollider _wheelCollider, Transform _wheelTransform, Transform _wheelBone)
	{
		Vector3 _pos = _wheelTransform.position;
		Quaternion _quat = _wheelTransform.rotation;

		_wheelCollider.GetWorldPose(out _pos, out _quat);

		_wheelTransform.position = _pos;
		_wheelTransform.rotation = _quat;

		_wheelBone.position = _pos;
	}

    [Command]
    private void CmdEngineSound()
    {
        RpcEngineSound();
    }

    [ClientRpc]
    private void RpcEngineSound()
    {
        aSource.pitch = 1 + (currentSpeed / topSpeed) * 2f;
    }
}