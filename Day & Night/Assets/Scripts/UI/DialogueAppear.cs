using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAppear : MonoBehaviour
{
    // Makes customImage visible in inspector without making it a public variable.
    [SerializeField] private GameObject dialogueImage; 


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dialogueImage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dialogueImage.SetActive(false);
        }
    }


}
