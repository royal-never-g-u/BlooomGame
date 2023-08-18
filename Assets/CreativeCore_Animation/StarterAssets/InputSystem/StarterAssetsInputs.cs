using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		private int sensitivity = 100;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
#endif

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#else
        // old input sys if we do decide to have it (most likely wont)...

#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			//Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif
        public void Start()
        {
			
        }
        public void Update()
        {
			move = Vector2.zero;
			if (Input.GetKeyDown(KeyCode.Space))
			{
				jump = true;
			}
			if (Input.GetKey(KeyCode.W))
			{
				move += Vector2.up;
			}
            if (Input.GetKey(KeyCode.S))
            {
                move += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                move += Vector2.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                move += Vector2.right;
            }
			if (Input.GetKey(KeyCode.LeftShift))
			{
				sprint = true;
			}
			else
			{
				sprint = false;
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (Cursor.lockState == CursorLockMode.Locked)
				{
                    Cursor.lockState = CursorLockMode.None;
                }
				else
				{
					Cursor.lockState = CursorLockMode.Locked;
                }
			}
			if (Cursor.lockState == CursorLockMode.Locked)
			{
				float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
				float mouseY = -Input.GetAxisRaw("Mouse Y") * sensitivity;
				look = new Vector2(mouseX, mouseY);
			}
        }
    }
	
}