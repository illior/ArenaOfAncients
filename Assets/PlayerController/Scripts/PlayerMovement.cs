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
		private bool sprint;

		[Header("Ground Check")]
		[SerializeField] private float playerHeight;
		[SerializeField] private LayerMask ground;
		private bool grounded;

		[Header("Slope Handler")]
		[SerializeField, Range(0, 60)] private float maxSlopeAngle;
		private RaycastHit slopeHit;
		private bool slope;
		private bool exitingSlope;

        private Rigidbody _rigidbody;
		private PlayerStats _playerStats;
		[HideInInspector] public Transform orientation;

		private Vector3 moveDirection;

		public enum MovementState
		{
			walking,
			sprinting,
			air
		}

		private MovementState state;
		public MovementState State
		{
			get { return state; }
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody>();
			_playerStats = GetComponent<PlayerStats>();
		}

		private void Start()
		{
			_rigidbody.freezeRotation = true;

			readyToJump = true;
			moveSpeed = _playerStats.WalkSpeed;
		}

		private void Update()
		{
			grounded = Physics.Raycast(transform.position + playerHeight * 0.5f * Vector3.up, Vector3.down, playerHeight * 0.5f + 0.3f, ground);
			slope = OnSlope();

			SpeedControl();
			StateHandler();


			if(grounded)
				_rigidbody.drag = groundDrag;
			else
				_rigidbody.drag = 0;
		}

		private void StateHandler()
		{
			if(grounded && sprint)
			{
				state = MovementState.sprinting;
				moveSpeed = _playerStats.SprintSpeed;
			} else if(grounded)
			{
				state = MovementState.walking;
				moveSpeed = _playerStats.WalkSpeed;
			} else
			{
				state = MovementState.air;
			}
		}

		public void MovePlayer(float verticalInput, float horizontalInput, bool sprintInput = false)
		{
			moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
			sprint = sprintInput;

			moveSpeed = _playerStats.WalkSpeed;

			if (slope && !exitingSlope)
			{
				_rigidbody.AddForce(GetSlopeMoveDirection() * moveSpeed * _rigidbody.mass * 20f, ForceMode.Force);

				if(_rigidbody.velocity.y > 0)
				{
					_rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
				}
			} else if (grounded)
				_rigidbody.AddForce(moveDirection.normalized * moveSpeed * _rigidbody.mass * 10f, ForceMode.Force);
			else if(!grounded)
				_rigidbody.AddForce(moveDirection.normalized * moveSpeed * _rigidbody.mass * airMultiplier * 10f, ForceMode.Force);

			_rigidbody.useGravity = !slope;
		}

		private void SpeedControl()
		{
			if (OnSlope() && !exitingSlope)
			{
				if (_rigidbody.velocity.magnitude > moveSpeed)
                    _rigidbody.velocity = _rigidbody.velocity.normalized * moveSpeed;
			} else
			{
				Vector3 flatVel = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

				if (flatVel.magnitude > moveSpeed)
				{
					Vector3 limitedVel = flatVel.normalized * moveSpeed;
					_rigidbody.velocity = new Vector3(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
				}
			}
		}

		public bool CanJump()
		{
			return readyToJump && grounded;
		}

		public void Jump()
		{
            exitingSlope = true;
            readyToJump = false;

			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

			_rigidbody.AddForce(transform.up * _playerStats.JumpForce * _rigidbody.mass, ForceMode.Impulse);
		}

		public void ResetJump()
		{
            exitingSlope = false;
            readyToJump = true;
		}

		private bool OnSlope()
		{
			if(Physics.Raycast(transform.position + playerHeight * 0.5f * Vector3.up, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, ground))
			{
				float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

				return angle < maxSlopeAngle && angle != 0;
			}

			return false;
		}

		private Vector3 GetSlopeMoveDirection()
		{
			return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
		}
	}
}