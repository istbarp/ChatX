package guiPackage;

import java.awt.Component;
import java.awt.EventQueue;
import javax.swing.JFrame;
import javax.swing.JOptionPane;
import java.awt.List;
import javax.swing.JTextField;
import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.rmi.RemoteException;
import java.awt.event.ActionEvent;
import communicationPackage.*;

public class Graphics {

	private JFrame frame;
	private JTextField textField;
	static String usernameStatic = "";
	static String room = "";
	static boolean roomChosed = false;
	static IChatXServiceProxy proxy = new IChatXServiceProxy();
	public static List list;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {			
					
					Graphics window = new Graphics();
					
					usernameStatic = JOptionPane.showInputDialog(new JFrame(),
	                        "What is your name?", null);
					while(usernameStatic == "")
					{
						
					}
					
					String[] rooms = proxy.getRooms();

					room = (String) JOptionPane.showInputDialog(new JFrame(),
				            "What room you want to join?",
				            "Rooms", JOptionPane.QUESTION_MESSAGE,
				            null, rooms,"A");
					
					while(room == "")
					{
						
					}
					
					list.add("Login Successful");
					
					String socketInfo = proxy.login(usernameStatic);
					String[] socketInfoArray = socketInfo.split(":");
					Socket runningSocket = new Socket(socketInfoArray[0], Integer.parseInt(socketInfoArray[1]));
					Thread t = new Thread(runningSocket);
					t.start();
					
					window.frame.setVisible(true);
					
					
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the application.
	 */
	public Graphics() {
		initialize();
	}

	/**
	 * Initialize the contents of the frame.
	 */
	private void initialize() {
		frame = new JFrame();
		frame.setBounds(100, 100, 328, 445);
		frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		frame.getContentPane().setLayout(null);
		
		list = new List();
		list.setBounds(10, 10, 293, 344);
		frame.getContentPane().add(list);
		
		textField = new JTextField();
		textField.setBounds(10, 361, 176, 20);
		frame.getContentPane().add(textField);
		textField.setColumns(10);
		
		JButton btnNewButton = new JButton("Send message");
		btnNewButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent arg0) {				
				list.add(usernameStatic + ": " + textField.getText());				
				try {
					proxy.sendMessage(usernameStatic, "test", textField.getText());
				} catch (RemoteException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		});
		btnNewButton.setBounds(196, 360, 107, 23);
		frame.getContentPane().add(btnNewButton);
	}
	
	public static void addToListGUI(String message)
	{
		list.add(message);
	}
}
