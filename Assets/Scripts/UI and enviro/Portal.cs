using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public static Portal instance;
    public Material openMat;
    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }

    public void OpenPOrtal()
    {
        isOpen = true;
        GetComponent<MeshRenderer>().material = openMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen)
        {
            GameStateMessages.instance.ShowLevelComplete();
        }
        //finishGame
    }

}

