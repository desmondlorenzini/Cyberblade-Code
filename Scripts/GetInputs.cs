using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class GetInputs : MonoBehaviour
{
    public InputDeviceCharacteristics controller;
    public bool isLeftController;
    public XRInteractorLineVisual xrRay;

    private InputDevice targetDevice;
    private bool UISelected;

    private void Start()
    {
        GetDevice();
    }

    private void Update()
    {
        if ( xrRay.reticle.activeInHierarchy )
        {
            Inputs.leftTrigger = 0;
            Inputs.rightTrigger = 0;

            UISelected = true;
        }

        else
        {
            UISelected = false;
        }

        if ( !targetDevice.isValid )
        {
            GetDevice();

            return;
        }

        if ( targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float t) )
        {
            if ( UISelected )
            {
                return;
            }

            if ( isLeftController )
            {
                Inputs.leftTrigger = t;
            }

            else
            {
                Inputs.rightTrigger = t;
            }
        }

        if ( targetDevice.TryGetFeatureValue(CommonUsages.grip, out float g) )
        {
            if ( isLeftController )
            {
                Inputs.leftGrip = g;
            }

            else
            {
                Inputs.rightGrip = g;
            }
        }

        if ( targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool b) )
        {
            if ( isLeftController )
            {
                Inputs.xButtonPressed = b;
            }

            else
            {
                Inputs.aButtonPressed = b;
            }
        }
        
        if ( targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool n) )
        {
            if ( isLeftController )
            {
                Inputs.yButtonPressed = n;
            }

            else
            {
                Inputs.bButtonPressed = n;
            } 
        }
    }
    void GetDevice()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controller, devices);

        if ( devices.Count > 0 )
        {
            targetDevice = devices[0];
        }
    }
}
