using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    
    public Text nameText;
    public Text dialogueText;

    public GameObject dialogueGUI;
    public Transform dialogueBoxUI;

    public float letterDelay = 0.1f;
    public float letterMultiplier = 0.5f;

    public KeyCode dialogueInput = KeyCode.F;

    public string Names;
    public string[] dialogueLines;

    public bool letterIsMultiplied = false;
    public bool dialogueActive = false;
    public bool dialogueEnded = false;
    public bool outOfRange = true;
    
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
        outOfRange = false;
        dialogueGUI.SetActive(true);
        if(dialogueActive == true)
        {
            dialogueGUI.SetActive(false);
        }
    }

    public void ItemName()
    {
        outOfRange = false;
        dialogueBoxUI.gameObject.SetActive(true);
        nameText.text = Names;
        if(Input.GetKeyDown(KeyCode.F))
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
                    StartCoroutine(DisplayString(dialogueLines[currentDialogueIndex++]));

                    if(currentDialogueIndex >= dialogueLength)
                    {
                        dialogueEnded = true;
                    }
                }
                yield return 0;
            }


            while(true)
            {
                if(Input.GetKeyDown(dialogueInput) && dialogueEnded == false)
                {
                    break;
                }
                yield return 0;
            }
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
