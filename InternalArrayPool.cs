using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace ArknightsResources.Operators.Resources
{
    internal static class InternalArrayPools
    {
        internal static readonly ArrayPool<byte> ByteArrayPool = ArrayPool<byte>.Create(1024 * 1024, 3);
        internal static readonly ArrayPool<byte[]> ByteArrayArrayPool = ArrayPool<byte[]>.Create(12, 3);
        internal static readonly ArrayPool<int> Int32ArrayPool = ArrayPool<int>.Create(64, 3);
    }
}
