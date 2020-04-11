using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Skill_Btt_template : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    private SkillInfo allocatedSkill;
    [SerializeField] private GameObject selectedImage;

    private Action<SkillInfo> OnSelected;

    public void SetData(SkillInfo skill, Action<SkillInfo> OnSelected)
    {
        allocatedSkill = skill;
        this.OnSelected = OnSelected;

        GetComponent<Button>().image.sprite = allocatedSkill.img_actived;
        
        GetComponent<Button>().onClick.AddListener(SelectSkill);
    }

    private void SelectSkill()
    {
        OnSelected(allocatedSkill);
    }

    ////////////////HOVER
    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("caca Hover Jopystick o teclado");
        selectedImage.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("caca Hover mouse");
        selectedImage.SetActive(true);
    }

    ////////////Selecion ACEPTAR
    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("caca en el joystick");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("caca en el Mouse");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        selectedImage.SetActive(false);
    }
}
