#ifndef StreamUtils_h__
#define StreamUtils_h__
#include <fstream>
#include <ostream>
#include <string>
#include <sstream>
#include <vector>

#ifdef WIN32
#include <windows.h>
#include <shlobj.h>
#endif
// trim from left
inline std::string &ltrim(std::string &s, const char* t = " \t\n\r\f\v")
{
    s.erase(0, s.find_first_not_of(t));
    return s;
}

// trim from right
inline std::string &rtrim(std::string &s, const char* t = " \t\n\r\f\v")
{
    s.erase(s.find_last_not_of(t) + 1);
    return s;
}

// trim from left & right
inline std::string &trim(std::string &s, const char* t = " \t\n\r\f\v")
{
    return ltrim(rtrim(s, t), t);
}

inline std::vector<std::vector<std::string>> ReadSpaceDelimitedFile(const char* file)
{
    std::vector<std::vector<std::string>> data;
    std::ifstream infile(file);
    if (!infile.is_open())
    {
        std::cout << "could not open the file " << file << std::endl;
        return data;
    }
    while (infile)
    {
        std::string s;
        if (!getline(infile, s)) break;

        std::istringstream ss(s);
        std::vector <std::string> record;

        while (ss)
        {
            std::string s;

            ss >> s;

            //if (!getline(ss, s, ',')) break;
            if (!trim(s).empty())
                record.push_back(s);
        }

        data.push_back(record);
    }
    if (!infile.eof())
    {
        std::cerr << "Fooey!\n";
    }

    return data;
}
inline std::string GetFileName(const std::string &FilePath)
{
    size_t found;
    found = FilePath.find_last_of("/\\");
    return FilePath.substr(found + 1);
}
inline std::string GetFileNameWithoutExtention(const std::string &FilePath)
{
    std::string filename = GetFileName(FilePath);
    size_t found;
    found = filename.find_last_of(".");
    return filename.substr(0,found);
}
inline std::string GetFileNameExtention(const std::string &FilePath)
{
    std::string filename = GetFileName(FilePath);
    size_t found;
    found = filename.find_last_of(".");
    return filename.substr(found+1,filename.size());
}
inline std::string GetFileFolderName(const std::string &FilePath)
{
    size_t found;
    found = FilePath.find_last_of("/\\");

    return FilePath.substr(0,found);
}
inline std::string GetMyDocsDirA()
{
    char szPath[MAX_PATH];
    std::string my_docs_dir;
    if (SUCCEEDED(SHGetFolderPathA(NULL,
                                   CSIDL_PERSONAL | CSIDL_FLAG_CREATE,
                                   NULL,
                                   0,
                                   szPath)))
    {
        my_docs_dir = szPath;
    }

    return my_docs_dir;
};
inline std::string GetExePathA()
{

    char szPath[_MAX_PATH + 1];
    GetModuleFileNameA(0, szPath, _MAX_PATH + 1);

    return std::string(szPath);
}
inline std::string ReadWholeFile(const std::string& FilePath)
{
    //read the whole file
    std::ifstream ifs(FilePath);
    ifs.seekg(0, std::ios::end);
    size_t length = ifs.tellg();
    std::string Buffer(length, '0');
    ifs.seekg(0, std::ios::beg);
    ifs.read(&Buffer[0], length);
    ifs.close();

    return Buffer;

}
inline std::vector<std::string> GetFilesInDirectoryA(const std::string directory, const std::string filter="*")
{
    std::vector<std::string> out;
#ifdef WIN32
    HANDLE dir;
    WIN32_FIND_DATAA file_data;

    if ((dir = FindFirstFileA((directory + "/" + filter).c_str(), &file_data)) == INVALID_HANDLE_VALUE)
        return out; /* No files found */

    do
    {
        const std::string file_name = file_data.cFileName;
        const std::string full_file_name = directory + "/" + file_name;
        const bool is_directory = (file_data.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) != 0;

        if (file_name[0] == '.')
            continue;

        if (is_directory)
            continue;

        out.push_back(full_file_name);
    }
    while (FindNextFileA(dir, &file_data));

    FindClose(dir);

    return out;
#else
    DIR* dir;
    class dirent* ent;
    class stat st;

    dir = opendir(directory);
    while ((ent = readdir(dir)) != NULL)
    {
        const string file_name = ent->d_name;
        const string full_file_name = directory + "/" + file_name;

        if (file_name[0] == '.')
            continue;

        if (stat(full_file_name.c_str(), &st) == -1)
            continue;

        const bool is_directory = (st.st_mode & S_IFDIR) != 0;

        if (is_directory)
            continue;

        out.push_back(full_file_name);
    }
    closedir(dir);
    return out;
#endif
} // GetFilesInDirectory

#endif // StreamUtils_h__
