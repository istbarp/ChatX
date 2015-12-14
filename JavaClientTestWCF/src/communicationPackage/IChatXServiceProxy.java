package communicationPackage;

public class IChatXServiceProxy implements communicationPackage.IChatXService {
  private String _endpoint = null;
  private communicationPackage.IChatXService iChatXService = null;
  
  public IChatXServiceProxy() {
    _initIChatXServiceProxy();
  }
  
  public IChatXServiceProxy(String endpoint) {
    _endpoint = endpoint;
    _initIChatXServiceProxy();
  }
  
  private void _initIChatXServiceProxy() {
    try {
      iChatXService = (new communicationPackage.ChatXServiceLocator()).getBasicHttpBinding_IChatXService();
      if (iChatXService != null) {
        if (_endpoint != null)
          ((javax.xml.rpc.Stub)iChatXService)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
        else
          _endpoint = (String)((javax.xml.rpc.Stub)iChatXService)._getProperty("javax.xml.rpc.service.endpoint.address");
      }
      
    }
    catch (javax.xml.rpc.ServiceException serviceException) {}
  }
  
  public String getEndpoint() {
    return _endpoint;
  }
  
  public void setEndpoint(String endpoint) {
    _endpoint = endpoint;
    if (iChatXService != null)
      ((javax.xml.rpc.Stub)iChatXService)._setProperty("javax.xml.rpc.service.endpoint.address", _endpoint);
    
  }
  
  public communicationPackage.IChatXService getIChatXService() {
    if (iChatXService == null)
      _initIChatXServiceProxy();
    return iChatXService;
  }
  
  public void joinRoom(java.lang.String username, java.lang.String roomName) throws java.rmi.RemoteException{
    if (iChatXService == null)
      _initIChatXServiceProxy();
    iChatXService.joinRoom(username, roomName);
  }
  
  public void leaveRoom(java.lang.String username, java.lang.String roomName) throws java.rmi.RemoteException{
    if (iChatXService == null)
      _initIChatXServiceProxy();
    iChatXService.leaveRoom(username, roomName);
  }
  
  public void sendMessage(java.lang.String username, java.lang.String roomName, java.lang.String encryptedMessage) throws java.rmi.RemoteException{
    if (iChatXService == null)
      _initIChatXServiceProxy();
    iChatXService.sendMessage(username, roomName, encryptedMessage);
  }
  
  public java.lang.String[] getRooms() throws java.rmi.RemoteException{
    if (iChatXService == null)
      _initIChatXServiceProxy();
    return iChatXService.getRooms();
  }
  
  public java.lang.String[] whoIs(java.lang.String roomName) throws java.rmi.RemoteException{
    if (iChatXService == null)
      _initIChatXServiceProxy();
    return iChatXService.whoIs(roomName);
  }
  
  public java.lang.String login(java.lang.String username) throws java.rmi.RemoteException{
    if (iChatXService == null)
      _initIChatXServiceProxy();
    return iChatXService.login(username);
  }
  
  
}