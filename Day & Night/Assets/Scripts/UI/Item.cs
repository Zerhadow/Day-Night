using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour
{
    
    public Transform ChatBackground;
    public Transform item;

    private DialogueSystem dialogueSystem;

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
        Vector3 position = Camera.main.WorldToScreenPoint(item.position);
        position.y += 50;
        ChatBackground.position = position;
    }

    public void OnTriggerStay(Collider other)
    {
        this.gameObject.GetComponent<Item>().enabled = true;
        FindObjectOfType<DialogueSystem>().EnterRangeOfItem();
        if((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
        {
            this.gameObject.GetComponent<Item>().enabled = true;
            dialogueSystem.Names = Name;
            dialogueSystem.dialogueLines = sentences;
            FindObjectOfType<DialogueSystem>().ItemName();
        }
    }

    public void OnTriggerExit()
    {
        FindObjectOfType<DialogueSystem>().OutOfRange();
        this.gameObject.GetComponent<Item>().enabled = false;
    }


}
