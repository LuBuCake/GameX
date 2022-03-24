using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GameX.Base.Helpers
{
    public static class MarshalType<T>
    {
        #region Properties
        /// <summary>
        /// Gets if the type can be stored in a registers (for example ACX, ECX, ...).
        /// </summary>
        public static bool CanBeStoredInRegisters { get; private set; }
        /// <summary>
        /// State if the type is <see cref="IntPtr"/>.
        /// </summary>
        public static bool IsIntPtr { get; private set; }
        /// <summary>
        /// The real type.
        /// </summary>
        public static Type RealType { get; private set; }
        /// <summary>
        /// The size of the type.
        /// </summary>
        public static int Size { get; private set; }
        /// <summary>
        /// The size of the type as a <see cref="IntPtr"/>.
        /// </summary>
        public static IntPtr SizeAsPointer { get; private set; }
        /// <summary>
        /// The typecode of the type.
        /// </summary>
        public static TypeCode TypeCode { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes static information related to the specified type.
        /// </summary>
        static MarshalType()
        {
            // Gather information related to the provided type
            IsIntPtr = typeof(T) == typeof(IntPtr);
            RealType = typeof(T);
            Size = TypeCode == TypeCode.Boolean ? 1 : Marshal.SizeOf(RealType);
            SizeAsPointer = new IntPtr(Size);
            TypeCode = Type.GetTypeCode(RealType);
            // Check if the type can be stored in registers
            CanBeStoredInRegisters =
                IsIntPtr ||
#if x64
                TypeCode == TypeCode.Int64 ||
                TypeCode == TypeCode.UInt64 ||
#endif
                TypeCode == TypeCode.Boolean ||
                TypeCode == TypeCode.Byte ||
                TypeCode == TypeCode.Char ||
                TypeCode == TypeCode.Int16 ||
                TypeCode == TypeCode.Int32 ||
                TypeCode == TypeCode.Int64 ||
                TypeCode == TypeCode.SByte ||
                TypeCode == TypeCode.Single ||
                TypeCode == TypeCode.UInt16 ||
                TypeCode == TypeCode.UInt32;
        }
        #endregion

        #region Methods

        #region ObjectToByteArray
        /// <summary>
        /// Marshals a managed object to an array of bytes.
        /// </summary>
        /// <param name="obj">The object to marshal.</param>
        /// <returns>A array of bytes corresponding to the managed object.</returns>
        public static byte[] ObjectToByteArray(T obj)
        {
            // We'll tried to avoid marshalling as it really slows the process
            // First, check if the type can be converted without marhsalling
            switch (TypeCode)
            {
                case TypeCode.Object:
                    if (IsIntPtr)
                    {
                        switch (Size)
                        {
                            case 4:
                                return BitConverter.GetBytes(((IntPtr)(object)obj).ToInt32());
                            case 8:
                                return BitConverter.GetBytes(((IntPtr)(object)obj).ToInt64());
                        }
                    }
                    break;
                case TypeCode.Boolean:
                    return BitConverter.GetBytes((bool)(object)obj);
                case TypeCode.Char:
                    return Encoding.UTF8.GetBytes(new[] { (char)(object)obj });
                case TypeCode.Double:
                    return BitConverter.GetBytes((double)(object)obj);
                case TypeCode.Int16:
                    return BitConverter.GetBytes((short)(object)obj);
                case TypeCode.Int32:
                    return BitConverter.GetBytes((int)(object)obj);
                case TypeCode.Int64:
                    return BitConverter.GetBytes((long)(object)obj);
                case TypeCode.Single:
                    return BitConverter.GetBytes((float)(object)obj);
                case TypeCode.String:
                    throw new InvalidCastException("This method doesn't support string conversion.");
                case TypeCode.UInt16:
                    return BitConverter.GetBytes((ushort)(object)obj);
                case TypeCode.UInt32:
                    return BitConverter.GetBytes((uint)(object)obj);
                case TypeCode.UInt64:
                    return BitConverter.GetBytes((ulong)(object)obj);

            }

            return new byte[0];
        }
        #endregion

        #region ByteArrayToObject

        /// <summary>
        /// Marshals an array of byte to a managed object.
        /// </summary>
        /// <param name="byteArray">The array of bytes corresponding to a managed object.</param>
        /// <param name="index">[Optional] Where to start the conversion of bytes to the managed object.</param>
        /// <returns>A managed object.</returns>
        public static T ByteArrayToObject(byte[] byteArray, int index = 0)
        {
            // We'll tried to avoid marshalling as it really slows the process
            // First, check if the type can be converted without marshalling
            switch (TypeCode)
            {
                case TypeCode.Object:
                    if (IsIntPtr)
                    {
                        switch (byteArray.Length)
                        {
                            case 1:
                                return (T)(object)new IntPtr(BitConverter.ToInt32(new byte[] { byteArray[index], 0x0, 0x0, 0x0 }, index));
                            case 2:
                                return (T)(object)new IntPtr(BitConverter.ToInt32(new byte[] { byteArray[index], byteArray[index + 1], 0x0, 0x0 }, index));
                            case 4:
                                return (T)(object)new IntPtr(BitConverter.ToInt32(byteArray, index));
                            case 8:
                                return (T)(object)new IntPtr(BitConverter.ToInt64(byteArray, index));
                        }
                    }
                    break;
                case TypeCode.Boolean:
                    return (T)(object)BitConverter.ToBoolean(byteArray, index);
                case TypeCode.Byte:
                    return (T)(object)byteArray[index];
                case TypeCode.Char:
                    return (T)(object)Encoding.UTF8.GetChars(byteArray)[index];
                case TypeCode.Double:
                    return (T)(object)BitConverter.ToDouble(byteArray, index);
                case TypeCode.Int16:
                    return (T)(object)BitConverter.ToInt16(byteArray, index);
                case TypeCode.Int32:
                    return (T)(object)BitConverter.ToInt32(byteArray, index);
                case TypeCode.Int64:
                    return (T)(object)BitConverter.ToInt64(byteArray, index);
                case TypeCode.Single:
                    return (T)(object)BitConverter.ToSingle(byteArray, index);
                case TypeCode.String:
                    throw new InvalidCastException("This method doesn't support string conversion.");
                case TypeCode.UInt16:
                    return (T)(object)BitConverter.ToUInt16(byteArray, index);
                case TypeCode.UInt32:
                    return (T)(object)BitConverter.ToUInt32(byteArray, index);
                case TypeCode.UInt64:
                    return (T)(object)BitConverter.ToUInt64(byteArray, index);
            }

            return (T)(object)0;
        }

        #endregion

        #endregion
    }
}
