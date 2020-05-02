using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



/*
 * que tienen las activas?
 * cooldown
 * tiempo de uso
 * Hover si te paras encima de informacion
 * - nombre
 * - descripcion lore
 * - descripcion tecnica
 * 
*/


public class UI3D_Element : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    FeedbackInteractBase[] feedbacks;
    public FeedbackOneInteractBase[] feedbackOnSelect;
    public FeedbackOneInteractBase[] feedbackOnEndLoad;
    public Image img_to_fill;
    public ParticleSystem part_end_load;
    SkillInfo mySkillInfo;

    private void Awake()
    {
        feedbacks = GetComponentsInChildren<FeedbackInteractBase>();
        
    }

    public void SetSkillInfo(SkillInfo skillInfo) => mySkillInfo = skillInfo;
    public void SetCooldow(float val) => img_to_fill.fillAmount = val;
    public void SkillLoaded() => part_end_load.Play();
    public void SetBlocked() { }

    public void OnSelect(BaseEventData eventData) { foreach (var f in feedbackOnSelect) f.Execute(); }
    public void OnDeselect(BaseEventData eventData) { Debug.Log("OnSelect"); }
    public void OnPointerEnter(PointerEventData eventData) 
    {
        foreach (var f in feedbacks) f.Show();
    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        foreach (var f in feedbacks) f.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var f in feedbackOnSelect) f.Execute();
    }
}
