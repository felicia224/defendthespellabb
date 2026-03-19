using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;

public class ArduinoForceListener : MonoBehaviour
{
    public TheForce theForce; // Dra in ert TheForce-GameObject här
    public Lightsaber lightsaber;

    private MqttClient client;
    private string lastMessage; // lagrar senaste meddelande från Arduino

    void Start()
    {
        // Koppla upp mot HiveMQ public broker på port 1883
        client = new MqttClient("broker.hivemq.com", 1883, false, null, null, MqttSslProtocols.None);

        // Anslut utan användarnamn/lösenord
        client.Connect("UnityForceClient");

        // Prenumerera på topic där Arduino skickar Force On/Off
        client.Subscribe(new string[] { "IM/TESTING/GROUP2" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        // Callback när meddelande kommer
        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
    }

    // Denna körs i separat tråd av MQTT, så vi lagrar meddelandet
    private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        lastMessage = Encoding.UTF8.GetString(e.Message);
        Debug.Log("Arduino skickade: " + lastMessage);
    }

    void Update()
    {
        if (string.IsNullOrEmpty(lastMessage)) return;

        if (lastMessage == "Force On")
        {
            theForce.HandleButtonPress();
        }

        if (lastMessage == "Button")
        {
            lightsaber.StartScaling();
        }


        lastMessage = "";
    }

    void OnDestroy()
    {
        if (client != null && client.IsConnected)
            client.Disconnect();
    }

    public void SendVibration()
    {
        if (client != null && client.IsConnected)
        {
            client.Publish("Unity/Vibration", Encoding.UTF8.GetBytes("Vibrate"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Debug.Log("Skickade Vibrate till Arduino");
        }
    }

    public void TurnOnLamp()
    {
        if (client != null && client.IsConnected)
        {
            client.Publish("Unity/Vibration", Encoding.UTF8.GetBytes("Lamp"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Debug.Log("Skickade Lamp till Arduino");
        }
    }

    public void TurnOffLamp()
    {
        if (client != null && client.IsConnected)
        {
            client.Publish("Unity/Vibration", Encoding.UTF8.GetBytes("LampOff"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Debug.Log("Skickade Lamp till Arduino");
        }
    }
}