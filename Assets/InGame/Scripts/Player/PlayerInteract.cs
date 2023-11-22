//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerInteract : IPlayerState
{
    [Header("boxの大きさ")]
    [SerializeField] private float _boxScale;
    [Header("判定の距離")]
    [SerializeField] private float _maxDistance = 10;
    [SerializeField] private LayerMask _layerMask;
    private PlayerEnvroment _env;

    public void SetUp(PlayerEnvroment env, CancellationToken token)
    {
        _env = env;
        InputProvider.Instance.SetEnterInput(InputProvider.InputType.Interact, Interact);
    }

    public void Update()
    {

    }

    public void FixedUpdate()
    {

    }

    public void Interact()
    {
        if (_env.PlayerState.HasFlag(PlayerStateType.Damage) ||
            _env.PlayerState.HasFlag(PlayerStateType.Inoperable)) return;
        Debug.Log("きた");
        var isHit = Physics2D.BoxCast(_env.PlayerTransform.position + (Vector3)(_env.LastDir * 2), Vector2.one * _boxScale, 0, _env.LastDir, _maxDistance, _layerMask);
        GizmoHelper.OnDrawBox(_env.PlayerTransform.position + (Vector3)(_env.LastDir * 2), _env.LastDir, _boxScale, _maxDistance, isHit);
        if (isHit.collider != null)
        {
            Debug.Log("当たってます");
        }
        if (isHit.collider != null && isHit.collider.TryGetComponent<IInteractEvent>(out var interactEvent))
        {
            Debug.Log(isHit);
            interactEvent.Execute();
        }
    }

    public void Dispose()
    {
        InputProvider.Instance.LiftEnterInput(InputProvider.InputType.Interact, Interact);
    }

}
