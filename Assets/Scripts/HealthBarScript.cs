using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [FormerlySerializedAs("damagable")]
    [Tooltip("Default value is from parent game object")]
    public Damageable damageable;
    [Tooltip("Default value is from child game object")]
    public Slider slider;
    [Description("Health bar slider position update rate in seconds")]
    public float positionUpdateRate = 0.1f;

    private Transform _mainCameraTransform;
    void Awake()
    {
        damageable ??= gameObject.GetComponentInParent<Damageable>();
        damageable.CurrentHealthChangedEventHandler += OnHealthChanged;
        slider ??= gameObject.GetComponentInChildren<Slider>();
        
        slider.maxValue = damageable.MaxHealth;
        slider.value = damageable.CurrentHealth;
        gameObject.SetActive(false);
        _mainCameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            slider.value = damageable.CurrentHealth;
        }
        StartCoroutine(PointAtCamera());
    }

    private void OnHealthChanged(object sender, int value)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);   
        }
        
        slider.value = value;
    }
    
    private IEnumerator PointAtCamera()
    {
        slider.transform.LookAt(_mainCameraTransform);
        yield return new WaitForSeconds(positionUpdateRate);
    }

    private void OnDestroy()
    {
        damageable.CurrentHealthChangedEventHandler -= OnHealthChanged;
    }
}
