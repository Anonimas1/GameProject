using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [Tooltip("Default value is from parent game object")]
    public Damagable damagable;
    [Tooltip("Default value is from child game object")]
    public Slider slider;
    [Description("Health bar slider position update rate in seconds")]
    public float positionUpdateRate = 0.1f;

    private Transform mainCameraTransform;
    void Awake()
    {
        damagable ??= gameObject.GetComponentInParent<Damagable>();
        damagable.CurrentHealthChangedEventHandler += OnHealthChanged;
        slider ??= gameObject.GetComponentInChildren<Slider>();
        
        slider.maxValue = damagable.MaxHealth;
        slider.value = damagable.CurrentHealth;
        mainCameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        StartCoroutine(PointAtCamera());
    }

    private void OnHealthChanged(object sender, int value)
    {
        slider.value = value;
    }
    
    private IEnumerator PointAtCamera()
    {
        slider.transform.LookAt(mainCameraTransform);
        yield return new WaitForSeconds(positionUpdateRate);
    }

    private void OnDestroy()
    {
        damagable.CurrentHealthChangedEventHandler -= OnHealthChanged;
    }
}
