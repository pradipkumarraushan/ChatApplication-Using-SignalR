using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace groupChat.Hubs
{

    public class ChatMessage
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string GroupName { get; set; }

        public string FriendUniqueId { get; set; }
    }
    [HubName("letsChatHub")]
    public class LetsChatHub : Hub
    {


        //chat.server.sendGroupMessage({ Name: "ABC", Message: "Hello World", GroupName: "chatGroup1" });
        public void sendGroupMessage([Bind(Include = "Name,Message,GroupName")] ChatMessage message)
        {

            Clients.Group(message.GroupName, Context.ConnectionId).addNewGroupMessageToPage(message.Name, message.Message);

        }


        //chat.server.sendPrivateMessage({ Name: "ABC", Message: "Hello World", FriendUniqueId: "FriendUniqueId" });
        public void sendPrivateMessage(ChatMessage message)
        {
            //Context.ConnectionId returning because i dont want to display send message in right side . (Filter of message )
            Clients.Client(message.FriendUniqueId).addNewPrivateMessageToPage(message.Name, message.Message, Context.ConnectionId);
        }


        //server
        public void joinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
        //Delete from group
        public void deleteUserFromGroup(string groupName)
        {
            Groups.Remove(Context.ConnectionId, groupName);
        }


        public void sendChatMessage(string who, string name, string message)
        {
            //who - > is who sent the msg (connection id)
            Clients.AllExcept(who).addChatMessage(name, message);

            name = Context.User.Identity.Name;

            //foreach (var connectionId in _connections.GetConnections(who))
            //{
            //    Clients.Client(connectionId).addChatMessage(name, message);
            //}
        }



    //    private readonly static ConnectionMapping<string> _connections =
    //       new ConnectionMapping<string>();
    //    public override Task OnConnected()
    //    {
    //        string name = Context.User.Identity.Name;

    //        _connections.Add(name, Context.ConnectionId);

    //        return base.OnConnected();
    //    }

    //    public override Task OnDisconnected(bool stopCalled)
    //    {
    //        string name = Context.User.Identity.Name;

    //        _connections.Remove(name, Context.ConnectionId);

    //        return base.OnDisconnected(stopCalled);
    //    }

    //    public override Task OnReconnected()
    //    {
    //        string name = Context.User.Identity.Name;

    //        if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
    //        {
    //            _connections.Add(name, Context.ConnectionId);
    //        }

    //        return base.OnReconnected();
    //    }

    //}

    //public class ConnectionMapping<T>
    //{
    //    private readonly Dictionary<T, HashSet<string>> _connections =
    //        new Dictionary<T, HashSet<string>>();

    //    public int Count
    //    {
    //        get
    //        {
    //            return _connections.Count;
    //        }
    //    }

    //    public void Add(T key, string connectionId)
    //    {
    //        lock (_connections)
    //        {
    //            HashSet<string> connections;
    //            if (!_connections.TryGetValue(key, out connections))
    //            {
    //                connections = new HashSet<string>();
    //                _connections.Add(key, connections);
    //            }

    //            lock (connections)
    //            {
    //                connections.Add(connectionId);
    //            }
    //        }
    //    }

    //    public IEnumerable<string> GetConnections(T key)
    //    {
    //        HashSet<string> connections;
    //        if (_connections.TryGetValue(key, out connections))
    //        {
    //            return connections;
    //        }

    //        return Enumerable.Empty<string>();
    //    }

    //    public void Remove(T key, string connectionId)
    //    {
    //        lock (_connections)
    //        {
    //            HashSet<string> connections;
    //            if (!_connections.TryGetValue(key, out connections))
    //            {
    //                return;
    //            }

    //            lock (connections)
    //            {
    //                connections.Remove(connectionId);

    //                if (connections.Count == 0)
    //                {
    //                    _connections.Remove(key);
    //                }
    //            }
    //        }
    //    }
    }
    [HubName("chatConversationHub")]
        public class ChatConversationHub : Hub
        {
            public static void BroadcastData(string groupName)
            {
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatConversationHub>();
                context.Clients.Group(groupName).refreshChatConversationData();
            }
        }

}
/*
 SignalR has two methods:


Clients.All.receiveMessage(msgFrom, msg);
    and:
Clients.Group(GroupName,Exceptional).receiveMessage(msgFrom, msg);
(In both methods, receiveMessage(msgFrom, msg) is a client side method to be called from SignalR Hub server.)

Both methods are useful in group message broadcast. But there is an important difference between these two methods.

The method...

Clients.All.receiveMessage(msgFrom, msg);
...broadcasts the message to all connected users of hub. To send message to limited users, exclude those users, is the option given.


string[] Exceptional = new string[1];//the array of connection ids, to whom message was not to be delivered
Exceptional[0] = connection_id;
Clients.AllExcept(Exceptional).receiveMessage(msgFrom, msg);
To add number of connection ids to an array is a hectic and time consuming work. Hence it is better to use Group messaging in a signalR.


Clients.Group(GroupName,Exceptional).receiveMessage(msgFrom, msg);
It is the right method to broadcast messages to users connected to group. Here also, the option to add, exceptional is available to exclude some users from receiving messages.

Use...

Groups.Add(connection_id, GroupName);
...to add connection id of incoming user to group.

For private chat, rather than excluding all other users than receiver one, simply send the message only to the particular user.


Clients.Client(touserid).receiveMessage(msgFrom, msg);
Here, touserid is the connection id of user who is going to receive the message.
     



Property	Description

All	        Calls a method on all connected clients
Caller	    Calls a method on the client that invoked the hub method
Others	    Calls a method on all connected clients except the client that invoked the method
Hub.Clients also contains the following methods:

Method	     Description

AllExcept	Calls a method on all connected clients except for the specified connections
Client	    Calls a method on a specific connected client
Clients   	Calls a method on specific connected clients
Group	    Calls a method on all connections in the specified group
GroupExcept	Calls a method on all connections in the specified group, except the specified connections
Groups	    Calls a method on multiple groups of connections
OthersInGroup	Calls a method on a group of connections, excluding the client that invoked the hub method
User	   Calls a method on all connections associated with a specific user
Users	   Calls a method on all connections associated with the specified users
     
     */
