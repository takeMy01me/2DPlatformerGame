using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region 参数
    private Rigidbody2D rb; // 刚体组件
    private Animator anim; // 动画控制器

    private float movementInputDirection; // 输入的移动方向
    private bool isFacingRight = true; // Player朝向
    private bool isWalking; // 是否正在行走 用于Animtor
    private bool isGrounded; // 是否在地面上 用于判断跳跃
    private bool canJump; // 是否可以跳跃

    private int amountOfJumpsLeft; // 剩余可跳跃次数

    [SerializeField] private float movementSpeed = 10.0f; // 移动速度
    [SerializeField] private float jumpForce = 16.0f; // 跳跃力
    [SerializeField] private int amountOfJumps = 2; // 总的跳跃次数
    [SerializeField] private Transform groundCheck; // 进行地面检测的物体
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private LayerMask groundCheckLayerMask;
   
    #endregion

    #region 生命周期
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
    #endregion

    #region 方法
    /// <summary>
    /// 输入检测
    /// </summary>
    private void CheckInput()
    {
        movementInputDirection = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    /// <summary>
    /// 移动方向检测
    /// </summary>
    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        isWalking = rb.velocity.x != 0;
    }

    /// <summary>
    /// 判断周边物体 用于是否在地面上、
    /// </summary>
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundCheckLayerMask);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }

    /// <summary>
    /// 执行Player朝向的翻转逻辑
    /// </summary>
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    /// <summary>
    /// 执行移动逻辑
    /// </summary>
    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
    }

    /// <summary>
    /// 执行跳跃逻辑
    /// </summary>
    private void Jump()
    {
        if (canJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
        
    }

    /// <summary>
    /// 更新动画状态机
    /// </summary>
    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
    #endregion
}
