using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace VerticalShooter {

	//This class represents the joytick for controlling the playeSpaceShip on Android devices (also works for IOS, though, not configured now)
	[RequireComponent (typeof(RectTransform))]
	public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
		//This is the top and controllable part of the joystick
		public RectTransform handler;
		//This is the joystick base, serving as a container and parent for the handler
		public RectTransform joystickBase;
		//This is the speed in which the handler goes back to its initial position
		public Vector2 autoReturnSpeed = new Vector2 (10.0f, 10.0f);
		//This is the size of the handler in radius
		public float radius = 50.0f;

		//This represents the onFingerDown event for the joystick based in a 2d position
		public event Action<Joystick, Vector2> OnStartJoystickMovement;
		//This represents the onFingerMove event for the joystick based in a current 2d position
		public event Action<Joystick, Vector2> OnJoystickMovement;
		//This represents the onFingerUp event for the joystick
		public event Action<Joystick> OnEndJoystickMovement;
		//The condition for the handler move back to its initial position
		private bool returnHandler;

		void Start () {
			//Setting the condition to move the handler back to its initial position to true
			returnHandler = true;
		}

		void Update () {
			//Testing each frame, if the condition for making the joystick handler is true, then...
			if (returnHandler) {
				//Testing if the magnitude (size of the position vector) of the joystick handler is greater than 
				//the smallest float value different than zero
				if (handler.anchoredPosition.magnitude > Mathf.Epsilon) {
					//Whenever the returnHandler is true, makes the handler go back to its initial position using the autoReturnSpeed
					//stopping if it's closer enough to zero position (or the handler intial position)
					handler.anchoredPosition -= new Vector2 (handler.anchoredPosition.x * autoReturnSpeed.x, handler.anchoredPosition.y * autoReturnSpeed.y) * Time.deltaTime;
				} else {
					//Else, if in some moment the magnitude of the position vector of the handler is zero, 
					//the condition 'returnHandler' becomes false
					returnHandler = false;
				}
			}
		}

		//This is the callback coordinates for moving something using the joystick's normalized position
		public Vector2 Coordinates {
			get {
				//Testing if the handler's position vector's size is smaller than the current radius (50 by default), then...
				if (handler.anchoredPosition.magnitude < radius) {
					//returns as coordinates the handler's current position divided by the current radius
					return handler.anchoredPosition / radius;
				}

				//Else, returns as coordinates the current handler's normalized position
				return handler.anchoredPosition.normalized;
			}
		}

		//Implementation of the OnPointerDown event handler of the joystick
		void IPointerDownHandler.OnPointerDown (PointerEventData eventData) {
			//While the handler is being touched (with the pointer down), then the handler will not try to go back to its initial posit.
			returnHandler = false;
			//Fixes the joytsick handler's anchoredPosition using the joystickOffset in relation to the game screen
			handler.anchoredPosition = GetJoystickOffset (eventData);

			//Testing if it's not null to avoid nullpointer exception
			if (OnStartJoystickMovement != null) {
				OnStartJoystickMovement (this, Coordinates);
			}
		}

		//Implementation of the OnDrag event handler of the joystick
		void IDragHandler.OnDrag (PointerEventData eventData) {
			//Fixes the joytsick handler's anchoredPosition using the joystickOffset in relation to the game screen
			handler.anchoredPosition = GetJoystickOffset (eventData);

			//Testing if it's not null to avoid nullpointer exception
			if (OnJoystickMovement != null) {
				//Telling the event what is the dragged object and creating a callback for the current position (coordinates)
				OnJoystickMovement (this, Coordinates);
			}
		}

		//Implementation of the OnPointerUp event handler of the joystick
		void IPointerUpHandler.OnPointerUp (PointerEventData eventData) {
			//The joystick handler has been released, so the handler will start moving back to its initial position
			returnHandler = true;

			//Testing if it's not null to avoid nullpointer exception
			if (OnEndJoystickMovement != null) {
				//Telling the event which was the object released
				OnEndJoystickMovement (this);
			}
		}

		private Vector2 GetJoystickOffset (PointerEventData eventData) {
			Vector3 globalHandler;

			//Transform a screen space point to a position in world space that is on the plane of the given RectTransform.
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle (joystickBase, eventData.position, eventData.pressEventCamera, out globalHandler)) {
				//Assigning the returned Vector (globalHandler) to the joystick handler's position
				handler.position = globalHandler;
			}

			//Assigning the joystick handler's anchoredPosition to the hanlderOffset
			Vector2 handlerOffset = handler.anchoredPosition;

			//Tests if the size of the position vector of the handlerOffset is greater than the current radius, then...
			if (handlerOffset.magnitude > radius) {
				//Redefine the handlerOffset to its normalized position vector multiplyed by the current radius
				handlerOffset = handlerOffset.normalized * radius;
				//Fix the joytick handler's anchoredPosition accordingly to the obtained handlerOffset
				handler.anchoredPosition = handlerOffset;
			}

			//returns the calculated handlerOffset
			return handlerOffset;
		}
	}
}