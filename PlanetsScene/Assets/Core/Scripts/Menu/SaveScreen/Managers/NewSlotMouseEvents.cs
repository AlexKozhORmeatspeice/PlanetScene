using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using Planet_Window;

namespace Save_screen
{
	public interface INewSlotMouseEvents
	{
		event Action<INewSaveSlot> OnMouseEnter;
		event Action<INewSaveSlot> OnMouseMove;
		event Action<INewSaveSlot> OnMouseLeave;
		event Action<INewSaveSlot> OnMouseClick;
		event Action<INewSaveSlot> OnMouseDoubleClick;
	}

	public class NewSlotMouseEvents : INewSlotMouseEvents, ITickable, IStartable, IDisposable
	{
		[Inject] private IPointerManager pointer;
		private INewSaveSlot hoveredSlot;
		private PointerEventData eventData;
		private List<RaycastResult> raycastList = new();

		public event Action<INewSaveSlot> OnMouseEnter;
		public event Action<INewSaveSlot> OnMouseLeave;
		public event Action<INewSaveSlot> OnMouseClick;
		public event Action<INewSaveSlot> OnMouseDoubleClick;
		public event Action<INewSaveSlot> OnMouseMove;

		private bool isDetectedButton;
		public void Initialize()
		{
			isDetectedButton = false;	
			pointer.OnDoubleTap += InvokeDoubleClick;
			pointer.OnPointerDown += InvokeClick;

			eventData = new PointerEventData(EventSystem.current);
		}

		public void Tick()
		{
			raycastList.Clear();
			eventData.position = Input.mousePosition;
			EventSystem.current.RaycastAll(eventData, raycastList);

			if (raycastList.Count == 0)
			{
				if (hoveredSlot != null)
				{
					OnMouseLeave?.Invoke(hoveredSlot);
					hoveredSlot = null;
				}
				return;
			}

			INewSaveSlot newSlot = null;
			foreach (RaycastResult result in raycastList)
			{
                if (result.gameObject.GetComponent<ITextButton>() != null || result.gameObject.GetComponent<IIconButton>() != null)
                {
					isDetectedButton = true;
                    return;
                }
				else
				{
					isDetectedButton = false;
				}

                if (result.gameObject.TryGetComponent(out newSlot))
				{
					break;
				}
			}

			if (newSlot == null)
			{
				if (hoveredSlot != null)
				{
					OnMouseLeave?.Invoke(hoveredSlot);
					hoveredSlot = null;
				}
				return;
			}

			if (hoveredSlot != newSlot)
			{
				if (hoveredSlot != null)
				{
					OnMouseLeave?.Invoke(hoveredSlot);
				}

				hoveredSlot = newSlot;
				OnMouseEnter?.Invoke(hoveredSlot);
			}

			OnMouseMove?.Invoke(hoveredSlot);
		}
		public void Dispose()
		{
			pointer.OnDoubleTap -= InvokeDoubleClick;
			pointer.OnPointerDown -= InvokeClick;
		}

		private void InvokeClick()
		{
			if (hoveredSlot == null || isDetectedButton)
				return;

			OnMouseClick?.Invoke(hoveredSlot);
		}
		private void InvokeDoubleClick()
		{
			if (hoveredSlot == null || isDetectedButton)
				return;

			OnMouseDoubleClick?.Invoke(hoveredSlot);
		}

		public void Start() { }
	}
}
