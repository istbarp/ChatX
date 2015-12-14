/**
 * ChatXServiceLocator.java
 *
 * This file was auto-generated from WSDL
 * by the Apache Axis 1.4 Apr 22, 2006 (06:55:48 PDT) WSDL2Java emitter.
 */

package communicationPackage;

public class ChatXServiceLocator extends org.apache.axis.client.Service implements communicationPackage.ChatXService {

    public ChatXServiceLocator() {
    }


    public ChatXServiceLocator(org.apache.axis.EngineConfiguration config) {
        super(config);
    }

    public ChatXServiceLocator(java.lang.String wsdlLoc, javax.xml.namespace.QName sName) throws javax.xml.rpc.ServiceException {
        super(wsdlLoc, sName);
    }

    // Use to get a proxy class for BasicHttpBinding_IChatXService
    private java.lang.String BasicHttpBinding_IChatXService_address = "http://10.28.51.86:8984/ChatXService/";

    public java.lang.String getBasicHttpBinding_IChatXServiceAddress() {
        return BasicHttpBinding_IChatXService_address;
    }

    // The WSDD service name defaults to the port name.
    private java.lang.String BasicHttpBinding_IChatXServiceWSDDServiceName = "BasicHttpBinding_IChatXService";

    public java.lang.String getBasicHttpBinding_IChatXServiceWSDDServiceName() {
        return BasicHttpBinding_IChatXServiceWSDDServiceName;
    }

    public void setBasicHttpBinding_IChatXServiceWSDDServiceName(java.lang.String name) {
        BasicHttpBinding_IChatXServiceWSDDServiceName = name;
    }

    public communicationPackage.IChatXService getBasicHttpBinding_IChatXService() throws javax.xml.rpc.ServiceException {
       java.net.URL endpoint;
        try {
            endpoint = new java.net.URL(BasicHttpBinding_IChatXService_address);
        }
        catch (java.net.MalformedURLException e) {
            throw new javax.xml.rpc.ServiceException(e);
        }
        return getBasicHttpBinding_IChatXService(endpoint);
    }

    public communicationPackage.IChatXService getBasicHttpBinding_IChatXService(java.net.URL portAddress) throws javax.xml.rpc.ServiceException {
        try {
            communicationPackage.BasicHttpBinding_IChatXServiceStub _stub = new communicationPackage.BasicHttpBinding_IChatXServiceStub(portAddress, this);
            _stub.setPortName(getBasicHttpBinding_IChatXServiceWSDDServiceName());
            return _stub;
        }
        catch (org.apache.axis.AxisFault e) {
            return null;
        }
    }

    public void setBasicHttpBinding_IChatXServiceEndpointAddress(java.lang.String address) {
        BasicHttpBinding_IChatXService_address = address;
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        try {
            if (communicationPackage.IChatXService.class.isAssignableFrom(serviceEndpointInterface)) {
                communicationPackage.BasicHttpBinding_IChatXServiceStub _stub = new communicationPackage.BasicHttpBinding_IChatXServiceStub(new java.net.URL(BasicHttpBinding_IChatXService_address), this);
                _stub.setPortName(getBasicHttpBinding_IChatXServiceWSDDServiceName());
                return _stub;
            }
        }
        catch (java.lang.Throwable t) {
            throw new javax.xml.rpc.ServiceException(t);
        }
        throw new javax.xml.rpc.ServiceException("There is no stub implementation for the interface:  " + (serviceEndpointInterface == null ? "null" : serviceEndpointInterface.getName()));
    }

    /**
     * For the given interface, get the stub implementation.
     * If this service has no port for the given interface,
     * then ServiceException is thrown.
     */
    public java.rmi.Remote getPort(javax.xml.namespace.QName portName, Class serviceEndpointInterface) throws javax.xml.rpc.ServiceException {
        if (portName == null) {
            return getPort(serviceEndpointInterface);
        }
        java.lang.String inputPortName = portName.getLocalPart();
        if ("BasicHttpBinding_IChatXService".equals(inputPortName)) {
            return getBasicHttpBinding_IChatXService();
        }
        else  {
            java.rmi.Remote _stub = getPort(serviceEndpointInterface);
            ((org.apache.axis.client.Stub) _stub).setPortName(portName);
            return _stub;
        }
    }

    public javax.xml.namespace.QName getServiceName() {
        return new javax.xml.namespace.QName("http://tempuri.org/", "ChatXService");
    }

    private java.util.HashSet ports = null;

    public java.util.Iterator getPorts() {
        if (ports == null) {
            ports = new java.util.HashSet();
            ports.add(new javax.xml.namespace.QName("http://tempuri.org/", "BasicHttpBinding_IChatXService"));
        }
        return ports.iterator();
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(java.lang.String portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        
if ("BasicHttpBinding_IChatXService".equals(portName)) {
            setBasicHttpBinding_IChatXServiceEndpointAddress(address);
        }
        else 
{ // Unknown Port Name
            throw new javax.xml.rpc.ServiceException(" Cannot set Endpoint Address for Unknown Port" + portName);
        }
    }

    /**
    * Set the endpoint address for the specified port name.
    */
    public void setEndpointAddress(javax.xml.namespace.QName portName, java.lang.String address) throws javax.xml.rpc.ServiceException {
        setEndpointAddress(portName.getLocalPart(), address);
    }

}
