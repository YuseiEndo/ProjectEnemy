//control by pc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //プレイヤーコントローラー宣言
    PlayerController playerController;

    //ゲームオブジェクト宣言
    GameObject playerObj;
    GameObject cameraShootingPosObj;

    //変数宣言
    Vector3 playerPosition;
    Vector3 cameraShootingPos;
    float pitch = 20f;//ピッチ回転
    float yaw = 0f;//ヨー回転
    float distance = 4f;
    const float rollSpeed = 10f;
    float cameraHeight = 1.5f;
    const int mouseSensitivity = 2;

    /// <summary>
    /// 一番初めに処理
    /// </summary>
    void Awake()
    {
        //PlayerControllerクラス取得
        this.playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        //Playerオブジェクト取得
        this.playerObj = GameObject.Find("Player");
        //shootingCameraPosオブジェクト取得
        this.cameraShootingPosObj = GameObject.Find("cameraShootingPos");
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
        CameraPos();
        CameraRot();
    }

    void LateUpdate()
    {
        //右クリックを押したらプレイヤーの向いている方向に近距離カメラを向ける
        if (Input.GetMouseButtonDown(1))
        {
            transform.localEulerAngles = playerObj.transform.localEulerAngles;
        }
        //右クリックを離したらプレイヤーの向いていた方向に遠距離カメラを向ける
        else if (Input.GetMouseButtonUp(1))
        {
            yaw = playerObj.transform.localEulerAngles.y;
            Quaternion q = Quaternion.Euler(pitch, yaw, 0);
            playerPosition = playerObj.transform.position + Vector3.up * cameraHeight;
            transform.position = playerPosition + q * Vector3.back * distance;
            transform.LookAt(playerPosition);
        }
    }

    /// <summary>
    /// カメラの位置
    /// </summary>
    void CameraPos()
    {
        //射撃時以外のカメラ位置（遠距離カメラ）
        if(!(playerController.state == PlayerController.State.shooting))
        {
            Quaternion q = Quaternion.Euler(pitch, yaw, 0);
            playerPosition = playerObj.transform.position + Vector3.up * cameraHeight;
            transform.position = playerPosition + q * Vector3.back * distance;
            transform.LookAt(playerPosition);
        }
        //射撃時のカメラ位置（近距離カメラ）
        else
        {
            cameraShootingPos = cameraShootingPosObj.transform.position;
            transform.position = cameraShootingPos;
        }
    }

    /// <summary>
    /// カメラ回転
    /// </summary>
    void CameraRot()
    {
        ////射撃時以外のカメラ回転
        if (!(playerController.state == PlayerController.State.shooting))
        {
            ////左Ctrlキーでカメラ回転
            //if (Input.GetKey(KeyCode.LeftControl))
            //{
            //    yaw -= rollSpeed * Time.deltaTime;
            //}

            ////右Ctrlキーでカメラ回転
            //if (Input.GetKey(KeyCode.RightControl))
            //{
            //    yaw += rollSpeed * Time.deltaTime;
            //}

            //マウス軸取得
            float X_Rotation = Input.GetAxis("Mouse X");
            //Y軸回転
            yaw += X_Rotation * mouseSensitivity;
        }
        //射撃時のカメラ回転
        else
        {
            //マウス軸取得
            float X_Rotation = Input.GetAxis("Mouse X");
            float Y_Rotation = Input.GetAxis("Mouse Y");

            //Y軸回転（射撃時以外の分）
            yaw += X_Rotation * mouseSensitivity;

            //X軸回転
            Vector3 localAngle = transform.localEulerAngles;
            localAngle.x += (-Y_Rotation * mouseSensitivity);
            Mathf.Clamp(localAngle.x, 40f, -40f);
            transform.localEulerAngles = localAngle;

            //Y軸回転
            Vector3 angle = transform.eulerAngles;
            angle.y += X_Rotation * mouseSensitivity;
            transform.eulerAngles = angle;

            //カメラに合わせてプレイヤーの回転処理
            //playerObj.transform.Rotate(0, X_Rotation * mouseSensitivity, 0);
            Quaternion q1 = playerObj.transform.rotation;
            Quaternion q2 = gameObject.transform.rotation;
            playerObj.transform.rotation = Quaternion.Lerp(q1, q2, Time.deltaTime * rollSpeed);
        }
    }
}

//==========memo===============
//最終的には操作しやすい何かでカメラ回転する
//ゲームパッドで操作できるようにしたいね
