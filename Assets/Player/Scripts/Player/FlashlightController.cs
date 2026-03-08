using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private Light[] _lights;
    [Space(16)]
    [SerializeField] private float _dischargeSpeed;
    [SerializeField] private float _switchSpeed;
    [SerializeField] private float _intensity;

    private float _charge;

    private bool _isOn;

    private bool _isSwitching;

    private InputAction _flashlightAction;

    private void Start()
    {
        _flashlightAction = InputSystem.actions.FindAction("Flashlight");

        _charge = 100.0f;

        foreach (Light light in _lights)
        {
            light.intensity = 0;
        }
    }

    private void Update()
    {
        if (_isOn)
        {
            _charge -= _dischargeSpeed * Time.deltaTime;
        }

        if (_flashlightAction.WasPressedThisFrame() && !_isSwitching)
        {
            if (_isOn)
            {
                TurnOff();
                _player.playerAudioController.PlayFlashlightSound();
            }
            else
            {
                if (_charge > 0)
                {
                    TurnOn();
                }
                _player.playerAudioController.PlayFlashlightSound();
            }
        }

        if (_isOn && _charge <= 0)
        {
            _charge = 0;
            TurnOff();
        }
    }

    private void TurnOn()
    {
        Sequence turnOnSequence = DOTween.Sequence();
        turnOnSequence.SetEase(Ease.InOutSine);
        turnOnSequence.Pause();

        turnOnSequence.AppendCallback(() => _isSwitching = true);
        foreach (Light light in _lights)
        {
            turnOnSequence.Join(light.DOIntensity(_intensity, _switchSpeed));
        }
        turnOnSequence.AppendCallback(() => { _isOn = true; _isSwitching = false; });

        turnOnSequence.Play();
    }

    private void TurnOff()
    {
        Sequence turnOnSequence = DOTween.Sequence();
        turnOnSequence.SetEase(Ease.InOutSine);
        turnOnSequence.Pause();

        turnOnSequence.AppendCallback(() => _isSwitching = true);
        foreach (Light light in _lights)
        {
            turnOnSequence.Join(light.DOIntensity(0, _switchSpeed));
        }
        turnOnSequence.AppendCallback(() => { _isOn = false; _isSwitching = false; });

        turnOnSequence.Play();
    }
}
