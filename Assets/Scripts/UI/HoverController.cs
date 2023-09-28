using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverController : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{

    public GameObject hoverObject;
    public GameObject originalObject;
    //private Sprite originalSprite;
    //[SerializeField] private Sprite spriteTexture;

    public void OnPointerEnter(PointerEventData eventData)
    {
        originalObject.SetActive(false);
        hoverObject.SetActive(true);
        //hoverObject.GetComponent<SpriteRenderer>().sprite = spriteTexture;
        Debug.Log("Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        originalObject.SetActive(true);
        hoverObject.SetActive(false);
        //hoverObject.GetComponent<SpriteRenderer>().sprite = originalSprite;
        Debug.Log("Exit");
    }

    void Start()
    {
       //originalSprite = hoverObject.GetComponent<SpriteRenderer>().sprite;
    }
}
