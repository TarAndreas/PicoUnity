using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class HandController : MonoBehaviour
{
    [SerializeField] private ActionBasedController controller_leftHand;
    [SerializeField] private ActionBasedController controller_rightHand;

    [SerializeField] private Animator animator;

    private float triggerTarget;
    private float triggerCurrent;
    private float speed = 10;
    //private string animatorGripParam = "Grip";
    private string animatorTriggerParam = "TriggerLeft";

    public bool isHandContollerSetupFinish;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }




    // Update is called once per frame
    void Update()
    {
        //if (isHandContollerSetupFinish)
        //{





        //    if (triggerCurrent != triggerTarget)
        //    {

        //        Debug.Log("triggerCurrent" + triggerCurrent);

        //        triggerCurrent = controller_leftHand.activateActionValue.action.ReadValue<float>();

        //        triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
        //        animator.SetFloat(animatorTriggerParam, triggerCurrent);

        //        Debug.Log(animator.GetFloat(animatorTriggerParam));
        //    }

        //}

    }




    public void SetHandReferences(GameObject gameObject)
    {

        isHandContollerSetupFinish = true;

    }
}
