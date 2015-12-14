package controller;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.math.BigInteger;
import java.security.InvalidKeyException;
import java.security.Key;
import java.security.KeyFactory;
import java.security.KeyPair;
import java.security.KeyPairGenerator;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;
import java.security.interfaces.RSAPrivateCrtKey;
import java.security.spec.InvalidKeySpecException;
import java.security.spec.RSAPrivateKeySpec;
import java.security.spec.RSAPublicKeySpec;
import java.util.Base64;

import javax.crypto.Cipher;
import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;

public class RSACryptoProvider
{
	public Key PublicKey;
	private Key PrivateKey;
	
	private final String ALGORITHM = "RSA";
	private final String CIPHERALGORITHM = "RSA/ECB/PKCS1Padding";
	private final String PATH = "C:/keys.bin";
	
	public RSACryptoProvider() throws NoSuchAlgorithmException, InvalidKeySpecException, IOException
	{
		if (!(new File(PATH).exists()))
		{
			GenerateKeyPair();
			SaveKeys();
		}
		else
		{
			LoadKeys();
		}
	}
	
	private void GenerateKeyPair() throws NoSuchAlgorithmException
	{
		KeyPairGenerator generator;
		generator = KeyPairGenerator.getInstance(ALGORITHM);
		SecureRandom random = new SecureRandom();
		generator.initialize(1024, random);
		KeyPair pair = generator.generateKeyPair();
		PublicKey = pair.getPublic();
		PrivateKey = pair.getPrivate();
	}
	
	public byte[] Encrypt(String text) throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException
	{
		Cipher cipher = Cipher.getInstance(CIPHERALGORITHM);
		cipher.init(Cipher.ENCRYPT_MODE, PublicKey);
		return cipher.doFinal(text.getBytes());
	}
	
	public String Decrypt(byte[] text)throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException, IllegalBlockSizeException, BadPaddingException 
	{
		Cipher ciph = Cipher.getInstance(CIPHERALGORITHM);
		ciph.init(Cipher.DECRYPT_MODE, PrivateKey);
		return new String(ciph.doFinal(text));
	}
	
	private void SaveKeys() throws IOException, NoSuchAlgorithmException, InvalidKeySpecException
	{
		BufferedWriter output = new BufferedWriter(new FileWriter(PATH));

		RSAPrivateCrtKey keys = (RSAPrivateCrtKey)PrivateKey;
		
		output.append(new String(Base64.getEncoder().encodeToString(keys.getModulus().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getPublicExponent().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getPrivateExponent().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getPrimeP().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getPrimeQ().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getPrimeExponentP().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getPrimeExponentQ().toByteArray())));
		output.append("\n");
		output.append(new String(Base64.getEncoder().encodeToString(keys.getCrtCoefficient().toByteArray())));
		output.close();
	}
	
	private void LoadKeys() throws IOException, NoSuchAlgorithmException, InvalidKeySpecException
	{
		BufferedReader input = new BufferedReader(new FileReader(PATH));
		BigInteger modulus = new BigInteger(Base64.getDecoder().decode(input.readLine()));
		
		KeyFactory fact = KeyFactory.getInstance(ALGORITHM);
		RSAPublicKeySpec pub = new RSAPublicKeySpec(modulus, new BigInteger(Base64.getDecoder().decode(input.readLine())));
		RSAPrivateKeySpec pri = new RSAPrivateKeySpec(modulus, new BigInteger(Base64.getDecoder().decode(input.readLine())));

		PublicKey = fact.generatePublic(pub);
		PrivateKey = fact.generatePrivate(pri);
		
		input.close();
	}
	
	private void SaveKeystoXML() throws IOException 
	{
		BufferedWriter output = new BufferedWriter(new FileWriter(PATH));

	    RSAPrivateCrtKey privKey = (RSAPrivateCrtKey)PrivateKey;

	    BigInteger n = privKey.getModulus();
	    BigInteger e = privKey.getPublicExponent();
	    BigInteger d = privKey.getPrivateExponent();
	    BigInteger p = privKey.getPrimeP();
	    BigInteger q = privKey.getPrimeQ();
	    BigInteger dp = privKey.getPrimeExponentP();
	    BigInteger dq = privKey.getPrimeExponentQ();
	    BigInteger inverseQ = privKey.getCrtCoefficient(); 

	    StringBuilder builder = new StringBuilder();
	    builder.append("<RSAKeyValue>\n");
	    write(builder, "Modulus", n);
	    write(builder, "Exponent", e);
	    write(builder, "P", p);
	    write(builder, "Q", q);
	    write(builder, "DP", dp);
	    write(builder, "DQ", dq);
	    write(builder, "InverseQ", inverseQ);
	    write(builder, "D", d);
	    builder.append("</RSAKeyValue>");
	    
	    output.append(builder.toString());
	    output.flush();
	    output.close();
	    System.out.println(builder.toString());
	}
	
	private void write(StringBuilder builder, String tag, BigInteger bigInt)
	{
	    builder.append("\t<");
	    builder.append(tag);
	    builder.append(">");
	    builder.append(encode(bigInt));
	    builder.append("</");
	    builder.append(tag);
	    builder.append(">\n");
	}

	private String encode(BigInteger bigInt)
	{
	    return new String(Base64.getEncoder().encodeToString(bigInt.toByteArray()));
	}
}
