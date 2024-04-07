using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scp_Manager_FPS : MonoBehaviour
{
    public int frameTarget;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = frameTarget;
    }
}
