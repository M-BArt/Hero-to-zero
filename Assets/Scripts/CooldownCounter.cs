using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownCounter : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private float _cooldown = 0;
    private float nextReadyTime;
    private float remainingTime = 0f;
    private bool _isCounting;
    private Image _image;


    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _isCounting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCounting)
        {
            _text.enabled = true;
            _image.enabled = true;
            UpdateCooldownUI();
        }
        else
        {
            _text.enabled = false;
            _image.enabled = false;
        }
    }
    void UpdateCooldownUI()
    {
        remainingTime = Mathf.Max(0, nextReadyTime - Time.time);
        _text.text = remainingTime.ToString("F1");
        if (remainingTime <= 0)
        {
            _isCounting = false;
        }
    }
    public void StartCounting()
    {
        _isCounting = true;
    }
    public void SetCooldown(float cooldown)
    {
        _cooldown = cooldown;
        nextReadyTime = Time.time + _cooldown;
    }
}
