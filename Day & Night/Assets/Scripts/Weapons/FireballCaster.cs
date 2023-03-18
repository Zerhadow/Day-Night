using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireballCaster : MonoBehaviour
{
    [SerializeField] GameObject fireball = null;
    [SerializeField] GameObject loadedBall = null;
    [SerializeField] Transform firePoint = null;
    [SerializeField] float startTime = 0.05f;
    [SerializeField] float recoveryTime = 1f;

    bool canCast = true;
    Camera cam = null;

    // Start is called before the first frame update
    void Start()
    {
        cam = transform.parent.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1) && canCast)
        {
            StartCoroutine(cast());
        }

    }

    void OnEnable()
    {
        canCast = true;
    }

    IEnumerator cast() {
        canCast = false;
        yield return new WaitForSeconds(startTime);
        if (!isActiveAndEnabled)
        {
            canCast = true;
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

            GameObject newBolt = Instantiate(fireball, firePoint.position, Quaternion.identity);
            newBolt.transform.forward = direction;
            loadedBall.SetActive(false);
            yield return new WaitForSeconds(recoveryTime);

            canCast = true;
            loadedBall.SetActive(true);
        }
    }
}
