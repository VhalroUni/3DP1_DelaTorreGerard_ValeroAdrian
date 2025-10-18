using System;
using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;
public class PlayerController : MonoBehaviour
{
    Vector3 m_StartPosition;
    Quaternion m_StartRotation;
    float m_Yaw;
    float m_Pitch;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public float m_MinPitch;
    public float m_MaxPitch;
    public Transform m_PitchController;
    public bool m_UseInvertedYaw;
    public bool m_UseInvertedPitch;
    public CharacterController m_CharacterController;
    float m_VerticalSpeed = 0.0f;

    bool m_AngleLocked;
    public float m_Speed;
    public float m_JumpSpeed;
    public float m_SpeedMultiplier;
    public Camera m_Camera;

    [Header("Text")]
    public Text m_AmmoText;
    public Text m_LifeText;
    public Text m_ShieldText;


    [Header("Shoot")]
    public float m_ShootMaxDistance = 50.0f;
    public LayerMask m_ShootLayerMask;
    public GameObject m_ShootParticles;
    public int m_Ammo = 120;
    public int m_TotalMaxAmmo = 120;
    public int m_LoaderSize = 12;
    public int m_CurrentAmmo = 12;
    public float m_CooldownBetweenShots = 0.2f;
    private float m_ShootTimer = 0f;
    public float m_ReloadTime = 2f;
    PoolElements m_ShootParticlesPool;
    private bool m_IsReloading = false;
    private bool m_CanShoot = true;


    [Header("Input")]
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;
    public KeyCode m_ReloadKeyCode = KeyCode.R;
    public int m_ShootMouseButton = 0;

    [Header("Animations")]
    public Animation m_Animation;
    public AnimationClip m_IdleAnimationClip;
    public AnimationClip m_ReloadAnimationClip;
    public AnimationClip m_ShootAnimationClip;

    [Header("Health")]
    public int m_Life = 100;
    public int m_Shield = 100;

    [Header("Debug Input")]
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;

    void Start()
    {
        m_ShootParticlesPool = new PoolElements();
        m_ShootParticlesPool.Init(25, m_ShootParticles);
        PlayerController l_Player = GameManager.GetGameManager().GetPLayer();
        if (l_Player != null)
        {
            l_Player.m_CharacterController.enabled = false;
            l_Player.transform.position = transform.position;
            l_Player.transform.rotation = transform.rotation;
            l_Player.m_CharacterController.enabled = true;
            l_Player.m_StartPosition = transform.position;
            l_Player.m_StartRotation = transform.rotation;
            GameObject.Destroy(gameObject);
            return;
        }
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        DontDestroyOnLoad(gameObject);
        GameManager.GetGameManager().SetPlayer(this);
        Cursor.lockState = CursorLockMode.Locked;
        SetIdleAnimation();
        UpdateAmmoHUD();
        UpdateLifeHUD();
        UpdateShieldHUD();
    }

    void Update()
    {
        if (m_ShootTimer > 0f)
            m_ShootTimer -= Time.deltaTime;

        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");

        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;

        if (!m_AngleLocked)
        {
            m_Yaw = m_Yaw + l_MouseX * m_YawSpeed * Time.deltaTime * (m_UseInvertedYaw ? -1.0f : 1.0f);
            m_Pitch = m_Pitch + l_MouseY * m_PitchSpeed * Time.deltaTime * (m_UseInvertedPitch ? -1.0f : 1.0f);
            m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
            transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
            m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
        }

        Vector3 l_Movement = Vector3.zero;
        float l_YawPiRadins = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90PiRadians = (m_Yaw + 90) * Mathf.Deg2Rad;
        Vector3 l_ForwardDirection = new Vector3(Mathf.Sin(l_YawPiRadins), 0.0f, Mathf.Cos(l_YawPiRadins));
        Vector3 l_RightDirection = new Vector3(Mathf.Sin(l_Yaw90PiRadians), 0.0f, Mathf.Cos(l_Yaw90PiRadians));


        if (Input.GetKey(m_RightKeyCode))
            l_Movement = l_RightDirection;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Movement = -l_RightDirection;

        if (Input.GetKey(m_UpKeyCode))
            l_Movement += l_ForwardDirection;
        else if (Input.GetKey(m_DownKeyCode))
            l_Movement -= l_ForwardDirection;

        float l_SpeedMultiplier = 1.0f;

        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultiplier = m_SpeedMultiplier;

        l_Movement.Normalize();
        l_Movement *= m_Speed * l_SpeedMultiplier * Time.deltaTime;

        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);
        if (m_VerticalSpeed < 0.0f && (l_CollisionFlags & CollisionFlags.Below) != 0) //si estoy cyendoo colisiono con el suelo
        {
            m_VerticalSpeed = 0.0f;
            if (Input.GetKeyDown(m_JumpKeyCode))
                m_VerticalSpeed = m_JumpSpeed;

        }
        else if (m_VerticalSpeed > 0.0f && (l_CollisionFlags & CollisionFlags.Above) != 0) //si estyoy subiendo y colision con un techo  
            m_VerticalSpeed = 0.0f;

        if (CanShoot() && Input.GetMouseButtonDown(0))
            Shoot();
        if (CanReload() && Input.GetKeyDown(m_ReloadKeyCode))
            Reload();
    }
    bool CanReload()
    {
        if (!m_IsReloading && m_CurrentAmmo < m_LoaderSize && m_Ammo > 0)
            return true; 
        return false;
    }
    void Reload()
    {
        m_IsReloading = true;
        m_ShootTimer = m_ReloadTime;
        int m_NeededAmmo = m_LoaderSize - m_CurrentAmmo;

        if (m_Ammo >= m_NeededAmmo)
        {
            m_CurrentAmmo += m_NeededAmmo;
            m_Ammo -= m_NeededAmmo;
        }
        else
        {
            m_CurrentAmmo += m_Ammo;
            m_Ammo = 0;
        }

        SetReloadAnimation();
        UpdateAmmoHUD();
        m_IsReloading = false;
    }
    bool CanShoot()
    {
        if (!m_IsReloading && m_CanShoot && m_CurrentAmmo > 0 && m_ShootTimer <= 0f)
            return true;
        return false;
    }
    void Shoot()
    {
        m_CanShoot = false;
        m_ShootTimer = m_CooldownBetweenShots;
        SetShootAnimation();
        if(m_CurrentAmmo > 0)
        {
            Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_ShootMaxDistance, m_ShootLayerMask.value))
            {
                if (l_RaycastHit.collider.CompareTag("HitCollider"))
                    l_RaycastHit.collider.GetComponent<HitCollider>().Hit();
                else if (l_RaycastHit.collider.CompareTag("Target"))
                    l_RaycastHit.collider.GetComponent<ShootingGallery>().HitTarget();
                else
                    CreateShootHitParticles(l_RaycastHit.point, l_RaycastHit.normal);
            }
            m_CurrentAmmo--;
            UpdateAmmoHUD();
        }
        else
        {
            Reload();
        }
        m_CanShoot = true;
    }
    void CreateShootHitParticles(Vector3 Position, Vector3 Normal)
    {
        GameObject l_ShootParticles = m_ShootParticlesPool.GetNextElement();
        l_ShootParticles.transform.position = Position;
        l_ShootParticles.transform.rotation = Quaternion.LookRotation(Normal);
        l_ShootParticles.SetActive(true);

    }
    void SetIdleAnimation()
    {
        m_Animation.CrossFade(m_IdleAnimationClip.name);
    }
    void SetReloadAnimation()
    {
        m_Animation.CrossFade(m_ReloadAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.0f);
    }
    void SetShootAnimation()
    {
        m_Animation.CrossFade(m_ShootAnimationClip.name, 0.1f);
        m_Animation.CrossFadeQueued(m_IdleAnimationClip.name, 0.0f);
    }
    public void AddAmmo(int Ammo)
    {
        m_Ammo += Ammo;
        UpdateAmmoHUD();
    }
    public void UpdateAmmoHUD()
    {
        if (m_AmmoText != null)
            m_AmmoText.text = m_CurrentAmmo + " / " + m_Ammo;
    }
    public void AddLife(int Life)
    {
        m_Life += Life;
        UpdateLifeHUD();
    }
    public void UpdateLifeHUD()
    {
        if (m_LifeText != null)
            m_LifeText.text = "Life: " + m_Life;
    }
    public void AddShield(int Shield)
    {
        m_Shield += Shield;
        UpdateShieldHUD();
    }
    public void UpdateShieldHUD()
    {
        if (m_ShieldText != null)
            m_ShieldText.text = "Shield: " + m_Shield;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            Item l_Item = other.GetComponent<Item>();
            if (l_Item.CanPick())
                l_Item.Pick();
        }
        else if (other.CompareTag("DeadZone"))
            Kill();
        else if (other.CompareTag("SceneChanger"))
            SceneManager.LoadSceneAsync("Level2Scene");
    }

    void Kill()
    {
        GameManager.GetGameManager().m_Fade.FadeIn(() => {
            GameManager.GetGameManager().RestartLevel();
        });
        
    }
    public void Restart()
    {
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_CharacterController.enabled = true;
    }
}