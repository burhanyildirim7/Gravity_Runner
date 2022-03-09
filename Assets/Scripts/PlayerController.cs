using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using DG.Tweening;


public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
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
    public GameObject model;
    public GameObject controlAnimationPanel, leftContolPanel, rightControlPanel, slideControlPanel;
    private bool isFirstLevel;
    public int LevelPlatformCount = 6;

    [SerializeField] private ParticleSystem _tozEfekti;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

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
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("RT");
			}
            else if ((lastMousePosY - firstMousePosY) <- swipeControlLimit && (lastMousePosX - firstMousePosX) > swipeControlLimit / 2) // RB
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("RB");
            }
            else if ((lastMousePosY - firstMousePosY) < -swipeControlLimit && (lastMousePosX - firstMousePosX) <- swipeControlLimit / 2) // LB
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("LB");
            }
            else if ((lastMousePosY - firstMousePosY) > swipeControlLimit && (lastMousePosX - firstMousePosX) < -swipeControlLimit / 2) // LT
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("LT");
            }
            else if ((lastMousePosY - firstMousePosY) > swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit / 2) // T
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("T");
            }
            else if ((lastMousePosY - firstMousePosY) <- swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit / 2) // T
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("T");
            }     
        }
        else if (Input.GetMouseButton(0) && isEnableForSwipe && LevelPlatformCount == 4)
        {
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;

            if ((lastMousePosY - firstMousePosY) > swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit) // T
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("RT");
            }
            else if ((lastMousePosY - firstMousePosY) < -swipeControlLimit && Mathf.Abs(lastMousePosX - firstMousePosX) < swipeControlLimit) // B
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("RB");
            }
            else if (Mathf.Abs(lastMousePosY - firstMousePosY) < swipeControlLimit && (lastMousePosX - firstMousePosX) > swipeControlLimit ) // R
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("LB");
            }
            else if (Mathf.Abs(lastMousePosY - firstMousePosY) < swipeControlLimit && (lastMousePosX - firstMousePosX) < -swipeControlLimit) // L
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                Debug.Log("LT");
            }
        }
        else if (Input.GetMouseButton(0) && isEnableForSwipe && LevelPlatformCount == 2)
        {
            lastMousePosX = Input.mousePosition.x - screenWidth;
            lastMousePosY = Input.mousePosition.y - screenHeight;

            if ((lastMousePosY - firstMousePosY) > swipeControlLimit) // T
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                MoveT();
                Debug.Log("T");
            }
            else if ((lastMousePosY - firstMousePosY) < -swipeControlLimit) // B
            {
                isEnableForSwipe = false;
                StartCoroutine(DlayAndEnabledSwipe());
                MoveB();
                Debug.Log("B");
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

    private void MoveT()
	{
        transform.DOLocalMove(new Vector3(0,10,0),.5f);
	}

    private void MoveB()
	{
        transform.DOLocalMove(Vector3.zero, .5F);
	}

    private void MoveHorizontal()
    {
        //if (isFirstLevel)
        //{
        //    GetComponentInChildren<Animator>().speed = 1f;
        //    KarakterPaketiMovement.instance._speed = 8;
        //    controlAnimationPanel.SetActive(false);
        //    rightControlPanel.SetActive(false);
        //    leftContolPanel.SetActive(false);
        //    slideControlPanel.SetActive(false);

        //}

        if (right)
        {
            if (transform.position.x > horizontalRadius - 1)
            {
                isEnableForSwipe = true;
                return;
            }
            else if (transform.position.x > -1)
            {
                _tozEfekti.Play();
                JumpAnim();
                //transform.DOMoveX(horizontalRadius, playerSwipeSpeed).OnComplete(() =>
                //{
                //    isEnableForSwipe = true;
                //    return;
                //});

            }
            else if (transform.position.x < -1)
            {
                _tozEfekti.Play();
                JumpAnim();
               // transform.DOMoveX(0, playerSwipeSpeed).OnComplete(() =>
                //{
                //    isEnableForSwipe = true;
                //    return;
                //});
            }
        }
        else if (left)
        {
            if (transform.position.x < -horizontalRadius + 1)
            {
                isEnableForSwipe = true;
                return;
            }
            else if (transform.position.x < 1)
            {
                _tozEfekti.Play();
                JumpAnim();
                //transform.DOMoveX(-horizontalRadius, playerSwipeSpeed).OnComplete(() =>
                //{
                //    isEnableForSwipe = true;
                //    return;
                //});
            }

            else if (transform.position.x > 1)
            {
                _tozEfekti.Play();
                JumpAnim();
               // transform.DOMoveX(0, playerSwipeSpeed).OnComplete(() =>
                //{
                //    isEnableForSwipe = true;
                //    return;
                //});
            }

        }
    }

    private void SlideEvents()
    {
        // collider küçülecek... kayma animasyonu yapılacak... 
        if (isFirstLevel)
        {
            GetComponentInChildren<Animator>().speed = 1f;
            //KarakterPaketiMovement.instance._speed = 8;
            controlAnimationPanel.SetActive(false);
            rightControlPanel.SetActive(false);
            leftContolPanel.SetActive(false);
            slideControlPanel.SetActive(false);

        }
        _tozEfekti.Play();
        SlideAnim();
    }

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

            GameController.instance.isContinue = false;
            // GetComponent<Collider>().enabled = false;
        

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
    
        transform.parent.transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.parent.transform.position = Vector3.zero;
        transform.position = new(0, 0.4f, 0);
        GameController.instance.isContinue = false;
        GameController.instance.score = 0;
        transform.position = new Vector3(0, transform.position.y, 0);
        GetComponent<Collider>().enabled = true;
        isEnableForSwipe = true;
    }

    public void PreStartingEvents()
    {
        RunAnim();
        GameController.instance.isContinue = true;
    }

 

    #region ANIMATIONS .....


    private void RunAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("run");
        StartCoroutine(DelayAndResetAnims());
    }

    private void IdleAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("idle");
        StartCoroutine(DelayAndResetAnims());
    }

    private void JumpAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("jump");
        StartCoroutine(DelayAndResetAnims());
    }

    private void SlideAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("slide");
        StartCoroutine(DelayAndResetAnims());
    }

    private void CrashAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("struggle");
        StartCoroutine(DelayAndResetAnims());
    }

    private void RopeJumpAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("ropejump");
        StartCoroutine(DelayAndResetAnims());
    }

    private void RopePosAnim()
    {
        GetComponentInChildren<Animator>().SetTrigger("throw");
        StartCoroutine(DelayAndResetAnims());
    }


    private IEnumerator DelayAndResetAnims()
    {
        yield return new WaitForSeconds(.05f);
        GetComponentInChildren<Animator>().ResetTrigger("idle");
        GetComponentInChildren<Animator>().ResetTrigger("jump");
        GetComponentInChildren<Animator>().ResetTrigger("run");
        GetComponentInChildren<Animator>().ResetTrigger("slide");
        GetComponentInChildren<Animator>().ResetTrigger("struggle");
        GetComponentInChildren<Animator>().ResetTrigger("ropejump");
        GetComponentInChildren<Animator>().ResetTrigger("ropepos");
        GetComponentInChildren<Animator>().ResetTrigger("throw");
    }

    #endregion

}
