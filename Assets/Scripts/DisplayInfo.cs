using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DisplayInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject text;
    private bool display = false;
    private void Update()
    {
        text.SetActive(display);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        display = true;
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        display = false;
        throw new System.NotImplementedException();
    }
}
