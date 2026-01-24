using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private Light[] lights;

    [SerializeField] private PlayerSound playerSoundscript;

    [SerializeField] private float maxCharge;
    [SerializeField] private float dischargeSpeed;
    [SerializeField] private float switchSpeed;
    [SerializeField] private float intensity;

    private float charge;

    private bool isOn;

    private bool isSwitching;

    private InputAction flashlightAction;

    private void Start()
    {
        flashlightAction = InputSystem.actions.FindAction("Flashlight");

        charge = maxCharge;

        foreach (Light light in lights)
        {
           light.intensity = 0;
        }
    }

    private void Update()
    {
        if (isOn)
        {
            charge -= dischargeSpeed * Time.deltaTime;
        }

        if (flashlightAction.WasPressedThisFrame() && !isSwitching)
        {
            if (isOn)
            {
                TurnOff();
                playerSoundscript.PlayFlashlightSound();
            }
            else
            {
                if (charge > 0)
                {
                    TurnOn();
                }
                playerSoundscript.PlayFlashlightSound();
            }
        }

        if (isOn && charge <= 0)
        {
            charge = 0;
            TurnOff();
        }
    }

    private void TurnOn()
    {
        Sequence turnOnSequence = DOTween.Sequence();
        turnOnSequence.SetEase(Ease.InOutSine);
        turnOnSequence.Pause();

        turnOnSequence.AppendCallback(() => {isSwitching = true;});
        foreach (Light light in lights)
        {
            turnOnSequence.Join(light.DOIntensity(intensity, switchSpeed));
        }
        turnOnSequence.AppendCallback(() => {isOn = true;});
        turnOnSequence.AppendCallback(() => {isSwitching = false;});

        turnOnSequence.Play();
    }

    private void TurnOff()
    {
        Sequence turnOnSequence = DOTween.Sequence();
        turnOnSequence.SetEase(Ease.InOutSine);
        turnOnSequence.Pause();

        turnOnSequence.AppendCallback(() => {isSwitching = true;});
        foreach (Light light in lights)
        {
            turnOnSequence.Join(light.DOIntensity(0, switchSpeed));
        }
        turnOnSequence.AppendCallback(() => {isOn = false;});
        turnOnSequence.AppendCallback(() => {isSwitching = false;});

        turnOnSequence.Play();
    }
}
