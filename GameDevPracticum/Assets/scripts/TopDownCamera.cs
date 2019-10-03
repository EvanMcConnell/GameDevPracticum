﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        //distance from out target
        //bools for zooming and smoothfollowing
        //min and max zoom settings
        public float distanceFromTarget = -50;
        public bool allowZoom = true;
        public float zoomSmooth = 100;
        public float zoomStep = 2;
        public float maxZoom = -30;
        public float minZoom = -60;
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector]
        public float newDistance = -50; //used for smooth zooming - gives us a "destination" zoom
    }

    [System.Serializable]
    public class OrbitSettings
    {
        //holding our current x and y rotation for our camera
        //bool for allowing orbit
        public float xRotation = -65;
        public float yRotation = -180;
        public bool allowOrbit = true;
        public float yOrbitSmooth = 0.5f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string MOUSE_ORBIT = "MouseOrbit";
        public string ZOOM = "Mouse ScrollWheel";
    }

    Vector3 destination = Vector3.zero;
    Vector3 camVelocity = Vector3.zero;
    Vector3 currentMousePosition = Vector3.zero;
    Vector3 previousMousePosition = Vector3.zero;
    float mouseOrbitInput, zoomInput;

    void Start()
    {
        //setting camera target
    }

    public void SetCameraTarget(Transform t)
    {
        //if we want to set a new target at runtime
    }

    void GetInput()
    {
        //filling the values for our input variables
    }

    void Update()
    {
        //getting input
        //zooming
    }

    void FixedUpdate()
    {
        //movetotarget
        //lookattarget
        //orbit
    }

    void MoveToTarget()
    {
        //handeling getting our camera to is destination
    }

    void LookAtTarget()
    {
        //handling getting our camera to look at the target at all times
    }

    void MouseOrbitTarget()
    {
        //getting the camera to orbit around our character
    }

    void ZoomInOnTarget()
    {
        //modifying the distancefromtarget to be closer to or further away from our target
    }
}
