using UnityEngine;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Text;

public class ArduinoForceListener : MonoBehaviour
{
    public TheForce theForce; // Dra in ert TheForce-GameObject här

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
        // Kör på main thread
        if (lastMessage == "Force On")
        {
            theForce.HandleButtonPress();
            lastMessage = ""; // reset efter att vi kört
        }
    }

    void OnDestroy()
    {
        if (client != null && client.IsConnected)
            client.Disconnect();
    }
}