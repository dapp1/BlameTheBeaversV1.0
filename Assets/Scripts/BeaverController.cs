using UnityEngine;


public class BeaverController : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    private bool _isGrounded;
    private Animator _anim;
    private CharacterController player;
    private ClickableObject _clickable;

    private bool _isAttack;
    private bool _isDead;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _clickable = GetComponent<ClickableObject>();
        player = FindObjectOfType<CharacterController>();
        
        _clickable.ClickEvent.AddListener(() =>
        {
            player.KickBeaver(this, Die);
        });
        
        _speed = GlobalSettings.Instance.BeaverSpeed;
    }

    private void OnEnable()
    {
        _anim.Play("BeaverRun");
        _isDead = false;
        _isAttack = false;
    }
    
    void Update()
    {
        if (_isDead)
            return;
        
        var playerPosition = player.transform.position;
        if (Mathf.Abs(transform.position.x - playerPosition.x) <= 0.5f && _isAttack == false) 
        {
            StartAttack();
;        } else if (_isAttack == false)
        {
            var direction = transform.position.x > playerPosition.x ? -1 : 1;
            transform.localScale = new Vector3(-direction, transform.localScale.y);
            transform.position += new Vector3(direction * _speed * Time.deltaTime, 0);
        }
    }

    private void StartAttack ()
    {
        _isAttack = true;
        _anim.SetBool("isAttack", true);
        _anim.Play("BeaverAttack");
    }

    public void DamagePlayer()
    {
        if (_isDead)
            return;
        
        var playerPosition = player.transform.position;
        if ((Mathf.Abs(transform.position.x - playerPosition.x) <= 0.5f))
        {
            CharacterController.Instance.Damage();
        }
        else
        {
            _isAttack = false;
            _anim.SetBool("isAttack", false);
        }
    }

    private void Die()
    {
        CoinsAndScoreController.Instance.ChangeCoinsValue(GlobalSettings.Instance.CoinsForBeaver);
        CoinsAndScoreController.Instance.ChangeScoreValue(GlobalSettings.Instance.ScoreForBeaver);
        _anim.Play("BeaverDie");
        _isDead = true;
    }

    //Called from animation event
    public void Destroy()
    {
        gameObject.SetActive(false);
    }
}
