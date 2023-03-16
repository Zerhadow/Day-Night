using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRaycastSelect : RaycastSelect
{
    [SerializeField] private DialogueTrigger trig;

    protected override void OnRaycastEnter(GameObject target)
    {
        trig = target.GetComponent<DialogueTrigger>();
        trig.TriggerDialogue();
    }
}
