//control by pc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    UIDirector UI;

    //コンポーネント宣言
    Rigidbody rigidbody;
    Animator animator;

    //変数宣言
    const float speed = 8f;//キャラの移動速度

    /// <summary>
    /// プレイヤーの状態を管理
    /// </summary>
    public enum State
    {
        none,//通常時
        physicalAttack,//物理攻撃時
        shooting,//射撃時
    }
    public State state = State.none;//最初は通常時をセット

    public GameObject swordPrefab;
    public GameObject boxesObj;


    /// <summary>
    /// 一番初めに処理
    /// </summary>
    void Awake()
    {
        this.UI = GameObject.Find("UI").GetComponent<UIDirector>();
        this.rigidbody = GetComponent<Rigidbody>();
        this.animator = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Awakeの次に処理
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// フレーム毎に処理
    /// </summary>
    void Update()
    {
        //処理
        Move();
        ChangeState();
        PhysicalAttack();
        ShootingAttack();

        
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    void Move()
    {
        //キー入力
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move;//移動方向

        if (!(state == State.shooting)){
            move = new Vector3(horizontal, 0, vertical);
            float y = Camera.main.transform.rotation.eulerAngles.y;
            move = Quaternion.Euler(0, y, 0) * move;
            if(move.magnitude > 0)
            {
                transform.rotation = Quaternion.LookRotation(move);
            }
            
        }
        else
        {
            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            // 方向キーの入力値とカメラの向きから、移動方向を決定
            move = cameraForward * vertical + Camera.main.transform.right * horizontal;
        }
        if (move.magnitude > 0)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

        // 移動方向にスピードを掛ける。
        rigidbody.velocity = move * speed;
    }

    /// <summary>
    /// プレイヤーの状態遷移を変更する
    /// </summary>
    void ChangeState()
    {
        //右クリックで射撃状態ON/OFF
        if (Input.GetMouseButtonDown(1))
        {
            //GameObject go = GameObject.Find("Main Camera");
            //go.transform.localEulerAngles = gameObject.transform.localEulerAngles;
            state = State.shooting;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            state = State.none;
        }
    }

    /// <summary>
    /// 物理攻撃
    /// </summary>
    void PhysicalAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("swordAttack");
            Invoke("Go", .65f);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            animator.SetTrigger("attack");
        }
    }
    /// <summary>
    /// 物理攻撃の剣出現
    /// </summary>
    void Go()
    {
        GameObject go = Instantiate(swordPrefab, boxesObj.transform);
    }

    /// <summary>
    /// 射撃
    /// </summary>
    void ShootingAttack()
    {
        if(state == State.shooting)
        {
            UI.AimOn();
            animator.SetBool("aim", true);
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("shot");
            }
        }
        else
        {
            UI.AimOff();
            animator.SetBool("aim", false);
        }
    }
    void ShootingAttackPlayerRot()
    {

    }


    void FixedUpdate()
    {
        //重力調整
        rigidbody.AddForce(Vector3.down * 5, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}

//=============memo==============
