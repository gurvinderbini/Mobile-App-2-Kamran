using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App14
{
    class socketConnection
    {
        /*
        public static Socket socket { get; set; }
        public socketConnection() { }

        public void socketConnection()
        {
            // connect to a Socket.IO server
            socket = IO.Socket("https://logos13.cloudschool.management/itcrm/webservices");
            socket.Connect();

            // disconnect from the server
            socket.Close();

            // whenever the server emits "login", print the login message
            socket.On("login", data =>
            {
                connected = true;

                // get the json data from the server message
                var jobject = data as JToken;

                // get the number of users
                var numUsers = jobject.Value<int>("numUsers");

                // display the welcome message...
            });

            // whenever the server emits "new message", update the chat body
            socket.On("new message", data =>
            {
                // get the json data from the server message
                var jobject = data as JToken;

                // get the message data values
                var username = jobject.Value<string>("username");
                var message = jobject.Value<string>("message");

                // display message...
            });
        }*/
        
    }

}
