#include "svn_version.h"
// instructions read from link below
//http://www.zachburlingame.com/2011/03/integrating-the-subversion-revision-into-the-version-automatically-with-native-cc/

#define STRINGIZE2(s) #s
#define STRINGIZE(s) STRINGIZE2(s)

//VERSION_REVISION  
//0 for alpha(status)
//1 for beta(status)
//2 for release candidate
//3 for (final) release

#define VERSION_MAJOR               1
#define VERSION_MINOR               0
#define VERSION_REVISION            0 
#define VERSION_BUILD               SVN_REVISION

#if SVN_LOCAL_MODIFICATIONS
#define VERSION_MODIFIER "M"
#else
#define VERSION_MODIFIER
#endif

#define VER_FILE_DESCRIPTION_STR    "Description"
#define VER_FILE_VERSION            VERSION_MAJOR, VERSION_MINOR, VERSION_REVISION, VERSION_BUILD


#define VER_FILE_VERSION_STR        STRINGIZE(VERSION_MAJOR)        \
                                    "." STRINGIZE(VERSION_MINOR)    \
                                    "." STRINGIZE(VERSION_REVISION) \
                                    "." STRINGIZE(VERSION_BUILD)    \

#define VER_PRODUCTNAME_STR         "Application"
#define VER_PRODUCT_VERSION         VER_FILE_VERSION
#define VER_PRODUCT_VERSION_STR     VER_FILE_VERSION_STR
#define VER_ORIGINAL_FILENAME_STR   VER_PRODUCTNAME_STR ".exe"
#define VER_INTERNAL_NAME_STR       VER_ORIGINAL_FILENAME_STR
#define VER_COPYRIGHT_STR           "Copyright (C) 2016"

#ifdef _DEBUG
#define VER_VER_DEBUG             VS_FF_DEBUG
#else
#define VER_VER_DEBUG             0
#endif

#define VER_FILEOS                  VOS_NT_WINDOWS32
#define VER_FILEFLAGS               VER_VER_DEBUG
#define VER_FILETYPE                VFT_APP
