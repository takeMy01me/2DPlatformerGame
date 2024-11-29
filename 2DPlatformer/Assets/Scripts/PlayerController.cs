using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region ����
    private Rigidbody2D rb; // �������
    private Animator anim; // ����������

    private float movementInputDirection; // ������ƶ�����
    private bool isFacingRight = true; // Player����
    private bool isWalking; // �Ƿ��������� ����Animtor
    private bool isGrounded; // �Ƿ��ڵ����� �����ж���Ծ
    private bool canJump; // �Ƿ������Ծ

    private int amountOfJumpsLeft; // ʣ�����Ծ����

    [SerializeField] private float movementSpeed = 10.0f; // �ƶ��ٶ�
    [SerializeField] private float jumpForce = 16.0f; // ��Ծ��
    [SerializeField] private int amountOfJumps = 2; // �ܵ���Ծ����
    [SerializeField] private Transform groundCheck; // ���е����������
    [SerializeField] private float groundCheckRadius = 1f;
    [SerializeField] private LayerMask groundCheckLayerMask;
   
    #endregion

    #region ��������
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

    #region ����
    /// <summary>
    /// ������
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
    /// �ƶ�������
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
    /// �ж��ܱ����� �����Ƿ��ڵ����ϡ�
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
    /// ִ��Player����ķ�ת�߼�
    /// </summary>
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    /// <summary>
    /// ִ���ƶ��߼�
    /// </summary>
    private void ApplyMovement()
    {
        rb.velocity = new Vector2(movementInputDirection * movementSpeed, rb.velocity.y);
    }

    /// <summary>
    /// ִ����Ծ�߼�
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
    /// ���¶���״̬��
    /// </summary>
    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }
    #endregion
}
