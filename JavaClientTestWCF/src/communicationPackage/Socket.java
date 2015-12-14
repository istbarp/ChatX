package communicationPackage;

import java.io.OutputStream;
import java.io.PrintWriter;
import java.util.ArrayList;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStream;
import java.io.InputStreamReader;

public class Socket implements Runnable{
	
	static ArrayList<String> incoming = new ArrayList<String>();
	static boolean threadClose = true;
	static String hostIP;
	static int portNumber;

	
	//not working, will prob work if paired with c# code. 
	//where is socket part of server application??..
	
	public Socket(String ip, int port) {		
		hostIP = ip;
		portNumber = port;
	}
	
	public static void CloseThread()
	{
		threadClose = false;
	}

	public void run() {
		
		try{
			final String host = "localhost";
			final int portNumber = 81;
			
			@SuppressWarnings("resource")
			java.net.Socket socket = new java.net.Socket(host, portNumber);	

			while (threadClose) {
				
				BufferedReader br = new BufferedReader(new InputStreamReader(socket.getInputStream()));
				PrintWriter out = new PrintWriter(socket.getOutputStream(), true);
				
				System.out.println("server says:" + br.readLine());

				BufferedReader userInputBR = new BufferedReader(new InputStreamReader(System.in));
				String userInput = userInputBR.readLine();

				out.println(userInput);
				
				incoming.add(br.readLine());

				if ("exit".equalsIgnoreCase(userInput)) {
					socket.close();
					break;
				}
			}
		}
		catch(Exception e)
		{
			//return "fail";
		}
		
	}
}
