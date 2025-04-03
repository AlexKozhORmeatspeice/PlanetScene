using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

public interface IPointerManager
{
    event Action OnUpdate;
    event Action OnPointerMove;
    event Action OnPointerUp;
    event Action OnPointerDown;
    event Action OnDoubleTap;

    Vector2 Speed { get; }
    Vector2 NowScreenPosition { get; }
    Vector2 LastScreenClickPosition { get; }

    Vector2 NowWorldPosition { get; }
    Vector2 LastWorldClickPosition { get; }

    public bool IsDown { get; }
}

public class MouseManager : IPointerManager, ITickable
{
    private Camera _camera;
    private Vector3 lastClickPos;
    private bool IsMouseDown;

    private const float tapThreshold = 0.25f;
    private float tapTimer = 0.0f;
    private bool tap = false;

    private Vector2 speed;
    private Vector2 lastFrameMousePos;
    private float timeElapsedFromLastFrame;

    public event Action OnPointerMove;
    public event Action OnPointerUp;
    public event Action OnPointerDown;
    public event Action OnDoubleTap;
    public event Action OnUpdate;

    public Vector2 NowScreenPosition => Input.mousePosition;

    public Vector2 LastScreenClickPosition => lastClickPos;

    public bool IsDown => IsMouseDown;

    public Vector2 NowWorldPosition => _camera.ScreenToWorldPoint(Input.mousePosition);

    public Vector2 LastWorldClickPosition => _camera.ScreenToWorldPoint(lastClickPos);

    public Vector2 Speed => speed;


    [Inject]
    public void Construct()
    {
        speed = Vector2.zero;
        lastClickPos = Vector3.zero;
        lastFrameMousePos = Input.mousePosition;
        IsMouseDown = false;

        _camera = Camera.main;
    }

    public void Tick()
    {
        CalculateSpeed();
        CheckEvents();

        OnUpdate?.Invoke();
    }

    private void CalculateSpeed()
    {
        Vector2 nowMousePos = Input.mousePosition;
        speed = (nowMousePos - lastFrameMousePos).Abs() / Time.deltaTime;

        lastFrameMousePos = nowMousePos;
    }

    private void CheckEvents()
    {
        if (Math.Abs(Input.GetAxis("Mouse X")) > 0.0f || Math.Abs(Input.GetAxis("Mouse Y")) > 0.0f)
        {
            OnPointerMove?.Invoke();
        }

        if (IsMouseDown && Input.GetMouseButtonUp(0))
        {
            IsMouseDown = false;
            OnPointerUp?.Invoke();
        }

        if (!IsMouseDown && Input.GetMouseButtonDown(0))
        {
            lastClickPos = Input.mousePosition;
            IsMouseDown = true;

            if (Time.time < tapTimer + tapThreshold)
            {
                tap = false;

                OnDoubleTap?.Invoke();
                return;
            }

            tap = true;
            tapTimer = Time.time;
        }

        if (!IsMouseDown && tap == true && Time.time > tapTimer + tapThreshold)
        {
            tap = false;

            OnPointerDown?.Invoke();
        }
    }
}
