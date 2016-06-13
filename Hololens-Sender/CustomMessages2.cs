/*
 * CustomMessages.cs
 *
 * Allows for sending body data as custom messages to the Hololens
 * Requires a SharingStage GameObject
 */

using HoloToolkit.Sharing;
using HoloToolkit.Unity;
using System.Collections.Generic;
using UnityEngine;

public class CustomMessages2 : Singleton<CustomMessages2> {

    // Message enum containing information bytes to share
    // The first message type has to start with UserMessageIDStart
    // so as not to conflict with HoloToolkit internal messages
    public enum TestMessageID : byte {
        BodyData = MessageID.UserMessageIDStart,
        Max
    }

    public enum UserMessageChannels {
        Anchors = MessageChannel.UserMessageChannelStart,
    }

    // Cache the local user's ID to use when sending messages
    public long localUserID {
        get; set;
    }

    public delegate void MessageCallback(NetworkInMessage msg);
    private Dictionary<TestMessageID, MessageCallback> _MessageHandlers =
        new Dictionary<TestMessageID, MessageCallback>();
    public Dictionary<TestMessageID, MessageCallback> MessageHandlers {
        get {
            return _MessageHandlers;
        }
    }

    // Helper object that we use to route incoming message callbacks to the member
    // functions of this class
    NetworkConnectionAdapter connectionAdapter;

    // Cache the connection object for the sharing service
    NetworkConnection serverConnection;

    void Start() {
        InitializeMessageHandlers();
    }

    void InitializeMessageHandlers() {

        SharingStage sharingStage = SharingStage.Instance;
        
        if (sharingStage == null) {
            Debug.Log("Cannot Initialize CustomMessages. No SharingStage instance found.");
            return;
        }
        
        serverConnection = sharingStage.Manager.GetServerConnection();
        if (serverConnection == null) {
            Debug.Log("Cannot initialize CustomMessages. Cannot get a server connection.");
            return;
        }
        
        connectionAdapter = new NetworkConnectionAdapter();
        connectionAdapter.MessageReceivedCallback += OnMessageReceived;

        // Cache the local user ID
        this.localUserID = SharingStage.Instance.Manager.GetLocalUser().GetID();

        for (byte index = (byte)TestMessageID.BodyData; index < (byte)TestMessageID.Max; index++) {
            
            if (MessageHandlers.ContainsKey((TestMessageID)index) == false) {
                MessageHandlers.Add((TestMessageID)index, null);
            }

            serverConnection.AddListener(index, connectionAdapter);
        }
    }

    private NetworkOutMessage CreateMessage(byte MessageType) {
        NetworkOutMessage msg = serverConnection.CreateMessage(MessageType);
        msg.Write(MessageType);
        return msg;
    }

    // Sends body data in the form of the tracking ID and then each joint's
    // Vector3 coordinates
    public void SendBodyData(ulong trackingID, Vector3[] bodyData) {
        // If we are connected to a session, broadcast our info
        if (this.serverConnection != null && this.serverConnection.IsConnected()) {
            // Create an outgoing network message to contain all the info we want to send
            NetworkOutMessage msg = CreateMessage((byte)TestMessageID.BodyData);

            msg.Write((long)trackingID);

            foreach (Vector3 jointPos in bodyData) {
                AppendVector3(msg, jointPos);
            }

            // Send the message as a broadcast
            this.serverConnection.Broadcast(
                msg,
                MessagePriority.Immediate,
                MessageReliability.UnreliableSequenced,
                MessageChannel.Avatar);
        }
    }

    void OnDestroy() {

        if (this.serverConnection != null) {

            for (byte index = (byte)TestMessageID.BodyData; index < (byte)TestMessageID.Max; index++) {
                this.serverConnection.RemoveListener(index, this.connectionAdapter);
            }
            this.connectionAdapter.MessageReceivedCallback -= OnMessageReceived;
        }
    }

    void OnMessageReceived(NetworkConnection connection, NetworkInMessage msg) {

        byte messageType = msg.ReadByte();
        MessageCallback messageHandler = MessageHandlers[(TestMessageID)messageType];
        if (messageHandler != null) {
            messageHandler(msg);
        }
    }

    #region HelperFunctionsForWriting

    void AppendVector3(NetworkOutMessage msg, Vector3 vector) {
        msg.Write(vector.x);
        msg.Write(vector.y);
        msg.Write(vector.z);
    }

    #endregion HelperFunctionsForWriting

    #region HelperFunctionsForReading

    public Vector3 ReadVector3(NetworkInMessage msg) {
        return new Vector3(msg.ReadFloat(), msg.ReadFloat(), msg.ReadFloat());
    }

    #endregion HelperFunctionsForReading
}