using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;

class Hash
{
    static void Main(string[] args)
    {
        // Verify the correct number of command-line arguments.
        if (args.Length != 2)
        {
            // Show the help message if an incorrect number of arguments was specified.
            ShowHelp();
            return;
        }
        else
        {
            byte[] hashValue = null;
            // Verify that the specified file exists.
            if (!File.Exists(args[1]))
            {
                // Show the help message if a non-existent filename was specified.
                ShowHelp();
                return;
            }
            else
            {
                try
                {
                    // Create a fileStream for the file.
                    FileStream fileStream = File.OpenRead(args[1]);
                    // Be sure it's positioned to the beginning of the stream.
                    fileStream.Position = 0;
                    // Use the specified hash algorithm.
                    switch (args[0].ToUpper())
                    {
                        case "MD5":
                            // Compute the MD5 hash of the fileStream.
                            hashValue = MD5.Create().ComputeHash(fileStream);
                            break;
                        case "SHA1":
                            // Compute the SHA1 hash of the fileStream.
                            hashValue = SHA1.Create().ComputeHash(fileStream);
                            break;
                        case "SHA256":
                            // Compute the SHA256 hash of the fileStream.
                            hashValue = SHA256.Create().ComputeHash(fileStream);
                            break;
                        case "SHA384":
                            // Compute the SHA384 hash of the fileStream.
                            hashValue = SHA384.Create().ComputeHash(fileStream);
                            break;
                        case "SHA512":
                            // Compute the SHA512 hash of the fileStream.
                            hashValue = SHA512.Create().ComputeHash(fileStream);
                            break;
                        case "BASE64":
                            // Compute the BASE64 hash of the fileStream.
                            byte[] binaryData = new Byte[fileStream.Length];
                            long bytesRead = fileStream.Read(binaryData, 0, (int)fileStream.Length);
                            if (bytesRead != fileStream.Length)
                            {
                                throw new Exception(String.Format("Number of bytes read ({0}) does not match file size ({1}).", bytesRead, fileStream.Length));
                            }
                            string base64String = System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
                            Console.WriteLine("File: {0}\r\nBASE64 Hash: {1}", fileStream.Name, base64String);
                            hashValue = null;
                            break;
                        default:
                            // Display the help message if an unrecognized hash algorithm was specified.
                            ShowHelp();
                            return;
                    }
                    if (hashValue != null)
                    {
                        // Write the hash value to the Console.
                        PrintHashData(args[0].ToUpper(), fileStream.Name, hashValue);
                    }
                    // Close the file.
                    fileStream.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: {0}", ex.Message);
                }
            }
        }
    }

    // Display the help message.
    private static void ShowHelp()
    { Console.WriteLine("HASH.exe <hash algorithm> <file name>\n\n" +
            "\tWhere <hash algorithm> is one of the following:\n" +
            "\t\tBASE64\n\t\tMD5\n\t\tSHA1\n\t\tSHA256\n\t\tSHA384\n\t\tSHA512\n");
    }

    // Print the hash data in a readable format.
    private static void PrintHashData(string algorithm, string fileName, byte[] array)
    {
        Console.Write("File: {0}\r\n{1} Hash: ", fileName, algorithm);
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(String.Format("{0:X2}", array[i]));
        }
        Console.WriteLine();

        Console.Write("File: {0}\r\n{1} Hash: ", fileName, algorithm);
        for (int i = 0; i < array.Length; i++)
        {
            Console.Write(String.Format("{0:x2}", array[i]));
        }
        Console.WriteLine();
    }
}