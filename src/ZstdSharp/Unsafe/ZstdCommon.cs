using static ZstdSharp.UnsafeHelper;

namespace ZstdSharp.Unsafe
{
    public static unsafe partial class Methods
    {
        /*-****************************************
         *  Version
         ******************************************/
        public static uint ZSTD_versionNumber()
        {
            return 1 * 100 * 100 + 5 * 100 + 4;
        }

        /*! ZSTD_versionString() :
         *  Return runtime library version, like "1.4.5". Requires v1.3.0+. */
        public static string ZSTD_versionString()
        {
            return "1.5.4";
        }

        /*! ZSTD_isError() :
         *  tells if a return value is an error code
         *  symbol is required for external callers */
        public static bool ZSTD_isError(nuint code)
        {
            return ERR_isError(code);
        }

        /*! ZSTD_getErrorName() :
         *  provides error code string from function result (useful for debugging) */
        public static string ZSTD_getErrorName(nuint code)
        {
            return ERR_getErrorName(code);
        }

        /*! ZSTD_getError() :
         *  convert a `size_t` function result into a proper ZSTD_errorCode enum */
        public static ZSTD_ErrorCode ZSTD_getErrorCode(nuint code)
        {
            return ERR_getErrorCode(code);
        }

        /*! ZSTD_getErrorString() :
         *  provides error code string from enum */
        public static string ZSTD_getErrorString(ZSTD_ErrorCode code)
        {
            return ERR_getErrorString(code);
        }

        /*=**************************************************************
         *  Custom allocator
         ****************************************************************/
        public static void* ZSTD_customMalloc(nuint size, ZSTD_customMem customMem)
        {
            if (customMem.customAlloc != null)
                return customMem.customAlloc(customMem.opaque, size);
            return malloc(size);
        }

        public static void* ZSTD_customCalloc(nuint size, ZSTD_customMem customMem)
        {
            if (customMem.customAlloc != null)
            {
                /* calloc implemented as malloc+memset;
                 * not as efficient as calloc, but next best guess for custom malloc */
                void* ptr = customMem.customAlloc(customMem.opaque, size);
                memset(ptr, 0, (uint)size);
                return ptr;
            }

            return calloc(1, size);
        }

        public static void ZSTD_customFree(void* ptr, ZSTD_customMem customMem)
        {
            if (ptr != null)
            {
                if (customMem.customFree != null)
                    customMem.customFree(customMem.opaque, ptr);
                else
                    free(ptr);
            }
        }
    }
}