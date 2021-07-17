using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuSFX.instance.PlaySelectSound();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        MenuSFX.instance.PlayClickSound();
    }
}
