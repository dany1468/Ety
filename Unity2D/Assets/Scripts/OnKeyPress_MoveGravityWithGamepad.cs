using UnityEngine;

public class OnKeyPress_MoveGravityWithGamepad
    : MonoBehaviour
{
    [SerializeField, Header("スピード")] private float speed = 3;
    [SerializeField, Header("ジャンプ力")] private float jumpPower = 8;

    private float _vx = 0;

    private bool _leftFlag;
    private bool _pushFlag;
    private bool _jumpFlag;
    private bool _groundFlag;
    private Rigidbody2D _rbody;

    private void Start()
    {
        _rbody = GetComponent<Rigidbody2D>();
        _rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        _vx = 0;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > 0)
        {
            _vx = speed;
            _leftFlag = false;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < 0)
        {
            _vx = -speed;
            _leftFlag = true;
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetButtonDown("Jump")) && _groundFlag)
        {
            if (!_pushFlag)
            {
                _jumpFlag = true;
                _pushFlag = true;
            }
        }
        else
        {
            // 押しっぱなし解除
            _pushFlag = false;
        }
    }

    private void FixedUpdate()
    {
        _rbody.velocity = new Vector2(_vx, _rbody.velocity.y);

        GetComponent<SpriteRenderer>().flipX = _leftFlag;

        if (_jumpFlag)
        {
            _jumpFlag = false;
            _rbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _groundFlag = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _groundFlag = false;
    }
}