using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    [SerializeField] private GameObject customImage;
    
    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f, 0f));
        RaycastHit hit; 
        if(Physics.Raycast(ray, out hit, LayerMask.GetMask("Item")))
        {
            var selection = hit.transform;
            var selectionRenderer = selection.GetComponent<Renderer>();
            if(selectionRenderer != null)
            {
                customImage.SetActive(true);
            }
        }

        else
        {
            customImage.SetActive(false);
        }
    }
}
