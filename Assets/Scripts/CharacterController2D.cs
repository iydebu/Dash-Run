using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float dashAmount = 5f;

    [Header("Components")]
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private LayerMask _dashLayerMask;
    [SerializeField] private Joystick _joystick;

    [Header("SoundClip")]
    [SerializeField] private List<SoundClip> _soundClip;

    private Rigidbody2D _rb;
    private AudioSource source;

    private Vector2 _moveDir = Vector2.zero;

    private bool _isIdle = true;
    private bool _isDash = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        HandleMovements();
    }

    /// <summary>
    /// Handles player's input.
    /// </summary>
    private void HandleInput()
    {
        float moveX = _joystick.Horizontal;
        float moveY = _joystick.Vertical;

        // Keyboard Input
        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        if (Input.GetKey(KeyCode.D)) moveX = 1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;

        _moveDir = new Vector2(moveX, moveY).normalized;

        // Check for Dash command
        if (Input.GetKeyDown(KeyCode.Space)) _isDash = true;

        // Sprite orientation based on movement direction
        _spriteRenderer.flipX = moveX > 0;

        // Detect idle state
        _isIdle = moveX == 0 && moveY == 0;
    }

    /// <summary>
    /// Handles movement and animation of the player.
    /// </summary>
    private void HandleMovements()
    {
        // Manage idle and movement animations
        if (_isIdle)
        {
            _rb.velocity = Vector2.zero;
            _animator.SetBool("isMoving", false);
        }
        else
        {
            _rb.velocity = _moveDir * speed;
            _animator.SetFloat("horizontalMovement", _moveDir.x);
            _animator.SetFloat("verticalMovement", _moveDir.y);
            _animator.SetBool("isMoving", true);
        }

        // Handle Dash
        if (_isDash)
        {
            Vector2 dashPosition = _rb.position + _moveDir * dashAmount;

            // Check for collisions during dash
            RaycastHit2D hit = Physics2D.Raycast(_rb.position, _moveDir, dashAmount, _dashLayerMask);
            if (hit.collider != null)
            {
                dashPosition = hit.point;
            }

            // Perform the dash
            _rb.MovePosition(dashPosition);
            _isDash = false;

            // Play Dash sound
            source.PlayOneShot(FindSoundClip("dash").clip, FindSoundClip("dash").volume);
        }
    }

    /// <summary>
    /// Finds a sound clip by its name.
    /// </summary>
    private SoundClip FindSoundClip(string soundName)
    {
        return _soundClip.Find(x => x.name == soundName);
    }

    /// <summary>
    /// Triggers the dash action.
    /// </summary>
    public void Dash()
    {
        _isDash = true;
    }
}

[System.Serializable]
class SoundClip
{
    public string name;
    public AudioClip clip;
    public float volume;
}
