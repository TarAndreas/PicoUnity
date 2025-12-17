/*
 * Show a list of connected VR/Hardware devices
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeviceCheck : MonoBehaviour
{

    private InputDeviceCharacteristics controllerCharacteristics;

    public void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count == 0)
        {

            Debug.Log("No Devices found");
        }
    }
}
