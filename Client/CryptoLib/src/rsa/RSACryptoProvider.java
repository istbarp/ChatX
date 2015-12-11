package rsa;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.math.BigInteger;
import java.security.InvalidKeyException;
import java.security.Key;
import java.security.KeyFactory;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.NoSuchProviderException;
import java.security.SecureRandom;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.RSAPrivateKeySpec;
import java.security.spec.RSAPublicKeySpec;

import javax.crypto.Cipher;
import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;

public class RSACryptoProvider
{
	public Key PublicKey;
	private Key PrivateKey;
	
	private final String ALGORITHM = "RSA";
	private final String CIPHERALGORITHM = "RSA";
	private final String PATH = "C:/keys";
	
	public RSACryptoProvider()
	{
		
	}
	
	public void GenerateKeyPair() throws 
	    NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, 
	    BadPaddingException, InvalidKeySpecException, FileNotFoundException, IOException, NoSuchProviderException 
	{
		KeyPairGenerator generator;
		generator = KeyPairGenerator.getInstance(ALGORITHM);
		SecureRandom random = new SecureRandom();
		generator.initialize(1024, random);
		KeyPair pair = generator.generateKeyPair();
		PublicKey = pair.getPublic();
		PrivateKey = pair.getPrivate();
		System.out.println(PrivateKey.toString());
		SaveKeys();
	}
	
	public byte[] Encrypt(String text) throws 
	    NoSuchAlgorithmException, 
	    NoSuchPaddingException, 
	    InvalidKeyException, 
	    IllegalBlockSizeException, 
	    BadPaddingException, NoSuchProviderException 
	{
		Cipher cipher = Cipher.getInstance(ALGORITHM);
		cipher.init(Cipher.ENCRYPT_MODE, PublicKey);
		return cipher.doFinal(text.getBytes());
	}
	
	public String Decrypt(byte[] text)throws 
	    NoSuchAlgorithmException, 
	    NoSuchPaddingException, 
	    InvalidKeyException, 
	    IllegalBlockSizeException, 
	    BadPaddingException, NoSuchProviderException 
	{
		Cipher ciph = Cipher.getInstance(ALGORITHM);
		ciph.init(Cipher.DECRYPT_MODE, PrivateKey);
		return new String(ciph.doFinal(text));
	}
	
	private void SaveKeys() throws NoSuchAlgorithmException, InvalidKeySpecException, FileNotFoundException, IOException, NoSuchProviderException
	{
		/*KeyFactory fact = KeyFactory.getInstance(ALGORITHM, "BC");
		RSAPublicKeySpec pub = fact.getKeySpec(PublicKey, RSAPublicKeySpec.class);
		RSAPublicKeySpec pri = fact.getKeySpec(PrivateKey, RSAPublicKeySpec.class);
		RSAEncryptionDescription rsaObj = new RSAEncryptionDescription();  
		rsaObj.saveKeys(PUBLIC_KEY_FILE, rsaPubKeySpec.getModulus(), rsaPubKeySpec.getPublicExponent());  
		rsaObj.saveKeys(PRIVATE_KEY_FILE, rsaPrivKeySpec.getModulus(), rsaPrivKeySpec.getPrivateExponent());*/
		
		
		KeyFactory fact = KeyFactory.getInstance(ALGORITHM);
		RSAPublicKeySpec pub = fact.getKeySpec(PublicKey, RSAPublicKeySpec.class);
		RSAPrivateKeySpec pri = fact.getKeySpec(PrivateKey, RSAPrivateKeySpec.class);
		ObjectOutputStream oout = new ObjectOutputStream(new BufferedOutputStream(new FileOutputStream(PATH)));
		oout.writeObject(pub.getModulus());
		oout.writeObject(pub.getPublicExponent());
		oout.writeObject(pri.getModulus());
		oout.writeObject(pri.getPrivateExponent());
		oout.close();
	}
	
	private void LoadKeys() throws NoSuchAlgorithmException, InvalidKeySpecException, FileNotFoundException, IOException, ClassNotFoundException, NoSuchProviderException
	{
		KeyFactory fact = KeyFactory.getInstance(ALGORITHM);
		ObjectInputStream oin = new ObjectInputStream(new BufferedInputStream(new FileInputStream(PATH)));
		RSAPublicKeySpec pub = new RSAPublicKeySpec((BigInteger)oin.readObject(), (BigInteger)oin.readObject());
		RSAPrivateKeySpec pri = new RSAPrivateKeySpec((BigInteger)oin.readObject(), (BigInteger)oin.readObject());
		PublicKey = fact.generatePublic(pub);
		PrivateKey = fact.generatePrivate(pri);
		oin.close();
	}
}
