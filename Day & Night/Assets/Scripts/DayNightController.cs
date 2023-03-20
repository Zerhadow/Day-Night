using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [Header("Environment Light Source")]
    [SerializeField] Light _skyLight;
    [SerializeField] float _transLength;

    [Header("Light Hues")]
    [SerializeField] Color[] _waveLight;
    [SerializeField] Color _nightLight;
    [SerializeField] Color _eclipseLight;
    
    [Header("Light Angles")]
    [SerializeField] Vector3[] _waveAngle;
    [SerializeField] Vector3 _nightAngle;
    [SerializeField] Vector3 _eclipseAngle;

    [Header("Light Intensities")]
    [SerializeField] float _dayIntense = 2.0f;
    [SerializeField] float _nightIntense = 2.0f;
    [SerializeField] float _eclipseIntense = 2.0f;

    [Header("Skyboxes")]
    [SerializeField] Material _daySky;
    [SerializeField] Material _nightSky;
    [SerializeField] Material _eclipseSky;

    [Header("Sun/Moon")]
    [SerializeField] GameObject _sun;
    [SerializeField] GameObject _eclipse;
    [SerializeField] Vector3 _rotation;

    private float t = 0f; 
    private float timer = 0f;
    private int index = 0;

    private Color col1, col2;
    private Vector3 angle1, angle2;
    private Vector3 sun1, sun2;

    // Start is called before the first frame update
    void Start()
    {
        if(_waveLight.Length == _waveAngle.Length)
        {
            timer = _transLength;
            RenderSettings.skybox = _daySky;
            _sun.transform.eulerAngles = new Vector3(-10, 1, -6);
        }
        else
        {
            Debug.Log("Ya done f'ed up A-Array");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Test Inputs:
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    UpdateSkyNextWave();
        //}
        //else if (Input.GetKeyDown(KeyCode.O))
        //{
        //    UpdateSkyDay();
        //}
        //else if (Input.GetKeyDown(KeyCode.I))
        //{
        //    UpdateSkyNight();
        //}
        //else if (Input.GetKeyDown(KeyCode.U))
        //{
        //    UpdateSkyEclipse();
        //}


        if (timer < _transLength)
        {
            t = Mathf.PingPong(timer, _transLength) / _transLength;
            _skyLight.transform.eulerAngles = Vector3.Lerp(angle1, angle2, t);
            _skyLight.color = Color.Lerp(col1, col2, t);
            _sun.transform.eulerAngles = Vector3.Lerp(sun1, sun2, t);
            timer += Time.deltaTime;
        }
    }

    // Updates the sky light to the next wave settings
    public void UpdateSkyNextWave()
    {
        Debug.Log("Next Wave light");

        if (_waveLight.Length > index + 1)
        {
            col1 = _waveLight[index];
            col2 = _waveLight[index + 1];
            angle1 = _waveAngle[index];
            angle2 = _waveAngle[index + 1];
            sun1 = _sun.transform.eulerAngles;
            sun2 = sun1 + _rotation;
            index++;
            t = 0;
            timer = 0;
        }
    }

    // Updates the skybox and sky light to day settings
    public void UpdateSkyDay()
    {
        // Debug.Log("Start of Day light");

        RenderSettings.skybox = _daySky;
        _skyLight.color = _waveLight[0];
        _skyLight.transform.eulerAngles = _waveAngle[0];
        _sun.transform.eulerAngles = new Vector3(-10, 1, -6);
        
        _skyLight.shadows = LightShadows.Soft;
        _skyLight.intensity = _dayIntense;

        _eclipse.SetActive(false);
        _sun.SetActive(true);
        
        index = 0;
        
    }

    // Updates the skybox and sky light to night settings
    public void UpdateSkyNight()
    {
        // Debug.Log("Night light");

        RenderSettings.skybox = _nightSky;
        _skyLight.color = _nightLight;
        _skyLight.transform.eulerAngles = _nightAngle;
        _sun.transform.eulerAngles = new Vector3(-190, 1, 0);

        _skyLight.shadows = LightShadows.Soft;
        _skyLight.intensity = _nightIntense;

        _eclipse.SetActive(false);
        _sun.SetActive(true);


        index = 100;
    }

    // Updates the skybox and sky light to eclipse settings
    public void UpdateSkyEclipse()
    {
        RenderSettings.skybox = _nightSky;
        _skyLight.color = _eclipseLight;
        _skyLight.transform.eulerAngles = _eclipseAngle;

        _skyLight.shadows = LightShadows.None;
        _skyLight.intensity = _eclipseIntense;

        Transform tempSun = _sun.transform;
        _eclipse.transform.position = tempSun.position;
        _eclipse.transform.rotation = tempSun.rotation;
        _eclipse.SetActive(true);
        _sun.SetActive(false);

        index = 100;
    }
}
