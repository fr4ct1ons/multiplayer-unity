using System;
using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;
using TMPro;


/// <summary>
/// Player manager.
/// Handles fire Input and Beams.
/// </summary>
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] public float currentHealth = 0.0f, maxHealth = 100.0f;
    public float damagePerSecond = 10.0f;

    public static GameObject localPlayerInstance;

    public GameObject playerUiPrefab;

    public TextMeshPro playerTopText;

    #region Private Fields

    [Tooltip("The Beams GameObject to control")]
    [SerializeField]
    private GameObject beams;
    //True, when the user is firing
    bool IsFiring;
    #endregion

    #region MonoBehaviour CallBacks

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
    /// </summary>
    void Awake()
    {
        if (beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        }
        else
        {
            beams.SetActive(false);
        }
        
        currentHealth = maxHealth;

        if (photonView.IsMine)
        {
            PlayerManager.localPlayerInstance = gameObject;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        GameObject newUi = Instantiate(playerUiPrefab);
        playerUiPrefab.GetComponent<PhotonPlayerUI>().SetTarget(this);
        playerTopText.SetText(photonView.Owner.NickName);
    }

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity on every frame.
    /// </summary>
    void Update()
    {

        if (photonView.IsMine)
        {
            ProcessInputs();
        }

        // trigger Beams active state
        if (beams != null && IsFiring != beams.activeInHierarchy)
        {
            beams.SetActive(IsFiring);
        }
        
        #if UNITY_5_4_OR_NEWER
        // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        #endif
    }

    #endregion

    #region Custom

    /// <summary>
    /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
    /// </summary>
    void ProcessInputs()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!IsFiring)
            {
                IsFiring = true;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if (IsFiring)
            {
                IsFiring = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == gameObject)
        {
            return;
        }
        if(!photonView.IsMine)
            return;
        
        
        Debug.Log(other.gameObject);

        PlayerManager otherManager = other.GetComponentInParent<PlayerManager>();
        
        if (otherManager)
        {
            currentHealth -= otherManager.damagePerSecond * Time.deltaTime;
            if (currentHealth <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
        }
        
    }

    #endregion

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(IsFiring);
            stream.SendNext(currentHealth);
        }
        else
        {
            this.IsFiring = (bool) stream.ReceiveNext();
            this.currentHealth = (float)stream.ReceiveNext();
        }
    }
    
    #if UNITY_5_4_OR_NEWER
    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        this.CalledOnLevelWasLoaded(scene.buildIndex);
    }
    #endif

    void CalledOnLevelWasLoaded(int level)
    {
        // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
        
        GameObject newUi = Instantiate(playerUiPrefab);
        playerUiPrefab.GetComponent<PhotonPlayerUI>().SetTarget(this);
        playerTopText.SetText(photonView.Owner.NickName);
    }
}