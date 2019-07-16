using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO.Ports;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Text;   // for Encoding

public class DropZone : MonoBehaviour, IDropHandler{

    public string DeviceName = "HMSoft";
    public string ServiceUUID = "FFE0";
    public string Characteristic = "FFE1";

    // this is our hm10 device
    private string _hm10;

     enum States
    {
        None,
        Scan,
        Connect,
        Subscribe,
        Unsubscribe,
        Disconnect,
        Communication,
    }

    private bool _workingFoundDevice = true;
    private bool _connected = false;
    private float _timeout = 0f;
    private States _state = States.None;

    void Reset ()
    {
        _workingFoundDevice = false;    // used to guard against trying to connect to a second device while still connecting to the first
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _hm10 = null;
    }

    public Dictionary<int, string> dict = new Dictionary<int, string>() {
            {48, "0"},  // C
            {49, "1"},  // C#
            {50, "2"},  // D
            {51, "3"},  // D#
            {52, "4"},  // E
            {53, "5"},  // F
            {54, "6"},  // F#
            {55, "7"},  // G
            {56, "8"},  // G#
            {57, "9"},  // A
            {58, "a"},  // A#
            {59, "b"},  // B
            {60, "c"},  // C
            {61, "d"},  // C#
            {62, "e"},  // D
            {63, "f"},  // D#
            {64, "g"},  // E
            {65, "h"},  // F
            {66, "i"},  // F#
            {67, "j"},  // G
            {68, "k"},  // G#
            {69, "l"},  // A
            {70, "m"},  // A#
            {71, "n"},  // B
            {72, "o"},  // C
            };

    public List<Draggable> CdList;

    public string title;
    public Draggable d;
    public GameObject scroll;
    private bool canRotatePlayer = false;
    public SerialPort stream;

    public Text HM10_Status;
    public bool enabled;

    void Start(){
        HM10_Status.text = "Initializing...";

        Reset();
        BluetoothLEHardwareInterface.Initialize (true, false, () => {
            
            SetState (States.Scan, 0.1f);
            HM10_Status.text = "Initialized";

        }, (error) => {
            
            BluetoothLEHardwareInterface.Log ("Error: " + error);
        });
        enabled = false;
    }

    public void OnDrop(PointerEventData eventData){
        d = eventData.pointerDrag.GetComponent<Draggable>();
        if(d != null){
            d.parentToReturnTo = this.transform;
        }
        canRotatePlayer = true;
        title = d.gameObject.name;
        print("Playing Track: " + title);
        StartCoroutine(SendArduino());
        scroll.GetComponent<ScrollRect>().enabled = false;
        foreach (Draggable cd in CdList){
            if (cd.name != title){
                cd.draggable = false;
            }
        }
    }

    void Update(){
        RotatePlayer();
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                switch (_state)
                {
                case States.None:
                    break;

                case States.Scan:
                    HM10_Status.text = "Scanning for HM10 devices...";

                    BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {

                        // we only want to look at devices that have the name we are looking for
                        // this is the best way to filter out devices
                        if (name.Contains (DeviceName))
                        {
                            _workingFoundDevice = true;

                            // it is always a good idea to stop scanning while you connect to a device
                            // and get things set up
                            BluetoothLEHardwareInterface.StopScan ();

                            // add it to the list and set to connect to it
                            _hm10 = address;

                            HM10_Status.text = "Found HM10";

                            SetState (States.Connect, 0.5f);

                            _workingFoundDevice = false;
                        }

                    }, null, false, false);
                    break;

                case States.Connect:

                    HM10_Status.text = "Connecting to HM10";

                    // note that the first parameter is the address, not the name. I have not fixed this because of backwards compatiblity.
                    // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                    // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                    // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                    BluetoothLEHardwareInterface.ConnectToPeripheral (_hm10, null, null, (address, serviceUUID, characteristicUUID) => {

                        if (IsEqual (serviceUUID, ServiceUUID))
                        {
                            // if we have found the characteristic that we are waiting for
                            // set the state. make sure there is enough timeout that if the
                            // device is still enumerating other characteristics it finishes
                            // before we try to subscribe
                            if (IsEqual (characteristicUUID, Characteristic))
                            {
                                _connected = true;
                                SetState (States.Subscribe, 2f);
                            }
                        }
                    }, (disconnectedAddress) => {
                        BluetoothLEHardwareInterface.Log ("Device disconnected: " + disconnectedAddress);
                        HM10_Status.text = "Disconnected";
                    });
                    break;

                case States.Subscribe:
                    HM10_Status.text = "Connected to HM10";
                    enabled = true;

                    BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_hm10, ServiceUUID, Characteristic, null, (address, characteristicUUID, bytes) => {
                    });

                    // set to the none state and the user can start sending and receiving data
                    _state = States.None;

                    break;

                case States.Unsubscribe:
                    BluetoothLEHardwareInterface.UnSubscribeCharacteristic (_hm10, ServiceUUID, Characteristic, null);
                    SetState (States.Disconnect, 4f);
                    break;

                case States.Disconnect:
                    if (_connected)
                    {
                        BluetoothLEHardwareInterface.DisconnectPeripheral (_hm10, (address) => {
                            BluetoothLEHardwareInterface.DeInitialize (() => {
                                
                                _connected = false;
                                _state = States.None;
                            });
                        });
                    }
                    else
                    {
                        BluetoothLEHardwareInterface.DeInitialize (() => {
                            
                            _state = States.None;
                        });
                        enabled = false;
                    }
                    break;
                }
            }
        }
     }

    private IEnumerator ReturnPlayer(float waitTime){
        yield return new WaitForSeconds(waitTime);
        canRotatePlayer = false;
        scroll.GetComponent<ScrollRect>().enabled = true;
        d.transform.SetParent(d.parent);
        d.parentToReturnTo = d.parent;
        d = null;
        foreach (Draggable cd in CdList){
            if (cd.name != title){
                cd.draggable = true;
            }
        }
    }

    private IEnumerator SendArduino(){
        yield return new WaitForSeconds(1.0f);  //wait awhile before playing song
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        string[] lines = d.TextFile.text.Split('\n');
        foreach (string line in lines){
            if(!string.IsNullOrWhiteSpace(line)){    // beat contains notes
                print(line);
                foreach(string note in Regex.Split(line, " ")){
                    int key = int.Parse(note);
                    print("sending " + key + ": " + dict[key]);
                    // stream.Write(dict[key]);
                    var data = Encoding.UTF8.GetBytes (dict[key]);
                    BluetoothLEHardwareInterface.WriteCharacteristic (_hm10, ServiceUUID, Characteristic, data, data.Length, false, (characteristicUUID) => {
                        BluetoothLEHardwareInterface.Log ("Write Succeeded");
                    });
                }   
            }
            yield return wait; //tempo of song
        }
        StartCoroutine(ReturnPlayer(1.0f));
    }

    protected void RotatePlayer(){
        if(canRotatePlayer){
            d.transform.Rotate(Vector3.forward, -90.0f * Time.deltaTime);
        }
    }
    
    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID (uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID (uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    string FullUUID (string uuid)
    {
        return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
    }

    void SetState (States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }
}