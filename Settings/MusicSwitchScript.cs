using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**************************************************************
 * Made for Music switch button prefab
 **************************************************************/
public class MusicSwitchScript : MonoBehaviour
{
    private MusicScript ms;
    // Start is called before the first frame update
    void Start()
    {
        ms = GameObject.FindObjectOfType<MusicScript>();
    }

    public void MusicSwitcher() {
        ms.SwitchMusic();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
