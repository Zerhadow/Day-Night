using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] GameObject bolt = null;
    [SerializeField] GameObject fullCrossbow = null;
    [SerializeField] GameObject emptyCrossbow = null;
    [SerializeField] Transform firePoint = null;
    [SerializeField] float startTime = 0.05f;
    [SerializeField] float recoveryTime = 1f;

    [SerializeField] AudioClip _crossbow;
    private AudioSource thwap;
    [SerializeField] AudioClip _loadBolt;
    private AudioSource loadBolt;

    bool canShoot = true;
    Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        thwap = gameObject.AddComponent<AudioSource>();
        thwap.clip = _crossbow;
        loadBolt = gameObject.AddComponent<AudioSource>();
        loadBolt.clip = _loadBolt;

        cam = transform.parent.GetComponent<Camera>();
        fullCrossbow.SetActive(true);
        emptyCrossbow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(shoot());
        }

    }

    void OnEnable()
    {
        canShoot = true;
    }

    IEnumerator shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(startTime);
        if (!isActiveAndEnabled)
        {
            canShoot = true;
            loadBolt.Play();
        }
        else
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            Vector3 target;
            if(Physics.Raycast(ray, out hit))
                target = hit.point;
            else
                target = ray.GetPoint(100);
            Vector3 direction = target - firePoint.position;

            thwap.Play();

            GameObject newBolt = Instantiate(bolt, firePoint.position, Quaternion.identity);
            newBolt.transform.forward = direction;
            fullCrossbow.SetActive(false);
            emptyCrossbow.SetActive(true);
            yield return new WaitForSeconds(recoveryTime);

            canShoot = true;
            fullCrossbow.SetActive(true);
            emptyCrossbow.SetActive(false);
        }
    }
}
