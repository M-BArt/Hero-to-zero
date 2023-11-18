using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    public void SetActiveSkill()
    {
        _image.color = Color.yellow;
    }
    public void ResetActiveSkill()
    {
        _image.color = Color.white;
    }
}
