using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day : MonoBehaviour
{
    public static Day instance {  get; private set; }
    public int day = 1;

    private void Awake()
    {
        if(instance == null)instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void addday()
    {
        day++;
        //Debug.Log(day);
    }
}
