using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LookatPlayer : MonoBehaviour
{
    [SerializeField] Transform m_TargetObj;
    [SerializeField] float m_DistanceAway;//摄像机与目标距离
    [SerializeField] float m_DistanceUp;//摄像机与目标高度
    [SerializeField] float m_smooth; // how smooth the camera movement is

    private void LateUpdate()
    {
        var targetPos = m_TargetObj.position + Vector3.up * m_DistanceUp - m_TargetObj.forward * m_DistanceAway;

        // making a smooth transition between it's current position and the position it wants to be in
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * m_smooth);

        // make sure the camera is looking the right way!
        transform.LookAt(m_TargetObj);

    }
}
