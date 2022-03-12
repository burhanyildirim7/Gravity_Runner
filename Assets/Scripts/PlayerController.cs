using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
	#region SINGLETON
	public static PlayerController instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
	#endregion
	public int collectibleDegeri;
    public bool xVarMi = true;
    public bool collectibleVarMi = true;
    private bool left, right, isEnableForSwipe;
    [SerializeField] private float skateSpeed = 5.0f;
    [SerializeField] private float swipeControlLimit;
    [SerializeField] private float playerSwipeSpeed;
    [SerializeField] private float horizontalRadius = 3;
    private float screenWidth, screenHeight;
    private float lastMousePosX, firstMousePosX, lastMousePosY, firstMousePosY;
    public GameObject model, cameraTarget;
    public GameObject controlAnimationPanel, leftContolPanel, rightControlPanel, slideControlPanel;
    private bool isFirstLevel;
    public int LevelPlatformCount = 6;
    public Animator PlayerAnimator;


    void Start()
    {
        if (PlayerPrefs.GetInt("level") == 0) isFirstLevel = true;
        StartingEvents();
        DOTween.Init();
        screenWidth = Screen.width / 2;
        screenWidth = Screen.height / 2;
    }

    private void Update()
    {
        #region For Mobile Control ... SBI
        //if (Input.touchCount > 0 && isEnableForSwipe)
        //{
        //    Touch myTouch = Input.GetTouch(0);
        //    if (myTouch.deltaPosition.x > swipeControlLimit)
        //    {
        //        isEnableForSwipe = false;
        //        right = true;
        //        left = false;
        //        MoveHorizontal();
        //        return;

        //    }
        //    else if (myTouch.deltaPosition.x < -swipeControlLimit)
        //    {
        //        isEnableForSwipe = false;
        //        right = false;
        //        left = true;
        //        MoveHorizontal();
        //        return;
        //    }
        //    else if (myTouch.deltaPosition.y < -swipeControlLimit)
        //    {
        //        StartCoroutine(DelayAndNormalizedCollider());
        //        //isEnableForSwipe = false;
        //        SlideEvents();
        //        return;
        //    }
        //}
        #endregion



        #region For Stand Control ... SBI

        if (Input.GetMouseButtonDown(0) && isEnableForSwipe)
        {
            firstMousePosX = Input.mousePosition.x - screenWidth;
            firstMousePosY = Input.mousePosition.y - screenHeight;
        }
        if (Input.GetMouseButton(0) && isEnableForSwipe && LevelPlatformCount == 6)
        {
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;

            if((lastMousePosY - firstMousePosY) > swipeControlLimit && (lastMousePosX - firstMousePosX) > swipeControlLimit / 2) // RT
			{
                StartCoroutine(DlayAndEnabledSwipe());
                MoveRT();
			}
            else if ((lastMousePosY - firstMousePosY) <- swipeControlLimit && (lastMousePosX - firstMousePosX) > swipeControlLimit / 2) // RB
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveRB();
            }
            else if ((lastMousePosY - firstMousePosY) < -swipeControlLimit && (lastMousePosX - firstMousePosX) <- swipeControlLimit / 2) // LB
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveLB();
            }
            else if ((lastMousePosY - firstMousePosY) > swipeControlLimit && (lastMousePosX - firstMousePosX) < -swipeControlLimit / 2) // LT
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveLT();
            }
            else if ((lastMousePosY - firstMousePosY) > swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit / 2) // T
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveT();
            }
            else if ((lastMousePosY - firstMousePosY) <- swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit / 2) // B
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveB();
            }     
        }
        else if (Input.GetMouseButton(0) && isEnableForSwipe && LevelPlatformCount == 4)
        {
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;

            if ((lastMousePosY - firstMousePosY) > swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit) // T
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveT();
            }
            else if ((lastMousePosY - firstMousePosY) < -swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit) // B
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveB();
            }
            else if (Mathf.Abs(lastMousePosY - firstMousePosY) < swipeControlLimit && (lastMousePosX - firstMousePosX) > swipeControlLimit ) // R
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveR();
            }
            else if (Mathf.Abs(lastMousePosY - firstMousePosY) < swipeControlLimit && (lastMousePosX - firstMousePosX) < -swipeControlLimit) // L
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveL();
            }
        }
        else if (Input.GetMouseButton(0) && isEnableForSwipe && LevelPlatformCount == 2)
        {
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;

            if ((lastMousePosY - firstMousePosY) > swipeControlLimit) // T
            {
                StartCoroutine(DlayAndEnabledSwipe());
                MoveT();
            }
            else if ((lastMousePosY - firstMousePosY) < -swipeControlLimit) // B
            {
                
                StartCoroutine(DlayAndEnabledSwipe());
                MoveB();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastMousePosX = firstMousePosX = lastMousePosY = firstMousePosY = 0;
        }

        #endregion
    }

    IEnumerator DlayAndEnabledSwipe()
    {     
        yield return new WaitForSeconds(.5f);
        isEnableForSwipe = true;
    }


	#region MOVEMENTS
	private void MoveT()
	{
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(0,6f,0),.5f);
        transform.DOLocalMove(new Vector3(0,10,0),.5f);
        transform.DORotate(new Vector3(0,0,180),.5f);
	}

    private void MoveB()
	{
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(0, 4f, 0), .5f);
        transform.DOLocalMove(Vector3.zero, .5F);
        transform.DORotate(new Vector3(0, 0,0), .5f);
    }

    private void MoveR()
	{
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(1f, 5, 0), .5f);
        transform.DOLocalMove(new Vector3(5, 5, 0), .5f);
        transform.DORotate(new Vector3(0, 0, 90), .5f);
    }

    private void MoveL()
	{
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(-1f, 5, 0), .5f);
        transform.DOLocalMove(new Vector3(-5, 5, 0), .5f);
        transform.DORotate(new Vector3(0, 0, -90), .5f);
    }

    private void MoveLB()
    {
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(-1f, 5, 0), .5f);
        transform.DOLocalMove(new Vector3(-4.8F, 2.5F, 0), .5f);
        transform.DORotate(new Vector3(0, 0, -60), .5f);
    }
    private void MoveLT()
    {
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(-1f, 5, 0), .5f);
        transform.DOLocalMove(new Vector3(-4.53F, 7.9F, 0), .5f);
        transform.DORotate(new Vector3(0, 0, -120), .5f);
    }

    private void MoveRT()
    {
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(1f, 5, 0), .5f);
        transform.DOLocalMove(new Vector3(4.7F, 7.9F, 0), .5f);
        transform.DORotate(new Vector3(0, 0, 120), .5f);
    }

    private void MoveRB()
    {
        JumpAnim();
        isEnableForSwipe = false;
        cameraTarget.transform.DOLocalMove(new Vector3(1f, 5, 0), .5f);
        transform.DOLocalMove(new Vector3(4.7F, 2.2F, 0), .5f);
        transform.DORotate(new Vector3(0, 0, 60), .5f);
    }

	#endregion 
	
  

    /// <summary>
    /// Playerin collider olaylari.. collectible, engel veya finish noktasi icin. Burasi artirilabilir.
    /// elmas icin veya baska herhangi etkilesimler icin tag ekleyerek kontrol dongusune eklenir.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("collectible"))
        {
            // COLLECTIBLE CARPINCA YAPILACAKLAR...
            MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
            Destroy(other.gameObject);;
            GameController.instance.SetScore(collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
           // GameObject arti = Instantiate(artiBir, new Vector3(model.transform.position.x, model.transform.position.y + 5, model.transform.position.z), Quaternion.identity, model.transform);
           
        }
        else if (other.CompareTag("engel"))
        {
            // ENGELELRE CARPINCA YAPILACAKLAR....
            MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);

           // GameObject eksi = Instantiate(eksiBir, new Vector3(model.transform.position.x, model.transform.position.y + 3, model.transform.position.z), Quaternion.identity, model.transform);
    
            GameController.instance.SetScore(-collectibleDegeri); // ORNEK KULLANIM detaylar icin ctrl+click yapip fonksiyon aciklamasini oku
            if (GameController.instance.score < 0) // SKOR SIFIRIN ALTINA DUSTUYSE
            {
                GameController.instance.isContinue = false; // çarptığı anda oyuncunun yerinde durması ilerlememesi için
                UIController.instance.ActivateLooseScreen();
                IdleAnim();
            }
            else
            {
                CrashAnim();
            }

        }
        else if (other.CompareTag("finish"))
        {
            other.GetComponent<Collider>().enabled = false;
            MoveB();
            // GetComponent<Collider>().enabled = false;
        }
        else if (other.CompareTag("dance"))
		{
            GameController.instance.isContinue = false;
            isEnableForSwipe = false;
            DanceAnim();
            UIController.instance.ActivateWinScreen();
		}
        //else if (other.CompareTag("finalx"))
        //{
        //    FinalEffectEvents(other.gameObject);
        //}
        //else if (isFirstLevel && other.CompareTag("control1"))
        //{
        //    controlAnimationPanel.SetActive(true);
        //    rightControlPanel.SetActive(true);
        //    //KarakterPaketiMovement.instance._speed = 2;
        //    GetComponentInChildren<Animator>().speed = .15f;
        //}
        //else if (isFirstLevel && other.CompareTag("control2"))
        //{
        //    controlAnimationPanel.SetActive(true);
        //    leftContolPanel.SetActive(true);
        //   // KarakterPaketiMovement.instance._speed = 2;
        //    GetComponentInChildren<Animator>().speed = .15f;
        //}
        //else if (isFirstLevel && other.CompareTag("control3"))
        //{
        //    controlAnimationPanel.SetActive(true);
        //    slideControlPanel.SetActive(true);
        //   // KarakterPaketiMovement.instance._speed = 2;
        //    GetComponentInChildren<Animator>().speed = .15f;

        //}

    }


  

    /// <summary>
    /// Bu fonksiyon her level baslarken cagrilir. 
    /// </summary>
    public void StartingEvents()
    {
        IdleAnim();
        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.transform.position = Vector3.zero;
        transform.position = new(0, 0f, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        GameController.instance.isContinue = false;
        GameController.instance.score = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
        GetComponent<Collider>().enabled = true;
        
    }

    public void PostStartingEvents()
    {
        RunAnim();
        isEnableForSwipe = true;
        GameController.instance.isContinue = true;
    }

 

    #region ANIMATIONS .....


    private void RunAnim()
    {
        PlayerAnimator.SetTrigger("run");
        StartCoroutine(DelayAndResetAnims());
    }

    private void IdleAnim()
    {
        PlayerAnimator.SetTrigger("idle");
        StartCoroutine(DelayAndResetAnims());
    }

    private void JumpAnim()
    {
        PlayerAnimator.SetTrigger("jump");
        StartCoroutine(DelayAndResetAnims());
    }

    private void SlideAnim()
    {
        PlayerAnimator.SetTrigger("slide");
        StartCoroutine(DelayAndResetAnims());
    }

    private void CrashAnim()
    {
        PlayerAnimator.SetTrigger("struggle");
        StartCoroutine(DelayAndResetAnims());
    }

    private void RopeJumpAnim()
    {
        PlayerAnimator.SetTrigger("ropejump");
        StartCoroutine(DelayAndResetAnims());
    }

    private void DanceAnim()
    {
        PlayerAnimator.SetTrigger("dance");
        StartCoroutine(DelayAndResetAnims());
    }


    private IEnumerator DelayAndResetAnims()
    {
        yield return new WaitForSeconds(.05f);
        PlayerAnimator.ResetTrigger("idle");
        PlayerAnimator.ResetTrigger("jump");
        PlayerAnimator.ResetTrigger("run");
        //GetComponentInChildren<Animator>().ResetTrigger("slide");
        //GetComponentInChildren<Animator>().ResetTrigger("struggle");
        //GetComponentInChildren<Animator>().ResetTrigger("ropejump");
        //GetComponentInChildren<Animator>().ResetTrigger("ropepos");
        //GetComponentInChildren<Animator>().ResetTrigger("throw");
    }

    #endregion

}
