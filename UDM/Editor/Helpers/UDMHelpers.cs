using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace UDM.Helpers {
	public class UDMHelpers {
		public static Texture2D LoadTextureWithGUID(string guid) =>
			AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(guid));
		
		// Convert an object to a byte array
		public static byte[] ObjectToByteArray(object obj)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}
// Convert a byte array to an Object
		public static T ByteArrayToObject<T>(byte[] arrBytes) where T : class
		{
			using (var memStream = new MemoryStream())
			{
				var binForm = new BinaryFormatter();
				memStream.Write(arrBytes, 0, arrBytes.Length);
				memStream.Seek(0, SeekOrigin.Begin);
				T obj = binForm.Deserialize(memStream) as T;
				return obj;
			}
		}
	}
}