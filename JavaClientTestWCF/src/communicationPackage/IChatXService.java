/**
 * IChatXService.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package communicationPackage;

public interface IChatXService extends java.rmi.Remote {
    public void joinRoom(java.lang.String username, java.lang.String roomName) throws java.rmi.RemoteException;
    public void leaveRoom(java.lang.String username, java.lang.String roomName) throws java.rmi.RemoteException;
    public void sendMessage(java.lang.String username, java.lang.String roomName, java.lang.String encryptedMessage) throws java.rmi.RemoteException;
    public java.lang.String[] getRooms() throws java.rmi.RemoteException;
    public java.lang.String[] whoIs(java.lang.String roomName) throws java.rmi.RemoteException;
    public java.lang.String login(java.lang.String username) throws java.rmi.RemoteException;
}
