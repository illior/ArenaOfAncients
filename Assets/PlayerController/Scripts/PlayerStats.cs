using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController
{
	public class PlayerStats : MonoBehaviour
	{
		[SerializeField] private float walkSpeed = 5f;
		public float WalkSpeed
		{
			get
			{
				return walkSpeed;
			}
		}

		[SerializeField] private float sprintSpeed = 8f;
		public float SprintSpeed
        {
			get
			{
				return sprintSpeed;
			}
		}

		[SerializeField] private float jumpForce = 6f;
		public float JumpForce
		{
			get
			{
				return jumpForce;
			}
		}
	}
}