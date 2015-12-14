package communicationPackage;

import java.io.OutputStream;
import java.net.ServerSocket;
import java.io.InputStream;

public class Socket {

	
	//not working, will prob work if paired with c# code. 
	//where is socket part of server application??..
	
	public Socket() {		
			
	}
	
	//be implemented in a standalone thread to constantly listen.
	public String socketServer()
	{
		try{
			@SuppressWarnings("resource")
			ServerSocket serverSocket = new ServerSocket(4343, 10);
	        java.net.Socket socket = serverSocket.accept();
	        InputStream is = socket.getInputStream();
	        OutputStream os = socket.getOutputStream();

	        // Receiving
	        byte[] lenBytes = new byte[4];
	        is.read(lenBytes, 0, 4);
	        int len = (((lenBytes[3] & 0xff) << 24) | ((lenBytes[2] & 0xff) << 16) |
	                  ((lenBytes[1] & 0xff) << 8) | (lenBytes[0] & 0xff));
	        byte[] receivedBytes = new byte[len];
	        is.read(receivedBytes, 0, len);
	        String received = new String(receivedBytes, 0, len);
	        return received;
		}
		catch(Exception e)
		{
			return "fail";
		}	
	}
}
