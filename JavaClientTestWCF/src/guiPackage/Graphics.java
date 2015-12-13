package guiPackage;

import java.awt.BorderLayout;
import java.awt.EventQueue;
import java.awt.GridLayout;

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JPasswordField;
import javax.swing.JTextArea;
import java.awt.List;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.security.auth.login.LoginContext;
import javax.swing.JButton;
import java.awt.event.ActionListener;
import java.rmi.RemoteException;
import java.awt.event.ActionEvent;
import communicationPackage.*;

public class Graphics {

	private JFrame frame;
	private JTextField textField;
	private static String usernameStatic = "";

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					Graphics window = new Graphics();
					window.frame.setVisible(true);
					usernameStatic = JOptionPane.showInputDialog(new JFrame(),
	                        "What is your name?", null);
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
		
		List list = new List();
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
				IChatXServiceProxy proxy = new IChatXServiceProxy();
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
}
