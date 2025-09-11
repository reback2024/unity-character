using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")]
    public Text textLabel;
    public Image faceImage;
    [Header("文本")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;
    bool textFinish;
    bool cancelTyping;
    [Header("头像")]
    public Sprite face01,face02;

    List<string> textList=new List<string>();
    private void Awake()
    {
        GetTextFormFile(textFile);
    }
    private void OnEnable()
    {
        textFinish = true;
        StartCoroutine(SetTextUI());
        //textLabel.text = textList[index++];
    }
    
   
    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.R)&&textFinish)
        //{
        //    //   textLabel.text = textList[index++];
        //    StartCoroutine(SetTextUI());
        //    if (index >= textList.Count)
        //    {
        //        gameObject.SetActive(false); 
        //        index = 0;
        //        textFinish = true;
        //        return;
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (textFinish && !cancelTyping)
            {
                 StartCoroutine(SetTextUI());
                if (index >= textList.Count)
                {
                    gameObject.SetActive(false);
                    index = 0;
                    textFinish = true;
                    Player.Instance.CanMove();
                    return;
                }
            }
            else if (!textFinish)
            {
                cancelTyping=!cancelTyping;
            }
        }
    }
    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;
        var lineDate =file.text.Split('\n');
        foreach(var line in lineDate)
        {
            textList.Add(line);
        }
    }
    IEnumerator SetTextUI()
    {
        textFinish = false;
        textLabel.text = "";
        switch (textList[index])
        {
            case "A":
                faceImage.sprite = face01;
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                index++;
                break;
        }

        //for (int i = 0; i < textList[index].Length; i++)
        //{
        //    textLabel.text += textList[index][i];
        //    yield return new WaitForSeconds(textSpeed);
        //}
        int letter = 0;
        while (!cancelTyping && letter < textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        textFinish = true;
        cancelTyping = false;
        index++;
    }
}
