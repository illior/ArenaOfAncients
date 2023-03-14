using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform orientation;
	[SerializeField] private Transform playerCamera;

	[Header("Dashing")]
	[SerializeField] private float dashForce;
	[SerializeField] private float dashUpwardForce;
	[SerializeField] private float dashDuration;

	[Header("Cooldown")]
	[SerializeField] private float dashCd;
	private float dashCdTimer;

	[Header("Settings")]
	//[SerializeField] private bool useCameraForward = true;
	//[SerializeField] private bool allowAllDirections = true;
	[SerializeField] private bool disableGravity = false;
	[SerializeField] private bool resetVel = true;

	private Rigidbody _rigidbody;
	private PlayerMovement _playerMovement;
	private PlayerStats _playerStats;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
		_playerMovement = GetComponent<PlayerMovement>();
		_playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyBinds.DashKey))
		{
			Dash();
		}

		if(dashCdTimer > 0)
		{
			dashCdTimer -= Time.deltaTime;
		}
    }

    private void Dash()
	{
		if(dashCdTimer > 0 )
		{
			return;
		} else
		{
			dashCdTimer = dashCd;
		}

		_playerMovement.dashing = true;

		Vector3 forceToApply = orientation.forward * _playerStats.DashForce + orientation.up * dashUpwardForce;

        delayedForceToApply = forceToApply;
		Invoke(nameof(DelayedDashForce), 0.025f);

        Invoke(nameof(ResetDash), dashDuration);
	}

	private Vector3 delayedForceToApply;
	private void DelayedDashForce()
	{
        if (resetVel)
            _rigidbody.velocity = Vector3.zero;

        _rigidbody.AddForce(delayedForceToApply * _rigidbody.mass, ForceMode.Impulse);
    }

	private void ResetDash()
	{
		_playerMovement.dashing = false;

        if (disableGravity)
            _rigidbody.useGravity = true;
    }
}
