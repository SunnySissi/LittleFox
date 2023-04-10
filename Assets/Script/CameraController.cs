using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f; // 缓动时间
    public float leftBound = -10f; // 左边界
    public float rightBound = 10f; // 右边界
    
    private Vector3 velocity = Vector3.zero; // 速度向量


    // Update is called once per frame
    void Update()
    {
        // 获取玩家的水平位置
        float playerX = player.position.x;
        // 获取摄像机的水平位置
        float cameraX = transform.position.x;
        // 判断玩家是否在左边界和右边界之间
        if (playerX > leftBound && playerX < rightBound)
        {
            // 计算目标位置，保持垂直位置不变
            Vector3 targetPosition = new Vector3(playerX, transform.position.y, transform.position.z);
            // 使用SmoothDamp方法实现缓动效果
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
