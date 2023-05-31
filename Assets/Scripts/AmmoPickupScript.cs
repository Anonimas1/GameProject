using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AmmoPickupScript : MonoBehaviour
{
    [FormerlySerializedAs("slider")]
    [Tooltip("Default value is from child game object")]
    public TMP_Text Text;
    public float positionUpdateRate = 0.1f;

    public float moveDistance = 1;
    public float moveTime = 1;
    public float fade = 0.2f;


    private float timePassed = 0f;
    private Transform _mainCameraTransform;

    void Awake()
    {
        _mainCameraTransform = GameObject.FindWithTag("MainCamera").transform;
    }

    private void OnEnable()
    {
        StartCoroutine(PointAtCamera());
        StartCoroutine(FadeAndMove());
        Destroy(gameObject, moveTime + 0.5f);
    }
    

    private IEnumerator FadeAndMove()
    {
        while (moveTime > timePassed)
        {
            yield return new WaitForSeconds(positionUpdateRate);
            Text.alpha -= fade;
            var pos = transform.position;
            pos.y += moveDistance;
            transform.position = pos;
        }
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
    }

    private IEnumerator PointAtCamera()
    {
        while (true)
        {
            gameObject.transform.LookAt(_mainCameraTransform);
            yield return new WaitForSeconds(positionUpdateRate);   
        }
    }
}