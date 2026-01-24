using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    [SerializeField] private GameObject cameraObject;

    [SerializeField] private Movement movementScript;
    [SerializeField] private PlayerSound playerSoundscript;

    [SerializeField] private float depth;
    [SerializeField] private float speed;

    private bool isMakingAStep;

    private float targetDepth;
    private float actualSpeed;
    private float leanDirection;

    private void Start()
    {
        leanDirection = 1.0f;
    }

    private void Update()
    {
        if (!isMakingAStep && movementScript.isMoving && movementScript.isGrounded)
        {
            MakeAStep();
        }
    }

    private void MakeAStep()
    {
        float movementSpeedRatio = movementScript.GetSpeedRatio();

        targetDepth = -depth * movementSpeedRatio;
        actualSpeed = speed / movementSpeedRatio;

        Sequence stepSequence = DOTween.Sequence();
        stepSequence.SetEase(Ease.InOutSine);
        stepSequence.Pause();

        stepSequence.AppendCallback(() => {isMakingAStep = true;});

        stepSequence.Append(cameraObject.transform.DOLocalMoveY(targetDepth, actualSpeed / 2));
        stepSequence.Join(cameraObject.transform.DOLocalMoveX(targetDepth / 2 * leanDirection, actualSpeed / 2));

        stepSequence.AppendCallback(() => {playerSoundscript.PlayFootstepSound(movementSpeedRatio > 1 ? 1f : (movementSpeedRatio < 1 ? 0.5f : 0.75f));});

        stepSequence.Append(cameraObject.transform.DOLocalMoveY(0, actualSpeed / 2));
        stepSequence.Join(cameraObject.transform.DOLocalMoveX(0, actualSpeed / 2));

        stepSequence.AppendCallback(() => {leanDirection *= -1;});

        stepSequence.AppendCallback(() => {isMakingAStep = false;});

        stepSequence.Play();
    }
}
