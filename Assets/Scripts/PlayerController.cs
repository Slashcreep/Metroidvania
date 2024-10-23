using System;
using System.Collections.Generic;
using UnityEngine;

namespace TarodevController
{
    /// <summary>
    /// Hey!
    /// Tarodev here. I built this controller as there was a severe lack of quality & free 2D controllers out there.
    /// I have a premium version on Patreon, which has every feature you'd expect from a polished controller. Link: https://www.patreon.com/tarodev
    /// You can play and compete for best times here: https://tarodev.itch.io/extended-ultimate-2d-controller
    /// If you hve any questions or would like to brag about your score, come to discord: https://discord.gg/tarodev
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rb;
        private CapsuleCollider2D _col;
        private FrameInput _frameInput;
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        public bool isFacingRight;

        private float _fallSpeedDampingChangeThreshold;
        #region Interface

        public Vector2 FrameInput => _frameInput.Move;
        public event Action<bool, float> GroundedChanged;
        public event Action Jumped;

        #endregion

        private float _time;

        #region Attacks

        public enum PlayerInputs { Up, Down, Left, Right, Block, Range };
        public List<PlayerInputs> upwardSlashRight;
        public List<PlayerInputs> upwardSlashLeft;
        public List<PlayerInputs> downwardSlashRight;
        public List<PlayerInputs> downwardSlashLeft;
        public List<PlayerInputs> sideSlashRight;
        public List<PlayerInputs> sideSlashLeft;
        public List<PlayerInputs> airSlashCCRight;
        public List<PlayerInputs> airSlashCCLeft;
        public List<PlayerInputs> airSlashCRight;
        public List<PlayerInputs> airSlashCLeft;
        public List<PlayerInputs> range;
        float timeSinceLastInput;
        public bool isBlocking = false;
        public bool isRightBlocking = false;
        public bool isLeftBlocking = false;
        public bool isUpBlocking = false;
        public bool isUpwardSlashRight = false;
        public bool isUpwardSlashLeft = false;
        public bool isDownwardSlashRight = false;
        public bool isDownwardSlashLeft = false;
        public bool isSideSlashRight = false;
        public bool isSideSlashLeft = false;
        public bool isAirSlashCCRight = false;
        public bool isAirSlashCCLeft = false;
        public bool isAirSlashCRight = false;
        public bool isAirSlashCLeft = false;
        public bool isAttacking = false;

        [SerializeField] List<PlayerInputs> groundedInputs = new List<PlayerInputs>();
        [SerializeField] List<PlayerInputs> midairInputs = new List<PlayerInputs>();

        #endregion

        private CameraFollowObject _cameraFollowObject;


        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<CapsuleCollider2D>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            isBlocking = false;
            isUpwardSlashRight = false;
            isDownwardSlashRight = false;
            isSideSlashRight = false;

            _fallSpeedDampingChangeThreshold = CameraManager.instance._fallSpeedYDampingChangeThreshold;
        }

        private void Update()
        {
            _time += Time.deltaTime;
            GatherInput();
            InputRemover();
            Attacks();
            DirectionChecker();

            if (_rb.velocity.y < _fallSpeedDampingChangeThreshold && !CameraManager.instance.IsLerpingYDamping && !CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpYDamping(true);
            }

            if (_rb.velocity.y >= 0f && !CameraManager.instance.IsLerpingYDamping & CameraManager.instance.LerpedFromPlayerFalling)
            {
                CameraManager.instance.LerpedFromPlayerFalling = false;

                CameraManager.instance.LerpYDamping(false);
            }
        }

        

        void ChangeLooking()
        {
            if (isFacingRight)
            {
                transform.GetChild(13).localScale = new Vector2(1f, 1f);
            }

            if (!isFacingRight)
            {
                transform.GetChild(13).localScale = new Vector2(-1f, 1f);
            }
        }

        private void DirectionChecker()
        {
            if (_rb.velocity.x < 0)
            {
                isFacingRight = false;
            }

            if (_rb.velocity.x > 0)
            {
                isFacingRight = true;
            }    
        }
        private void InputRemover()
        {
            if (groundedInputs.Count >= 3)
            {
                groundedInputs.RemoveAt(0);
                Debug.Log("Grounded Removed");
            }

            if (midairInputs.Count >= 4)
            {
                midairInputs.RemoveAt(0);
                Debug.Log("Air Input Removed");
            }

            timeSinceLastInput += Time.deltaTime;

            if (timeSinceLastInput > 1)
            {
                groundedInputs.Clear();
                midairInputs.Clear();
            }

        }

        private void GatherInput()
        {
            _frameInput = new FrameInput
            {
                JumpDown = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C),
                JumpHeld = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.C),
                Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
            };

            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }


            if (Input.GetKey(KeyCode.E)) { isBlocking = true; Debug.Log("Holding E"); }
            else if (Input.GetKeyUp(KeyCode.E)) { isBlocking = false; Debug.Log("Let go of E"); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { groundedInputs.Add(PlayerInputs.Range); Debug.Log("2"); }

            if (!isAttacking && _grounded)
            { 
                midairInputs.Clear();
            if (Input.GetKeyDown(KeyCode.UpArrow) && !isBlocking) { groundedInputs.Add(PlayerInputs.Up); timeSinceLastInput = 0f; }
            if (Input.GetKeyDown(KeyCode.DownArrow) && !isBlocking) { groundedInputs.Add(PlayerInputs.Down); timeSinceLastInput = 0f; }
            if (Input.GetKeyDown(KeyCode.LeftArrow) && !isBlocking) { groundedInputs.Add(PlayerInputs.Left); timeSinceLastInput = 0f; }
            if (Input.GetKeyDown(KeyCode.RightArrow) && !isBlocking) { groundedInputs.Add(PlayerInputs.Right); timeSinceLastInput = 0f; }
            }
            if (!isAttacking && !_grounded)
            {
                groundedInputs.Clear();
                if (Input.GetKeyDown(KeyCode.UpArrow) && !isBlocking) { midairInputs.Add(PlayerInputs.Up); timeSinceLastInput = 0f; }
                if (Input.GetKeyDown(KeyCode.DownArrow) && !isBlocking) { midairInputs.Add(PlayerInputs.Down); timeSinceLastInput = 0f; }
                if (Input.GetKeyDown(KeyCode.LeftArrow) && !isBlocking) { midairInputs.Add(PlayerInputs.Left); timeSinceLastInput = 0f; }
                if (Input.GetKeyDown(KeyCode.RightArrow) && !isBlocking) { midairInputs.Add(PlayerInputs.Right); timeSinceLastInput = 0f; }
            }
        }

        private void FixedUpdate()
        {
            CheckCollisions();

            HandleJump();
            HandleDirection();
            HandleGravity();
            
            ApplyMovement();
        }

        #region Collisions
        
        private float _frameLeftGrounded = float.MinValue;
        public bool _grounded;

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            // Ground and Ceiling
            bool groundHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_col.bounds.center, _col.size, _col.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            // Hit a Ceiling
            if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

            // Landed on the Ground
            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
            }
            // Left the Ground
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                GroundedChanged?.Invoke(false, 0);
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        #endregion


        #region Jumping

        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

            if (!_jumpToConsume && !HasBufferedJump) return;

            if (_grounded || CanUseCoyote) ExecuteJump();

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            Jumped?.Invoke();
        }

        #endregion

        #region Horizontal

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                var deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }


        #endregion

        #region Gravity

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                var inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        #endregion

        private void ApplyMovement() => _rb.velocity = _frameVelocity;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_stats == null) Debug.LogWarning("Please assign a ScriptableStats asset to the Player Controller's Stats slot", this);
        }
#endif

        private void Attacks()
        {
            #region Grounded Attacks
            if (!isBlocking && _grounded)
            {
                for (int i = groundedInputs.Count - 1, j = upwardSlashRight.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = groundedInputs[i];
                    PlayerInputs nextInput = upwardSlashRight[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Upward Slash Right");
                        isUpwardSlashRight = true;
                        groundedInputs.Clear();
                    }
                }

                for (int i = groundedInputs.Count - 1, j = upwardSlashLeft.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = groundedInputs[i];
                    PlayerInputs nextInput = upwardSlashLeft[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Upward Slash Left");
                        isUpwardSlashLeft = true;
                        groundedInputs.Clear();
                    }
                }

                for (int i = groundedInputs.Count - 1, j = downwardSlashRight.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = groundedInputs[i];
                    PlayerInputs nextInput = downwardSlashRight[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Downward Slash Right");
                        isDownwardSlashRight = true;
                        groundedInputs.Clear();
                    }
                }

                for (int i = groundedInputs.Count - 1, j = downwardSlashLeft.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = groundedInputs[i];
                    PlayerInputs nextInput = downwardSlashLeft[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Downward Slash Left");
                        isDownwardSlashLeft = true;
                        groundedInputs.Clear();
                    }
                }

                for (int i = groundedInputs.Count - 1, j = sideSlashRight.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = groundedInputs[i];
                    PlayerInputs nextInput = sideSlashRight[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Side Slash Right");
                        isSideSlashRight = true;
                        groundedInputs.Clear();
                    }
                }

                for (int i = groundedInputs.Count - 1, j = sideSlashLeft.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = groundedInputs[i];
                    PlayerInputs nextInput = sideSlashLeft[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Side Slash Left");
                        isSideSlashLeft = true;
                        groundedInputs.Clear();
                    }
                }
            }
            #endregion

            #region Air Attacks
            if (!isBlocking && !_grounded)
            {
                for (int i = midairInputs.Count - 1, j = airSlashCCRight.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = midairInputs[i];
                    PlayerInputs nextInput = airSlashCCRight[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Midair Slash CC Right");
                        isAirSlashCCRight = true;
                        midairInputs.Clear();
                    }
                }
            }

            if (!isBlocking && !_grounded)
            {
                for (int i = midairInputs.Count - 1, j = airSlashCCLeft.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = midairInputs[i];
                    PlayerInputs nextInput = airSlashCCLeft[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Midair Slash CC Right");
                        isAirSlashCCLeft = true;
                        midairInputs.Clear();
                    }
                }
            }

            if (!isBlocking && !_grounded)
            {
                for (int i = midairInputs.Count - 1, j = airSlashCRight.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = midairInputs[i];
                    PlayerInputs nextInput = airSlashCRight[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Midair Slash C Right");
                        isAirSlashCRight = true;
                        midairInputs.Clear();
                    }
                }
            }

            if (!isBlocking && !_grounded)
            {
                for (int i = midairInputs.Count - 1, j = airSlashCLeft.Count - 1; i >= 0; i--, j--)
                {
                    PlayerInputs input = midairInputs[i];
                    PlayerInputs nextInput = airSlashCLeft[j];

                    if (input != nextInput)
                    {
                        break;
                    }
                    else if (j == 0)
                    {
                        Debug.Log("Midair Slash C Left");
                        isAirSlashCLeft = true;
                        midairInputs.Clear();
                    }
                }
            }
            #endregion

            #region Block

            if (isBlocking)
            {
                isUpBlocking = Input.GetKey(KeyCode.UpArrow);
                isRightBlocking = Input.GetKey(KeyCode.RightArrow);
                isLeftBlocking = Input.GetKey(KeyCode.LeftArrow);
            }
            else if(!isBlocking)
            {
                isUpBlocking = false; isRightBlocking = false; isLeftBlocking = false;
            }
            #endregion
        }
    }
    
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    public interface IPlayerController
    {
        public event Action<bool, float> GroundedChanged;

        public event Action Jumped;
        public Vector2 FrameInput { get; }
    }
}