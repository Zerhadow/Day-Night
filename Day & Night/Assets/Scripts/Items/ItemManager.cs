using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject[] itemSets;

    GameObject currentSet;

    int numSets= 0;
    int setIndex;
    // Start is called before the first frame update
    void Start()
    {
        numSets = itemSets.Length;
    }

    public void spawn()
    {
        if (currentSet != null)
            Destroy(currentSet);

        if (setIndex < numSets)
            currentSet = Instantiate(itemSets[setIndex]);
    }

    public void despawn()
    {
        if (currentSet != null)
            Destroy(currentSet);
    }

    public void setSpawnSet(int index)
    {
        if (index < numSets && index > -1)
            setIndex = index;
    }
}
