using System.Collections;
using UnityEngine;
using TMPro;
using Unity.RemoteConfig;


public class Location_Handler : MonoBehaviour{

    public struct userAttributes{ }
    public struct appAttributes{ }
    public TextMeshProUGUI _GPS_Status;
    public TextMeshProUGUI _latitude;
    public TextMeshProUGUI _longitude;
    public TextMeshProUGUI _altitude;
    public TextMeshProUGUI _rct;
    public TextMeshProUGUI _rct_2;
    
    public TextMeshProUGUI _ope;

             //remote config values
   // [SerializeField]private float Top_Latitude;
    //[SerializeField]private float Bottom_Latitude;
    [SerializeField]private float Top_Longitude;
    [SerializeField]private float Bottom_Longitude;

    #region This is the data in main condition which is changed
    [SerializeField]private float Changeable_Longitude_Bottom;
    [SerializeField]private float Changeable_Longitude_Top;
    #endregion

    #region ROOM DATA(CHANGE/ADD to THIS ONLY)
    private float Bottom_Longitude_112; //112 Bottom longitude
    private float Top_Longitude_112;  //112 Top longitude
    private float Top_Longitude_201; //201 Top longitude
    private float Bottom_Longitude_201; //201 bottom longitude

    private float Top_Longitude_Presentation; //p Top longitude
    private float Bottom_Longitude_Presentation; //p bottom longitude
   
    #endregion

    [SerializeField]public UnityEngine.UI.Button DisableThisButton;
    
   
    private void Awake(){ //called at start
        ConfigManager.FetchCompleted+=SetLocationRemoteConfig;
        ConfigManager.FetchConfigs<userAttributes,appAttributes>(new userAttributes(),new appAttributes());
    }
       

    //Remote Config Part
    void SetLocationRemoteConfig(ConfigResponse response){ //SET REMOTE CONFIG VALUES HERE 
        //Testing
        Top_Longitude=ConfigManager.appConfig.GetFloat("T_Longitude");
        Bottom_Longitude=ConfigManager.appConfig.GetFloat("B_Longitude");
        //112
        Bottom_Longitude_112 = ConfigManager.appConfig.GetFloat("112_BL");
        Top_Longitude_112 = ConfigManager.appConfig.GetFloat("112_TL");
        //201
        Bottom_Longitude_201=ConfigManager.appConfig.GetFloat("201_BL");
        Top_Longitude_201=ConfigManager.appConfig.GetFloat("201_TL");
        //Presentation Room
        Bottom_Longitude_Presentation=ConfigManager.appConfig.GetFloat("P_BL");
        Top_Longitude_Presentation=ConfigManager.appConfig.GetFloat("P_TL");
        
    }
    void Start(){ 
        StartCoroutine(GPSLOCATION());
    }
       
    void Update(){
        ConfigManager.FetchConfigs<userAttributes,appAttributes>(new userAttributes(),new appAttributes());
        if (Input.location.status==LocationServiceStatus.Running)//if gps is on
        {
            // main condition
        //if(room no this){ is room ka longitude }
           if((Input.location.lastData.longitude>=Changeable_Longitude_Bottom && Input.location.lastData.longitude<=Changeable_Longitude_Top))
           {
               _ope.text="You are in class";
               _ope.color = Color.green;
               DisableThisButton.interactable= true; //button on
           }else{
               _ope.text="You are not in class,please move to your respective class";
               _ope.color=Color.red;
               DisableThisButton.interactable=false; //button off
               
           }
        }else{
            _ope.text="Turn on Location";
            DisableThisButton.interactable = false; //button off
        }
        UpdateGPSData();

        //testing
        _rct_2.text = Changeable_Longitude_Bottom.ToString();
    }
    IEnumerator GPSLOCATION(){ //location calculations
        if(!Input.location.isEnabledByUser){
            yield break;
        }
            Debug.Log("User has not enabled GPS");
            Input.location.Start(10,0.1f);
            int maxWait=20;
            while(Input.location.status==LocationServiceStatus.Initializing&&maxWait>0){
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            if(maxWait<1){
                _GPS_Status.text="TIME OUT";
                yield break;
            }
            if(Input.location.status==LocationServiceStatus.Failed){
                _GPS_Status.text="Unable to determine location";
                yield break;
            }
            else{
               _GPS_Status.text="Running";
               InvokeRepeating("UpdateGPSData",0.5f,1);
            }
        }
    
    private void UpdateGPSData(){ //only to update text
        if(Input.location.status==LocationServiceStatus.Running){ //location on hai kya
            _GPS_Status.text="Running";
            _latitude.text="Latitude: " + Input.location.lastData.latitude.ToString(); //tostring mtlb change data type to string
            _longitude.text="Longitude: " +Input.location.lastData.longitude.ToString();
            _altitude.text="Altitude: " +Input.location.lastData.altitude.ToString();
            
        }else{
            _GPS_Status.text="Stop";
        }
    }

    
    public void RoomNumberDropDownChangeLocation(int val){
        if(val==2){ //112 DP-LAB
            _rct.text = "112 DP-LAB";
            Changeable_Longitude_Bottom = Bottom_Longitude_112;
            Changeable_Longitude_Top = Top_Longitude_112;
        }else if(val==3){ // Testing
            _rct.text = "Testing";
            Changeable_Longitude_Bottom = Bottom_Longitude;
            Changeable_Longitude_Top = Top_Longitude;
        }else if(val==1){ // Presentation Room
            _rct.text = "Presentation Room";
            Changeable_Longitude_Bottom = Bottom_Longitude_Presentation;
            Changeable_Longitude_Top = Top_Longitude_Presentation;
        }
    }
}





 