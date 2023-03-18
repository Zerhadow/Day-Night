using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    
    public Text nameText; // Name from ChatBackground Game Object
    public Text dialogueText; // DialogueText from ChatBackground Game Object

    public GameObject dialogueGUI; // ItemPrompt(Canvas) that has "F to chat" (Text) inside
    public Transform dialogueBoxUI; // Contains DialogueGUI (Canvas) from DialogueSystem (Empty)
    public float letterDelay = 0.1f; // Typing Speed
    public float letterMultiplier = 0.5f; // Hold Action button Typing Speed

    public KeyCode dialogueInput = KeyCode.F;

    public string Names;
    public string[] dialogueLines;

    public bool letterIsMultiplied = false; // Text starts out moving at (letterDelay) speed
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true; // Player starts out of range of item
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterRangeOfItem()
    {
        outOfRange = false; // Player is in range of Item
        dialogueGUI.SetActive(true); // Show the prompt for the action key

        // If they have pressed the action key, stop showing the prompt.
        if(dialogueActive == true)
        {
            dialogueGUI.SetActive(false);
        }
    }

    // Called from Item.cs when Player collides with Item and presses action key
    public void ItemName()
    {
        outOfRange = false;
        dialogueBoxUI.gameObject.SetActive(true); // Item's Dialogue now appears on screen
        nameText.text = Names; // Get names from the Item

        if(Input.GetKeyDown(KeyCode.F)) // NOTE!!!!! Maybe remove this if statement since callling this function requires a keycode.f already
        {
            if(!dialogueActive)
            {
                dialogueActive = true;
                StartCoroutine(StartDialogue());
            }
        }
        StartDialogue();
    }

    private IEnumerator StartDialogue()
    {
        if(outOfRange == false)
        {
            int dialogueLength = dialogueLines.Length;
            int currentDialogueIndex = 0;

            while(currentDialogueIndex < dialogueLength || !letterIsMultiplied)
            {
                if(!letterIsMultiplied)
                {
                    letterIsMultiplied = true;
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++])); // Typewriter effect

                    if(currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return 0;
            }


            // Busy wait until player presses the action key to finish reading all dialogue and
            // dialogue box can disappear. 
            while(true)
            {
                if(Input.GetKeyDown(dialogueInput) && dialogueEnded == false)
                {
                    break;
                }
                yield return 0;
            }
            
            // Reset for the next time the dialogue is triggered
            dialogueEnded = false;
            dialogueActive = false;
            DropDialogue();

        }
    }

    private IEnumerator DisplayString(string stringToDisplay)
    {
        if(outOfRange == false)
        {
            int stringLength = stringToDisplay.Length;
            int currentCharacterIndex = 0;

            dialogueText.text = "";

            while(currentCharacterIndex < stringLength)
            {
                dialogueText.text += stringToDisplay[currentCharacterIndex];
                currentCharacterIndex++;

                if(currentCharacterIndex < stringLength)
                {
                    if(Input.GetKey(dialogueInput))
                    {
                        yield return new WaitForSeconds(letterDelay * letterMultiplier);
                    }

                    else
                    {
                        yield return new WaitForSeconds(letterDelay);
                    }
                }

                else
                {
                    dialogueEnded = false;
                    break;
                }

            }

            while(true)
            {
                if(Input.GetKeyDown(dialogueInput))
                {
                    break;
                }
                yield return 0;
            }
            dialogueEnded = false;
            letterIsMultiplied = false;
            dialogueText.text = "";

        }
    }

    public void DropDialogue()
    {
        dialogueGUI.SetActive(false);
        dialogueBoxUI.gameObject.SetActive(false);
    }

    public void OutOfRange()
    {
        outOfRange = true;
        if(outOfRange == true)
        {
            letterIsMultiplied = false;
            dialogueActive = false;
            StopAllCoroutines();
            dialogueGUI.SetActive(false);
            dialogueBoxUI.gameObject.SetActive(false);
        }
    }



}
