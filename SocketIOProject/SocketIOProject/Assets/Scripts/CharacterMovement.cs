using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour 
{
	public float speed = 6f;			
	public float turnSpeed = 60f;		
	public float turnSmoothing = 15f;

	private Vector3 movement;
	private float movementSpeed;
	private Vector3 turning;
	private Animator anim;
	private Rigidbody playerRigidbody;

	void Awake()
	{
		//Get references to components
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		//Store input axes
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis ("Vertical");
		Move (h, v);

		//Animating without BlendTree
		Animating(h, v);
		//Animating withBlendtree
		//Animating(movementSpeed);
	}

	void Move(float h, float v)
	{
		//Move the player
		movement.Set (h, 0f, v);
		movementSpeed = movement.magnitude;
		movement = movement * speed * Time.deltaTime;


        playerRigidbody.MovePosition(transform.position + movement);

		if(h != 0f || v != 0f)
		{
			Rotating(h, v);
		}

	}

    public void NetworkMovement(Vector3 pos)
    {
        transform.position = pos;
    }

	void Rotating(float h, float v)
	{
		Vector3 targetDirection = new Vector3(h, 0f, v);
		Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp (GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);
		GetComponent<Rigidbody>().MoveRotation(newRotation);
	}

	//Regular Animation
	void Animating(float h, float v)
	{
		bool running = h != 0f || v != 0f;

		anim.SetBool ("IsRunning", running);
	}
	//Blend Animation
//	void Animating(float mag)
//	{
//		//bool running = lh != 0f || lv != 0f;
//		if (Input.GetKey (KeyCode.RightShift)) {
//			anim.SetFloat ("Speed", movementSpeed *= 0.5f);
//		} else {
//
//			anim.SetFloat ("Speed", movementSpeed);
//		}
//	}
}
