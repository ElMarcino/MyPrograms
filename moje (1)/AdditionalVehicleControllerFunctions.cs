using NWH.VehiclePhysics2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AdditionalVehicleControllerFunctions : MonoBehaviour
{
    private Rigidbody rb;
    private VehicleController vc;  
    private Transform playerCar;

    //Angle indicator UI
    [SerializeField]
    private Transform uiSideIndicator;
    [SerializeField]
    private Transform uiBackIndicator;
    [SerializeField]
    private Text uiSideAngleText;
    [SerializeField]
    private Text uiBackAngleText;

    //Where the vehicle should be teleported after Respawn Button
    public GameObject SpawnPoint;

    //Maxiumim Speed the vehicle should gain
    public float maxSpeed;

    //Drive Change
    [SerializeField]
    private bool AllWheelDrive = true;
    private NWH.VehiclePhysics2.Powertrain.DifferentialComponent RearWheelDrive;
    public GameObject DriveChangeIndicator2H;
    public GameObject DriveChangeIndicator4H;
    public bool DoesCarHasToBeStopped;

    //Dirt On Screen
    private float timeToSpawn;
    private float timeSinceSpawn;



    //Engine Temperature
    [SerializeField]
    private float lowestOilTemperature;
    [SerializeField]
    private float highestOilTemperature;
    [SerializeField]
    private float lowestWaterTemperature;
    [SerializeField]
    private float highestWaterTemperature;
    [SerializeField]
    private float optimalRPM;
    private float oilTemp;
    private float waterTemp;
    //Engine Temperature UI
    [SerializeField]
    private ProgressBar oilTempIndicator;
    [SerializeField]
    private ProgressBar waterTempIndicator;
    [SerializeField]
    private Text oilTempText;
    [SerializeField]
    private Text waterTempText;
    //FuelLevel
    private float fuelLevel;
    [SerializeField]
    private float tankCapacity;
    [SerializeField]
    private ProgressBar fuelBar;
    [SerializeField]
    private Text fuelText;


    //BatteryLevel
    [SerializeField]
    private float batteryCapacity;
    private float batteryChargeLevel;
    [SerializeField]
    private Text batteryText;
    [SerializeField]
    private ProgressBar batteryBar;
    public enum BatteryState
    {
        NotCharging,
        ChargingSlight,
        OverCharging,
        NotDamaged
    }
    public BatteryState currentBatteryState;




    private void FixedUpdate()
    {
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void Start()
    {
        batteryChargeLevel = batteryCapacity;
        fuelLevel = tankCapacity;
        rb = gameObject.GetComponent<Rigidbody>();
        vc = gameObject.GetComponent<VehicleController>();
        RearWheelDrive = vc.powertrain.differentials[0];
        playerCar = gameObject.GetComponent<Transform>();
        DriveChangeIndicator4H.SetActive(true);

    }

    public void Update()
    {

        BatteryCharge();
        Fuel();
        DirtOnScreen();
        DragSetter();
        RotationDisplay();
        if (Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
        }
        StartCoroutine(DriveChange());

        SetEngineTemperature();
     
    }


    float carRotDump = 0;
    float carAngleDump = 0;
    Vector3 carRot;
    void RotationDisplay()
    {

        carRot = playerCar.eulerAngles;

        carRot.x = (carRot.x > 180) ? carRot.x - 360 : carRot.x;
        carRot.z = (carRot.z > 180) ? carRot.z - 360 : carRot.z;
        
        carRotDump = Mathf.Lerp(carRotDump, carRot.x, Time.deltaTime*5);
        carAngleDump = Mathf.Lerp(carAngleDump, carRot.z, Time.deltaTime * 5);

        uiSideIndicator.rotation = Quaternion.Euler(new Vector3(uiSideIndicator.rotation.eulerAngles.x, uiSideIndicator.rotation.eulerAngles.y, carRotDump));
        uiBackIndicator.rotation = Quaternion.Euler(new Vector3(uiBackIndicator.rotation.eulerAngles.x, uiBackIndicator.rotation.eulerAngles.y, carAngleDump));

        uiBackAngleText.text = carRot.z.ToString("F0");
        uiSideAngleText.text = carRot.x.ToString("F0");
    }
    void DragSetter()
    {
        float drag = vc.Wheels[0].wheelController.activeFrictionPreset.DragPreset;
        rb.drag = drag;
        //Debug.Log(drag);
        Debug.Log(vc.Wheels[0].wheelController.activeFrictionPreset.name);
    }
    void Respawn()
    {
        rb.velocity = new Vector3(0, 0, 0);
        playerCar.transform.position = SpawnPoint.transform.position;
        playerCar.transform.rotation = SpawnPoint.transform.rotation;
    }
    public IEnumerator DriveChange()
    {
        if (Input.GetKeyDown(KeyCode.Q) && AllWheelDrive)
        {
            if (DoesCarHasToBeStopped)
            {
                if (Mathf.Abs(rb.velocity.magnitude) < 1)
                {
                    DriveChange2WD();
                    yield return new WaitForSeconds(1);
                    StopCoroutine(DriveChange());
                }
                else Debug.Log("Car Has To be Stopped");
            }
            else
            {
                DriveChange2WD();
                yield return new WaitForSeconds(1);
                StopCoroutine(DriveChange());
            }

        }
        if(Input.GetKeyDown(KeyCode.Q) && !AllWheelDrive)
        {
            if (DoesCarHasToBeStopped)
            {
                if (Mathf.Abs(rb.velocity.magnitude) < 1)
                {
                    DriveChangeAWD();
                    yield return new WaitForSeconds(1);
                    StopCoroutine(DriveChange());
                }
                else Debug.Log("Car Has To Be Stopped");
            }
            else
            {
                DriveChangeAWD();
                yield return new WaitForSeconds(1);
                StopCoroutine(DriveChange());
            }
            
        }
    }

    public void DriveChangeAWD()
    {
        var diffs = vc.powertrain.differentials;
        diffs.Insert(0, RearWheelDrive);
        vc.powertrain.transmission.SetOutput(vc.powertrain.differentials[1].name);
        vc.powertrain.Initialize();
        AllWheelDrive = true;
        DriveChangeIndicator2H.SetActive(false);
        DriveChangeIndicator4H.SetActive(true);
    }
    public void DriveChange2WD()
    {
        var diffs = vc.powertrain.differentials;
        diffs.RemoveAt(0);
        vc.powertrain.transmission.SetOutput(vc.powertrain.differentials[1].name);
        vc.powertrain.Initialize();
        AllWheelDrive = false;
        DriveChangeIndicator2H.SetActive(true);
        DriveChangeIndicator4H.SetActive(false);
    }


    public void DirtOnScreen()
    {   
        foreach(var wheel in vc.Wheels)
        {
            if (wheel.HasLongitudinalSlip && wheel.wheelController.activeFrictionPreset.name == "MudTireFrictionPreset")
            {
                timeToSpawn = Mathf.Abs(wheel.LongitudinalSlip) / 10;
                
                timeSinceSpawn += Time.deltaTime;
                if (timeSinceSpawn >= timeToSpawn)
                {
                    ObjectPool.Instance.RequestImage();
                    timeSinceSpawn = 0;
                }
            }
        }       
    }
    
    public void SetEngineTemperature()
    {
        float currentRPM = vc.powertrain.transmission.RPM;
        float currentSpeed = rb.velocity.magnitude;
        float RPMdelta = currentRPM - optimalRPM;



        if (oilTemp < lowestOilTemperature)
            oilTemp = lowestOilTemperature;
        else if(oilTemp > highestOilTemperature)
            Debug.Log("Engine Blown");

        if (waterTemp < lowestWaterTemperature)
            waterTemp = lowestWaterTemperature;
        else if (waterTemp > highestWaterTemperature)
            Debug.Log("Radiator Blown");


        if (RPMdelta > 0)
        {
            oilTemp += (0.3f / (currentSpeed + 1f)) * Time.deltaTime;
            waterTemp += (0.7f / (currentSpeed + 1f)) * Time.deltaTime;

        }
            
        else if(RPMdelta <= 0)
        {
            oilTemp -= ((currentSpeed / 50) + 0.1f) * Time.deltaTime;
            waterTemp -= ((currentSpeed / 50) + 0.1f) * Time.deltaTime;
        }

        oilTempIndicator.current = oilTemp;
        oilTempIndicator.minimum = (2 * lowestOilTemperature) - highestOilTemperature;
        oilTempIndicator.maximum = highestOilTemperature;
        oilTempText.text = oilTemp.ToString("F0") + " C";

        waterTempIndicator.current = waterTemp;
        waterTempText.text = waterTemp.ToString("F0") + " C";
        waterTempIndicator.maximum = highestWaterTemperature;
        waterTempIndicator.minimum = (2 * lowestWaterTemperature) - highestWaterTemperature ;
    }

    private void Fuel()
    { 
        float currentRPM = vc.powertrain.transmission.RPM;
        fuelLevel -= currentRPM / 20000 * Time.deltaTime;
        fuelText.text = "Fuel Level: " + fuelLevel.ToString("F0") + " / " + tankCapacity.ToString("F0");
        if(fuelLevel < 0)
        {
            fuelLevel = 0;
            vc.powertrain.engine.Stop();
            vc.powertrain.engine.ignition = false;
        }

        fuelBar.maximum = tankCapacity;
        fuelBar.current = fuelLevel;
        fuelBar.minimum = 0;
    }

    private void BatteryCharge()
    {

        float currentRPM = vc.powertrain.transmission.RPM;
        batteryChargeLevel -= currentRPM / 20000 * Time.deltaTime;
        if(batteryChargeLevel < batteryCapacity)
        {
            switch (currentBatteryState)
            {
                case BatteryState.NotDamaged:
                    batteryChargeLevel += currentRPM / 15000 * Time.deltaTime;
                    return;
                case BatteryState.ChargingSlight:
                    batteryChargeLevel += currentRPM / 30000 * Time.deltaTime;
                    return;
                case BatteryState.NotCharging: 
                    return;
            }
        }
        if (currentBatteryState ==  BatteryState.OverCharging)
            batteryChargeLevel += currentRPM / 10000 * Time.deltaTime;

        if (batteryChargeLevel > batteryCapacity + 3 ^ batteryChargeLevel < 0)
        {
            vc.powertrain.engine.Stop();
            vc.powertrain.engine.ignition = false;
        }
        batteryBar.minimum = 0;
        batteryBar.maximum = batteryCapacity;
        batteryBar.current = batteryChargeLevel;
        batteryText.text = batteryChargeLevel.ToString("F0") + "V / " + batteryCapacity + "V";
            
    }
}
