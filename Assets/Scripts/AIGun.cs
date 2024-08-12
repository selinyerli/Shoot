using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIGun : MonoBehaviour
{
    public GameObject Player;
    public float Distance;
    public bool isAngered;
    public NavMeshAgent _agent;

    [Header("Gun Settings")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public float range = 100;
    public float impactForce = 30f;
    public float damage = 10;
    public int reservedAmmoCapacity = 270;
    public Camera fpsCam;
    public GameObject impactEffect;

    private bool _canShoot;
    private int _currentAmmoInClip;
    private int _ammoInReserve;

    [Header("Aiming Settings")]
    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    public float aimSmoothing = 10;

    public float attackRange = 10f;
    public AIAttack aiAttack;

    void Start()
    {
        _currentAmmoInClip = clipSize;
        _ammoInReserve = reservedAmmoCapacity;
        _canShoot = true;
        //_agent = GetComponent<NavMeshAgent>();
        aiAttack = GetComponent<AIAttack>();

        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent is not assigned or missing.");
        }

        if (aiAttack == null)
        {
            Debug.LogWarning("AIAttack component is not assigned. Adding it dynamically.");
            aiAttack = gameObject.AddComponent<AIAttack>();
            if (aiAttack == null)
            {
                Debug.LogError("Failed to add AIAttack component.");
            }
        }

        if (Player == null)
        {
            Debug.LogError("Player GameObject is not assigned.");
        }

        if (fpsCam == null)
        {
            Debug.LogError("Camera is not assigned.");
        }

        if (impactEffect == null)
        {
            Debug.LogError("ImpactEffect is not assigned.");
        }

        if (normalLocalPosition == Vector3.zero)
        {
            normalLocalPosition = transform.localPosition;
        }
        if (aimingLocalPosition == Vector3.zero)
        {
            aimingLocalPosition = transform.localPosition + Vector3.forward * 0.5f;
        }
    }

    void Update()
    {
        Distance = Vector3.Distance(Player.transform.position, transform.position);

        isAngered = Distance <= 50f;

        if (isAngered)
        {
            _agent.isStopped = false;
            _agent.SetDestination(Player.transform.position);

            if (Distance <= attackRange)
            {
                _agent.isStopped = true;
                if (aiAttack != null)
                {
                    aiAttack.Attack(); // Ensure AI attacks player when in range
                }
                else
                {
                    Debug.LogError("AIAttack component is null.");
                }
            }

            DetermineAim();
            if (Input.GetMouseButton(0) && _canShoot && _currentAmmoInClip > 0)
            {
                _canShoot = false;
                _currentAmmoInClip--;
                StartCoroutine(ShootGun());
            }
            else if (Input.GetKeyDown(KeyCode.R) && _currentAmmoInClip < clipSize && _ammoInReserve > 0)
            {
                int amountNeeded = clipSize - _currentAmmoInClip;
                if (amountNeeded >= _ammoInReserve)
                {
                    _currentAmmoInClip += _ammoInReserve;
                    _ammoInReserve -= amountNeeded;
                }
                else
                {
                    _currentAmmoInClip = clipSize;
                    _ammoInReserve -= amountNeeded;
                }
            }
        }
        else
        {
            _agent.isStopped = true;
        }
    }

    void DetermineAim()
    {
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);
        transform.localPosition = desiredPosition;
    }

    void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;
    }

    IEnumerator ShootGun()
    {
        DetermineRecoil();
        RayCastForAI();
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }

    void RayCastForAI()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            AIHealth target = hit.transform.GetComponent<AIHealth>();
            if (target != null)
            {
                target.Takedamage(damage);
            }
            else
            {
                Debug.LogError("AIHealth component is missing on the hit object: " + hit.transform.name);
            }


            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }
    void RayCastForPlayer()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log("Hit object name: " + hit.transform.name);

            // Çarpýlan nesnenin tag'ini kontrol et
            if (hit.transform.CompareTag("Player"))
            {
                AIHealth target = hit.transform.GetComponent<AIHealth>();
                if (target != null)
                {
                    target.Takedamage(damage);
                }
                else
                {
                    Debug.LogError("AIHealth component is missing on the hit object: " + hit.transform.name);
                }
            }
            else
            {
                Debug.Log("Hit object is not tagged as Player.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit any object within range.");
        }
    }
}