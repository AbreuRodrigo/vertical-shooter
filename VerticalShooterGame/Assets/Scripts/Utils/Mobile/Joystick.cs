using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent (typeof(RectTransform))]
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	public RectTransform handle;
	public RectTransform canvas;
	public Vector2 autoReturnSpeed = new Vector2 (10.0f, 10.0f);
	public float radius = 50.0f;

	public event Action<Joystick, Vector2> OnStartJoystickMovement;
	public event Action<Joystick, Vector2> OnJoystickMovement;
	public event Action<Joystick> OnEndJoystickMovement;

	private bool returnHandle;

	void Start () {	
		returnHandle = true;

		RectTransform touchZone = GetComponent<RectTransform> ();

		touchZone.pivot = Vector2.one * 0.5f;
	}

	void Update () {
		if (returnHandle) {
			if (handle.anchoredPosition.magnitude > Mathf.Epsilon) {
				handle.anchoredPosition -= new Vector2 (handle.anchoredPosition.x * autoReturnSpeed.x, handle.anchoredPosition.y * autoReturnSpeed.y) * Time.deltaTime;
			} else {
				returnHandle = false;
			}
		}
	}

	public Vector2 Coordinates {
		get {
			if (handle.anchoredPosition.magnitude < radius) {
				return handle.anchoredPosition / radius;
			}

			return handle.anchoredPosition.normalized;
		}
	}

	void IPointerDownHandler.OnPointerDown (PointerEventData eventData) {
		returnHandle = false;

		Vector2 handleOffset = GetJoystickOffset (eventData);

		handle.anchoredPosition = handleOffset;

		if (OnStartJoystickMovement != null) {
			OnStartJoystickMovement (this, Coordinates);
		}
	}

	void IDragHandler.OnDrag (PointerEventData eventData) {
		Vector2 handleOffset = GetJoystickOffset (eventData);
		handle.anchoredPosition = handleOffset;

		if (OnJoystickMovement != null) {
			OnJoystickMovement (this, Coordinates);
		}
	}

	void IPointerUpHandler.OnPointerUp (PointerEventData eventData) {
		returnHandle = true;

		if (OnEndJoystickMovement != null) {
			OnEndJoystickMovement (this);
		}
	}

	private Vector2 GetJoystickOffset (PointerEventData eventData) {
		Vector3 globalHandle;

		if (RectTransformUtility.ScreenPointToWorldPointInRectangle (canvas, eventData.position, eventData.pressEventCamera, out globalHandle)) {
			handle.position = globalHandle;
		}

		Vector2 handleOffset = handle.anchoredPosition;

		if (handleOffset.magnitude > radius) {
			handleOffset = handleOffset.normalized * radius;
			handle.anchoredPosition = handleOffset;
		}

		return handleOffset;
	}
}