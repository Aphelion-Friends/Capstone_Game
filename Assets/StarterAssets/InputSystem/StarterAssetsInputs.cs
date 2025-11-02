using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using static Unity.Collections.AllocatorManager;
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
                public bool selectFire = false;
                public bool pause = false;
                public bool reload;
                public bool inventoryOpen;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool click = false;

                [Header("Lock Input")]
                public bool locked = false;

#if ENABLE_INPUT_SYSTEM
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

        public void OnFire(InputValue value)
        {
			if (InventoryUI.inventoryOpen)
			{
				return;
			}
            ClickInput(value.isPressed);
        }

        public void OnReload(InputValue value)
        {
            ReloadInput(value.isPressed);
        }

        public void OnSelectFire(InputValue value)
        {
            SelectFireInput(value.isPressed);
        }

        public void OnPause(InputValue value)
        {
            PauseInput(value.isPressed);
        }
        public void OnOpenInventory(InputValue value)
        {
            OpenInventoryInput(value.isPressed);
        }



#endif


        public void MoveInput(Vector2 newMoveDirection)
		{
			if (locked == false) move = newMoveDirection;
                        else move = Vector3.zero;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
                    if (locked == false) look = newLookDirection;
                    else look = Vector3.zero;
		}

		public void JumpInput(bool newJumpState)
		{
			if (locked == false) jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			if (locked == false) sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        public void ClickInput(bool newClickState)
        {
            if (locked == false) click = newClickState;
        }

        public void ReloadInput(bool newReloadState)
        {
            if (locked == false) reload = newReloadState;
        }
        public void SelectFireInput(bool newSelectFireState)
        {
            if (locked == false) selectFire = newSelectFireState;
        }
        public void PauseInput(bool newPauseState)
        {
            pause = newPauseState;
        }
        public void OpenInventoryInput(bool newInventoryState)
        {
            inventoryOpen = newInventoryState;
        }





    }



}
