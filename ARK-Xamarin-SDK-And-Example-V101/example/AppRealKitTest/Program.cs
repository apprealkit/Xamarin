using System;
using System.Text;
using AppRealKit.Realtime;
using AppRealKit.Realtime.State;
using AppRealKit.Realtime.Data;
using AppRealKit.Realtime.Utils.SimpleJSON;
using System.Collections.Generic;

namespace AppRealKitTest
{
	class MainClass
	{
		private const bool debugFlg = false;
		private const string sAppID = "080693ab-7123-4aee-b560-9f6a8a332a43";
		private const string sAppsecret = "VORAI5D6yz";
		private const string sHost = "192.168.10.102";
		private const int iPort = 7777;
		private const string loginID = "1234";
		private const string memberID = "5678";
		private static string roomID = "room123456789";

		//Opened callback
		private static void openedCallback(object sender, ClientEventArgs e)
		{
			Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));

			//Call all methods
			if (Balancer.Instance.isConnected ()) {
				Console.WriteLine ("authenticate...");
				authenticate ();
				Console.WriteLine ("getListRoom...");
				getListRoom ();
				Console.WriteLine ("getUser...");
				getUser ();
				Console.WriteLine ("getListUser...");
				getListUser ();
				Console.WriteLine ("updatePoint...");
				updatePoint ();
				Console.WriteLine ("getLeaderBoard...");
				getLeaderBoard ();
				Console.WriteLine ("notifyListUser...");
				notifyListUser ();
			}
		}

		//Closed callback
		private static void closedCallback(object sender, ClientEventArgs e)
		{
			Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
		}

		//Sent callback
		private static void sentCallback(object sender, ClientEventArgs e)
		{
			Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
		}

		//Received callback
		private static void receivedCallback(object sender, ClientEventArgs e)
		{
			Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
		}

		public static void Main (string[] args)
		{
			//Set all callback methods
			Balancer.Instance.onKitOpen (openedCallback);
			Balancer.Instance.onKitClose (closedCallback);
			Balancer.Instance.onKitSent (sentCallback);
			Balancer.Instance.onKitMessage (receivedCallback);

			//Set debug
			Balancer.Instance.setVerbose (debugFlg);

			//Set APPID and APP Secret
			Balancer.Instance.setProductID (sAppID, sAppsecret);

			//Connection to serverkits server
			Balancer.Instance.connect (sHost, iPort);

			//Wait any pressed key
			Console.ReadLine ();
			Console.WriteLine ("Finish the test case");
		}

		//Authenticate method
		public static void authenticate()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.AUTHENTICATE_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = loginID;

			//Execute this method
			Balancer.Instance.authenticate (loginID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
				Console.WriteLine ("createRoom...");
				createRoom ();
			});
		}

		//Create room method
		public static void createRoom()
		{
			//Create parameters
			Dictionary<string, string> roomProperties = new Dictionary<string, string>();
			roomProperties ["name"] = "ROOM_NAME";
			roomProperties ["desc"] = "SERVER_KITS_ROOM_TEST_DEMO";

			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.CREATE_ROOM_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = loginID;
			jsonObject[Param.REQUEST_DATA_EXTRAS_KEY]["name"] = roomProperties ["name"];
			jsonObject[Param.REQUEST_DATA_EXTRAS_KEY]["desc"] = roomProperties ["desc"];
			int iCapacity = 0;

			//Execute this method
			Balancer.Instance.createRoom (loginID, roomProperties, iCapacity, (object sender, ClientEventArgs e) => {
				string sMessage = Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength);
				if(e.iResponeCode == 0) {
					//Parse JSON string to JSON object
					JSONNode jsonResponse = JSONNode.Parse(sMessage);

					//Set room ID
					roomID = jsonResponse["obj"]["id"];
				}
				Console.WriteLine (sMessage);

				Console.WriteLine ("joinRoom...");
				joinRoom ();

				Console.WriteLine ("leaveRoom...");
				leaveRoom ();

				Console.WriteLine ("getRoom...");
				getRoom ();

				Console.WriteLine ("notifyInRoom...");
				notifyInRoom ();
			});
		}

		//Join room method
		public static void joinRoom()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.JOIN_ROOM_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = memberID;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = roomID;

			//Execute this method
			Balancer.Instance.joinRoom (memberID, roomID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Leave room method
		public static void leaveRoom()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.LEAVE_ROOM_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = memberID;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = roomID;

			//Execute this method
			Balancer.Instance.leaveRoom (memberID, roomID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Get room detail
		public static void getRoom()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.GET_ROOM_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = roomID;

			//Execute this method
			Balancer.Instance.getRoom (roomID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Get list room
		public static void getListRoom()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.GET_LIST_ROOM_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = loginID;

			//Execute this method
			Balancer.Instance.getListRoom (loginID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Get user detail
		public static void getUser()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.GET_USER_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = loginID;

			//Execute this method
			Balancer.Instance.getUser (loginID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Update point of user
		public static void updatePoint()
		{
			//Create parameters
			Dictionary<string, int> arrpoint = new Dictionary<string, int>();
			arrpoint[Param.REQUEST_DATA_STAMINA_KEY] = 100;
			arrpoint[Param.REQUEST_DATA_EXP_KEY] = 1;

			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.UPDATE_POINT_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY] = loginID;
			foreach (KeyValuePair<string, int> itDetail in arrpoint)
			{
				jsonObject[Param.REQUEST_DATA_KEY][itDetail.Key].AsInt = itDetail.Value;
			}

			//Execute this method
			Balancer.Instance.updatePoint (loginID, arrpoint, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Gte list user
		public static void getListUser()
		{
			//Create list user
			string[] arrUserID = new string[2];
			arrUserID[0] = loginID;
			arrUserID[1] = memberID;

			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.GET_LIST_USER_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY][0] = loginID;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY][1] = memberID;

			//Execute this method
			Balancer.Instance.getListUser (arrUserID, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Get leader board
		public static void getLeaderBoard()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.GET_LEADER_BOARD_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_ORDER_BY_KEY] = Param.REQUEST_DATA_EXP_KEY;

			//Execute this method
			Balancer.Instance.getLeaderBoard (Param.REQUEST_DATA_EXP_KEY, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Notify user in room
		public static void notifyInRoom()
		{
			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.NOTIFY_IN_ROOM_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_ROOM_ID_KEY] = roomID;
			jsonObject[Param.RESPONSE_CODE_KEY].AsInt = 999;
			jsonObject[Param.RESPONSE_MESSAGE_KEY] = "NOTIFICATION TEXT DEMO";

			//Execute this method
			Balancer.Instance.notifyInRoom (roomID, 9999, "NOTIFICATION TEXT DEMO", false, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}

		//Notify for user list
		public static void notifyListUser()
		{
			//Create list user
			string[] arrUserID = new string[2];
			arrUserID[0] = loginID;
			arrUserID[1] = memberID;

			//Create message to send into server
			JSONClass jsonObject = new JSONClass();
			jsonObject[Param.REQUEST_CALLBACK_METHOD_KEY] = Method.NOTIFY_USER_LIST_CALLBACK_NAME;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY][0] = loginID;
			jsonObject[Param.REQUEST_DATA_USER_ID_KEY][1] = memberID;
			jsonObject[Param.RESPONSE_CODE_KEY].AsInt = 999;
			jsonObject[Param.RESPONSE_MESSAGE_KEY] = "NOTIFICATION TEXT DEMO";

			//Execute this method
			Balancer.Instance.notifyListUser (arrUserID, 9999, "NOTIFICATION TEXT DEMO", false, (object sender, ClientEventArgs e) => {
				Console.WriteLine (Encoding.UTF8.GetString (e.arrBytes, 0, e.iLength));
			});
		}
	}
}
