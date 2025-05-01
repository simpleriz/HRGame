using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ImageChange : MonoBehaviour
{
    public Image targetImage;
    public Sprite newSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        targetImage.sprite = newSprite;
    }
}
