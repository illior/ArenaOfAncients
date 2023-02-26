using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class PlayerInputController : MonoBehaviour
	{
		[SerializeField] private Transform orientation;
		[SerializeField] private float jumpCooldown = 0.25f;

		private PlayerMovement _playerMovement;

		private float verticalInput, horizontalInput;

		private void Awake()
		{
			_playerMovement = GetComponent<PlayerMovement>();
		}

		private void Start()
		{
			_playerMovement.orientation = orientation;
		}

		private void Update()
		{
			MyInput();	
		}

		private void FixedUpdate()
		{
			_playerMovement.MovePlayer(verticalInput, horizontalInput);
		}

		private void MyInput()
		{
			verticalInput = Input.GetAxis("Vertical");
			horizontalInput = Input.GetAxis("Horizontal");

			if(Input.GetKey(KeyBinds.JumpKey) && _playerMovement.CanJump())
			{
				_playerMovement.Jump();

				_playerMovement.Invoke(nameof(_playerMovement.ResetJump), jumpCooldown);
			}
		}
	}
}