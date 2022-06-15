using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using System;

public class Send_data_to_sheets : MonoBehaviour
{
    #region Changeable FieldNames values
    private string nameField;
    private string emailField;
    private string sectionField;
    private string roomNoField;
    private string deviceIDField;
    private string rollNoField;
    private string BASE_URL_FIELD="";
    #endregion
   public GameObject _name;
   public GameObject _email;
   public GameObject _section;
   public GameObject _roomNo;
   private GameObject _deviceID;
   public GameObject _rollNo;
   
   private string Name;
   private string Email;
   private string Section;
   private string RoomNo;
   private string Device_id;
   private string RollNo;
   private string resetText="";
   void Awake(){
        
       PlayerPrefs.GetFloat("TimeLeftForNextAttendance",timeLeft);
       Input.location.Start();
   }
   

   [SerializeField]private TextMeshProUGUI _timerText;
//    [SerializeField]private TextMeshProUGUI _successText;
   [SerializeField]private float timeLeft=10;
   [SerializeField]private bool IsPressed=false;
   [SerializeField]private string BASE_URL= "BASE_URL_FIELD";

   IEnumerator Push(string _name,string _email,string _section,string _roomNo,string _deviceID,string _rollNo){
       WWWForm form = new WWWForm();
        form.AddField(nameField,_name);
        form.AddField(emailField,_email);
        form.AddField(sectionField,_section);
        form.AddField(roomNoField,_roomNo);
        form.AddField(deviceIDField,_deviceID);
        form.AddField(rollNoField,_rollNo);
        // byte[] rawData = form.data;
        UnityWebRequest www=UnityWebRequest.Post(BASE_URL,form);
        yield return www.SendWebRequest();
        if(www.result!=UnityWebRequest.Result.Success){
            Debug.Log("Error");
        }
        else{
            Debug.Log("Success");
            
        }
        
   }
   public void Send_Data(){ //onclick event button dabane par
       Name=_name.GetComponent<TMP_InputField>().text;
       Email=_email.GetComponent<TMP_InputField>().text;
       Section=_section.GetComponent<TMP_InputField>().text;
       RoomNo=_roomNo.GetComponent<TMP_Dropdown>().options[_roomNo.GetComponent<TMP_Dropdown>().value].text;//[bracket me bss value li hai]
       RollNo=_rollNo.GetComponent<TMP_InputField>().text;
       Device_id=SystemInfo.deviceUniqueIdentifier;
       IsPressed=true;
       _name.GetComponent<TMP_InputField>().text=resetText;
       _email.GetComponent<TMP_InputField>().text=resetText;
       _section.GetComponent<TMP_InputField>().text=resetText;
       _rollNo.GetComponent<TMP_InputField>().text=resetText;
       
       StartCoroutine(Push(Name,Email,Section,RoomNo,Device_id,RollNo));
   }
   
   void Update(){
       if(IsPressed){
           
           GetComponent<UnityEngine.UI.Button>().interactable=false;
              timeLeft-=Time.deltaTime;
       }
       if(timeLeft<=0){
           GetComponent<UnityEngine.UI.Button>().interactable=true;
           IsPressed=false;
           timeLeft=10;
       }
      // OnGUI();
       
       PlayerPrefs.SetFloat("TimeLeftForNextAttendance",timeLeft);
       PlayerPrefs.Save();
   }
          //Removed from final build
    void OnGUI(){
         if(IsPressed){
             _timerText.enabled=true;
              _timerText.text="Next Attendace After: " +  (timeLeft/60).ToString("0") + " Minutes";
         }else{
             _timerText.enabled=false;
             
         }
    }

    public void DropDownChangeSubject(int val){
        if(val==1){
            //CN
            BASE_URL_FIELD ="https://forms.gle/S9nyLDyhtS2SSbi38";
            nameField = "entry.530988602";
            rollNoField="entry.176875534";
            sectionField="entry.2097425196";
            emailField="entry.329829510";
            roomNoField="entry.1180993933";
            deviceIDField="entry.122267119";
        }else if(val==2){
            //Testing
             BASE_URL_FIELD ="https://forms.gle/87LEFqYkta8c7Gvs9";
            nameField = "entry.1021337098";
            rollNoField="entry.1256154275";
            sectionField="entry.864572195";
            emailField="entry.1167709520";
            roomNoField="entry.1147606667";
            deviceIDField="entry.885456707";
        }
    }
}

        


