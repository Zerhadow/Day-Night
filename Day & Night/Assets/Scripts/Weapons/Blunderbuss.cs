using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Blunderbuss : MonoBehaviour
{
    [SerializeField] GameObject pellet = null;
    [SerializeField] Transform firePoint = null;
    [SerializeField] float startTime = 0.05f;
    [SerializeField] float recoveryTime = 1.5f;
    [SerializeField] float horizontalSpread = 5f;
    [SerializeField] float verticalSpread = 5f;
    [SerializeField] int numberOfPellets = 10;

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
        }
        else
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;

            Vector3 target;
            if (Physics.Raycast(ray, out hit))
                target = hit.point;
            else
                target = ray.GetPoint(100);
            Vector3 direction = target - firePoint.position;

            Vector3 axis = Vector3.Cross(direction, Vector3.up);
            for(int i = 0; i < numberOfPellets; i++)
            {
                Vector3 path = direction;
                float hDisplacement = Random.Range(-horizontalSpread / 2, horizontalSpread / 2);
                float vDisplacement = Random.Range(-verticalSpread / 2, verticalSpread / 2);
                path = Quaternion.AngleAxis(vDisplacement, axis) * Quaternion.Euler(0, hDisplacement, 0) * path;
                GameObject newPellet = Instantiate(pellet, firePoint.position, Quaternion.identity);
                newPellet.transform.forward = path;
            }
            yield return new WaitForSeconds(recoveryTime);

            canShoot = true;
        }
    }
}
