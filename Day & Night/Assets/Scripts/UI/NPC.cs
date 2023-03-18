using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NPC : MonoBehaviour
{
    
    public Transform ChatBackground; // Image of the Dialogue Box
    public Transform npc; // Object that Player interacts with

    private DialogueSystem dialogueSystem; // 

    public string Name;

    [TextArea(5, 10)]
    public string[] sentences;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(npc.position); // Turns npc's 3D position into a 2D pixelized position.
        position.y += 250; // Places DialogueBox 170 units above the center of the npc
        ChatBackground.position = position;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<NPC>().enabled = true; // NPC(Script) is disabled at first in the case of being in the presence of multiple npcs

        FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
        if((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Hit NPC.OnTriggerStay()");
            this.gameObject.GetComponent<NPC>().enabled = true;
            dialogueSystem.Names = Name; // Name that appears in DialogueBox is set
            dialogueSystem.dialogueLines = sentences; // DialogueText that appears in DialogueBox is set
            FindObjectOfType<DialogueSystem>().NPCName();
        }
    }

    public void OnTriggerExit()
    {
        Debug.Log("Hit NPC.OnTriggerExit()");
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<NPC>().enabled = false; // NPC(Script) is disabled once again
    }


}
