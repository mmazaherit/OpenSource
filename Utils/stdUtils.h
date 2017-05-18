#ifndef StreamUtils_h__
#define StreamUtils_h__
#include <fstream>
#include <ostream>
#include <string>
#include <sstream>
#include <vector>
#include <map>
#include <istream>
#include <iostream>
#include <iomanip>

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

//http://stackoverflow.com/questions/16605967/set-precision-of-stdto-string-when-converting-floating-point-values
template <typename T>
static inline std::string to_string_with_precision(const T a_value, const int n = 6)
{
    std::ostringstream out;
    out << std::setprecision(n) << a_value;
    return out.str();
}

inline std::vector<std::vector<std::string> > ReadSpaceDelimitedFile(const char* file)
{
    std::vector<std::vector<std::string> > data;
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
    std::string my_docs_dir;
    
    #ifdef WIN32
    char szPath[MAX_PATH];
    if (SUCCEEDED(SHGetFolderPathA(NULL,
                                   CSIDL_PERSONAL | CSIDL_FLAG_CREATE,
                                   NULL,
                                   0,
                                   szPath)))
    {
        my_docs_dir = szPath;
    }
    #endif
    return my_docs_dir;
};
inline std::string GetExePathA()
{
    #ifdef WIN32
    char szPath[_MAX_PATH + 1];
    GetModuleFileNameA(0, szPath, _MAX_PATH + 1);
    
    return std::string(szPath);
    #else
    return "";
    #endif
}
inline std::string ReadWholeFile(const std::string& FilePath)
{
    //read the whole file
    std::ifstream ifs(FilePath.c_str());
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
    
    dir = opendir(directory.c_str());
    while ((ent = readdir(dir)) != NULL)
    {
        const std::string file_name = ent->d_name;
        const std::string full_file_name = directory + "/" + file_name;
    
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


////////////////////////////////////////////////////////////////////////// VECTOR UTILS

template <typename T>
inline std::vector<size_t> sort_indexes_asc(const std::vector<T> &v)
{

    // initialize original index locations
    std::vector<std::size_t> idx(v.size());
    std::iota(idx.begin(), idx.end(), 0);
    
    // sort indexes based on comparing values in v
    std::sort(idx.begin(), idx.end(),
    [&v](std::size_t i1, std::size_t i2) {return v[i1] < v[i2]; });
    
    return idx;
}
template <typename T>
inline std::vector<size_t> sort_indexes_des(const std::vector<T> &v)
{

    // initialize original index locations
    std::vector<std::size_t> idx(v.size());
    std::iota(idx.begin(), idx.end(), 0);
    
    // sort indexes based on comparing values in v
    std::sort(idx.begin(), idx.end(),
    [&v](std::size_t i1, std::size_t i2) {return v[i1] > v[i2]; });
    
    return idx;
}

//http://stackoverflow.com/questions/9323903/most-efficient-elegant-way-to-clip-a-number
template <typename T>
T inline clip(const T& n, const T& lower, const T& upper)
{
    return std::max(lower, std::min(n, upper));
}


//http://stackoverflow.com/questions/6892754/creating-a-simple-configuration-file-and-parser-in-c
inline std::map<std::string, std::string> ReadConfig(const std::string &ConfigFile)
{
    std::ifstream is_file(ConfigFile.c_str());
	std::map<std::string, std::string> config;
	if (!is_file.is_open())
    {
        std::cout << "could not find the config file " << ConfigFile << std::endl;
        return config;
    }   
    std::string line;
    while (std::getline(is_file, line))
    {
		if (line.find('#')!=std::string::npos)
        continue;
        std::istringstream is_line(line);
        std::string key;
        if (std::getline(is_line, key, '='))
        {
            key = trim(key);
            std::string value;
            if (std::getline(is_line, value))
            {
                if (config.count(key))
                    std::cout << "Warning duplicate setting " << key << std::endl;
                    
                config[key] = trim(value);
            }
            
        }
    }
    is_file.close();
    
    return config;
}

////////////////////////////////////////////////////////////////////////// MATH UTILS
template<typename T>
static inline T DotProduct31(const T* const A, const T* const B)
{
    return A[0] * B[0] + A[1] * B[1] + A[2] * B[2];
}
template<typename T>
static inline void Subtract31(const T* const A, const T* const B, T* AminusB)
{
    AminusB[0] = A[0] - B[0];
    AminusB[1] = A[1] - B[1];
    AminusB[2] = A[2] - B[2];
}

template<typename T>
static inline T Distance31(const T* const vec1, const T* const vec2)
{
    T res[3];
    Subtract31(vec1, vec2, res);
    return sqrt(DotProduct31(res, res));
}


template<typename T>
static inline void ComputeMean(int n, const T* const Values, double MeanMinMax[3])
{
    double Sum = 0;
    T Max = -std::numeric_limits<T>::max();
    T Min = std::numeric_limits<T>::max();
    
    for (int i = 0; i < n; i++)
    {
        if (Values[i] > Max)
        {
            Max = Values[i];
        }
        else if (Values[i] < Min)
        {
            Min = Values[i];
        }
        
        Sum += Values[i];
    }
    
    MeanMinMax[0] = Sum / n;
    MeanMinMax[1] = Min;
    MeanMinMax[2] = Max;
    
}
template<typename T>
static inline double ComputeMean(int n, const T* const Values)
{
    double Sum = 0;
    for (int i = 0; i < n; ++i)
        Sum += Values[i];
        
    return Sum / n;
}

#endif // StreamUtils_h__
