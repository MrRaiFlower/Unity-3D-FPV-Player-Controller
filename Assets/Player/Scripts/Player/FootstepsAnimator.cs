using DG.Tweening;
using UnityEngine;

public class FootstepsAnimator : MonoBehaviour
{
    [SerializeField] private Player _player;
    [Space(16)]
    [SerializeField] private float _depth;
    [SerializeField] private float _speed;

    private bool _isStepping;

    private float _leanDirection;

    private void Start()
    {
        _leanDirection = 1.0f;
    }

    private void Update()
    {
        if (!_isStepping && _player.movementController.isMoving && _player.movementController.isGrounded)
        {
            Step(-_depth * _player.movementController.speedRatio, _speed / _player.movementController.speedRatio);
        }
    }

    private void Step(float targetDepth, float actualSpeed)
    {
        Sequence stepSequence = DOTween.Sequence();
        stepSequence.SetEase(Ease.InOutSine);
        stepSequence.Pause();

        stepSequence.AppendCallback(() => _isStepping = true);

        stepSequence.Append(_player.cameraTransform.DOLocalMoveY(targetDepth, actualSpeed / 2));
        stepSequence.Join(_player.cameraTransform.DOLocalMoveX(targetDepth / 2 * _leanDirection, actualSpeed / 2));

        stepSequence.AppendCallback(() => _player.playerAudioController.PlayFootstepSound());

        stepSequence.Append(_player.cameraTransform.DOLocalMoveY(0, actualSpeed / 2));
        stepSequence.Join(_player.cameraTransform.DOLocalMoveX(0, actualSpeed / 2));

        stepSequence.AppendCallback(() => _leanDirection *= -1);

        stepSequence.AppendCallback(() => _isStepping = false);

        stepSequence.Play();
    }
}
