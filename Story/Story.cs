using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : MonoBehaviour
{
    private StoryManager curday;//当前应该对应哪一天的脚本

    [HideInInspector]public int d;

    private Dictionary<int, StoryManager> states = new Dictionary<int, StoryManager>();

    private void Awake()
    {
        d = Day.instance.day;

        states.Add(1, new Day1(this));
        states.Add(2, new Day2(this));
        states.Add(3, new Day3(this));
        states.Add(4, new Day4(this));
        states.Add(5, new Day5(this));
        states.Add(6, new Day6(this));
        states.Add(7, new Day7(this));

        Transitionday(d);
    }

    //用于切换状态的函数
    public void Transitionday(int d)
    {
        if (curday != null)
        {
            curday.OnExit();
        }
        curday = states[d];
        curday.OnEnter();
    }

    private void Update()
    {
        curday.OnUpdate();
    }

    private void FixedUpdate()
    {
        curday.OnFixedUpdate();
    }
}
