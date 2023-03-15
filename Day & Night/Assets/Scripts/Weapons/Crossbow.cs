using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    [SerializeField] GameObject bolt = null;
    [SerializeField] GameObject loadedBolt = null;
    [SerializeField] Transform firePoint = null;
    [SerializeField] float startTime = 0.05f;
    [SerializeField] float recoveryTime = 1f;

    bool canShoot = true;
    Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.parent.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(shoot());
        }

    }

    IEnumerator shoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(startTime);
        if (!isActiveAndEnabled)
        {
            canShoot = true;
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

            GameObject newBolt = Instantiate(bolt, firePoint.position, Quaternion.identity);
            newBolt.transform.forward = direction;
            loadedBolt.SetActive(false);
            yield return new WaitForSeconds(recoveryTime);

            canShoot = true;
            loadedBolt.SetActive(true);
        }
    }
}
