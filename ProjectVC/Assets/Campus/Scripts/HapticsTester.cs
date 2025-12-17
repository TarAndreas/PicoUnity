using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticsTester : MonoBehaviour
{
    //public  XRBaseController leftController, rightController;
    public XRBaseController mainController;
    public float defaultAmplitude = .2f;
    public float defaultDuration = .5f;
    //[ContextMenu(itemName: "Send Haptics")]
    public void SendHaptics()
    {
        //leftController.SendHapticImpulse(defaultAmplitude, defaultDuration);
        //rightController.SendHapticImpulse(defaultAmplitude, defaultDuration);
        mainController.SendHapticImpulse(defaultAmplitude, defaultDuration);
    }
    //    public static void SendHaptics(float amplitude, float duration)
    //    {
    //        leftController.SendHapticImpulse(amplitude, duration);
    //        rightController.SendHapticImpulse(amplitude, duration);

    //    }
    //    public static void SendHaptics(bool isLeftController, float amplitude, float duration)
    //    {
    //        if (isLeftController)
    //        {
    //            leftController.SendHapticImpulse(amplitude, duration);
    //        }
    //        else
    //        {
    //            rightController.SendHapticImpulse(amplitude, duration);
    //        }

    //    }
    //    public void SendHaptics(XRBaseController controller, float amplitude, float duration)
    //    {
    //        controller.SendHapticImpulse(amplitude, duration);
    //    }
    private void OnCollisionEnter(Collision collision)
    {
        //SendHaptics();
        Debug.Log("collision Stadtgefunden");
    }
    private void OnCollisionStay(Collision collision)
    {
        //SendHaptics();
        Debug.Log("collision Stadtgefunden");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Haptic")
        {
            mainController.SendHapticImpulse(defaultAmplitude, defaultDuration);
        }
        /*
        if (other.tag == "HapticRightHand")
        {
            rightController.SendHapticImpulse(defaultAmplitude, defaultDuration);
        }
        */
        //if (other.name == "RightHand Controller")
        //{
        //    rightController.SendHapticImpulse(defaultAmplitude, defaultDuration);
        //}
        //SendHaptics();
        Debug.Log("OnTriggerEnter Stadtgefunden");
        Debug.Log(other.name);       
    }

}
//class NewClass
//{
//    //public void shoot()
//    //{
//    //    HapticsController.SendHaptics(amplitude: .1f, duration: .7f);
//    //}
//}