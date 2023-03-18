using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour
{
    
    public Transform ChatBackground; // Image of the Dialogue Box
    public Transform item; // Object that Player interacts with

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
        Vector3 position = Camera.main.WorldToScreenPoint(item.position); // Turns item's 3D position into a 2D pixelized position.
        position.y += 170; // Places DialogueBox 170 units above the center of the item
        ChatBackground.position = position;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<Item>().enabled = true; // Item(Script) is disabled at first in the case of being in the presence of multiple items

        FindObjectOfType<DialogueSystem>().EnterRangeOfItem();
        if((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<Item>().enabled = true;
            dialogueSystem.Names = Name; // Name that appears in DialogueBox is set
            dialogueSystem.dialogueLines = sentences; // DialogueText that appears in DialogueBox is set
            FindObjectOfType<DialogueSystem>().ItemName();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<Item>().enabled = false; // Item(Script) is disabled once again
    }


}
