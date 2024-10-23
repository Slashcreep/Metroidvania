using System.Collections;
using System.Collections.Generic;
using TarodevController;
using UnityEngine;
using Cinemachine;

public class RightLeftCamera : MonoBehaviour
{
    private float currentOffset;
    [SerializeField] float increaseSpeed = 0.5f;
    [SerializeField] float rightOffset = 0.2f;
    [SerializeField] float leftOffset = -0.2f;
    [SerializeField] GameObject player;
    private CinemachineVirtualCamera thisCamera;

    private void Awake()
    {
        thisCamera = GetComponent<CinemachineVirtualCamera>();
    }

    
    void Update()
    {
        if (player.GetComponent<PlayerController>().isFacingRight)
        {
            ValueIncreaser(rightOffset, leftOffset);
            thisCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(currentOffset, 0f, 8f);
        }

        if (!player.GetComponent<PlayerController>().isFacingRight)
        {
            ValueDecreaser(rightOffset, leftOffset);

            thisCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(currentOffset, 0f, 8f);
        }
        //Debug.Log(currentOffset);
    }

    void ValueDecreaser(float maxValue, float minValue)
    {
        if (currentOffset > minValue)
        {
            currentOffset -= increaseSpeed * Time.deltaTime;
            Debug.Log("Decreasing");
        }

        if (currentOffset < minValue)
        {
            currentOffset = minValue;
        }
    }

    void ValueIncreaser(float maxValue, float minValue)
    {
        if (currentOffset < maxValue)
        {
            Debug.Log("Increasing");
            currentOffset += increaseSpeed * Time.deltaTime;
        }

        if (currentOffset > maxValue)
        {
            currentOffset = maxValue;
        }
    }
}
