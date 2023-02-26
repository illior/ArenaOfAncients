using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class PlayerMovement : MonoBehaviour
	{
		[Header("Movement")]
		[SerializeField] private float groundDrag;
		[SerializeField] private float airMultiplier;
		private bool readyToJump;
		private float moveSpeed;

		[Header("Ground Check")]
		[SerializeField] private float playerHeight;
		[SerializeField] private LayerMask ground;
		private bool grounded;

		private Rigidbody _rigidbody;
		private PlayerStats _playerStats;
		[HideInInspector] public Transform orientation;

		private Vector3 moveDirection;

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_playerStats = GetComponent<PlayerStats>();
		}

		private void Start()
		{
			_rigidbody.freezeRotation = true;

			readyToJump = true;
		}

		private void Update()
		{
			grounded = Physics.Raycast(transform.position + playerHeight * 0.5f * Vector3.up, Vector3.down, playerHeight * 0.5f + 0.3f, ground);

			SpeedControl();

			if (grounded)
				_rigidbody.drag = groundDrag;
			else
				_rigidbody.drag = 0;
		}

		public void MovePlayer(float verticalInput, float horizontalInput)
		{
			moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

			moveSpeed = _playerStats.WalkSpeed;

			if(grounded)
				_rigidbody.AddForce(moveDirection.normalized * moveSpeed * _rigidbody.mass * 10f, ForceMode.Force);
			else if(!grounded)
				_rigidbody.AddForce(moveDirection.normalized * moveSpeed * _rigidbody.mass * airMultiplier * 10f, ForceMode.Force);
		}

		private void SpeedControl()
		{
			Vector3 flatVel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

			if(flatVel.magnitude > moveSpeed)
			{
				Vector3 limitedVel = flatVel.normalized * moveSpeed;
				_rigidbody.velocity = new Vector3(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
			}
		}

		public bool CanJump()
		{
			return readyToJump && grounded;
		}

		public void Jump()
		{
			readyToJump = false;

			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

			_rigidbody.AddForce(transform.up * _playerStats.JumpForce * _rigidbody.mass, ForceMode.Impulse);
		}

		public void ResetJump()
		{
			readyToJump = true;
		}
	}
}