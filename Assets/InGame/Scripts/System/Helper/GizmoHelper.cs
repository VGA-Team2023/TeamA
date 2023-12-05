//日本語対応
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoHelper : MonoBehaviour
{
    private static bool _isCast = false;
    private static Vector3 _transform;
    private static Vector3 _direction;
    private static float _scale;
    private RaycastHit2D _hit;
    private static float _maxDistance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void OnDrawBox(Vector3 mySelf, Vector3 direction, float scale, float maxDistance, RaycastHit2D hit)
    {
        _transform = mySelf;
        _direction = direction;
        _maxDistance = maxDistance;
        _scale = scale;
        _isCast = true;
    }

    public static void StopDrawGizmos()
    {
        _isCast = false;
    }

    private void OnDrawGizmos()
    {
        if (!_isCast || _transform == Vector3.zero) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_transform, _direction * _hit.distance);
        Gizmos.DrawWireCube(_transform + _direction * _hit.distance, Vector3.one * _scale);
    }

    public class DrawBoxData
    {
        private readonly Vector3 MySelf = default;
        private readonly Vector3 Direction = default;
        private readonly float Scale = 0f;
        private readonly float MaxDistance = 0f;
        private readonly RaycastHit2D Hit = default;

        public DrawBoxData(Vector3 mySelf, Vector3 direction, float scale, float max, RaycastHit2D hit)
        {
            MySelf = mySelf;
            Direction = direction;
            Scale = scale;
            MaxDistance = max;
            Hit = hit;
        }
    }
}
