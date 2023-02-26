using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
	[SerializeField] private Transform orientation;
	[SerializeField] private Transform playerObject;

	[SerializeField] private float sensX = 400f;
	[SerializeField] private float sensY = 400f;

	private float mouseX, mouseY;

	private float xRotation, yRotation;

	void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
	{
		mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
		mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

		yRotation += mouseX;
		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -60f, 60f);

		transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
		orientation.rotation = Quaternion.Euler(0, yRotation, 0);
		playerObject.rotation = Quaternion.Euler(0, yRotation, 0);
	}
}
