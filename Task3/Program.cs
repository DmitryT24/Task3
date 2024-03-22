using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Task3
{
    
    static void CleanCatalog(string CatalogPath)
    {
        const double timeOut = 1;
        DateTime dateTimeSet = DateTime.Now;

        ///  интервал в 30 минут
        TimeSpan timeInterval = TimeSpan.FromMinutes(timeOut);


        foreach (string fileOrFolder in Directory.GetFileSystemEntries(CatalogPath))
        {
            var fileInfo = new FileInfo(fileOrFolder);
            var directoryInfo = new DirectoryInfo(fileOrFolder);

            if (fileInfo.LastAccessTime.Add(timeInterval) < dateTimeSet || directoryInfo.LastAccessTime.Add(timeInterval) < dateTimeSet)
            {
                if (fileInfo.Attributes.HasFlag(FileAttributes.Directory))
                {
                    //directoryInfo.Attributes &= ~(FileAttributes.Hidden | FileAttributes.ReadOnly);

                    ///пробегаемся по каталогам и удаляем содержимое 
                    CleanCatalog(fileOrFolder);
                    ///Console.WriteLine(fileOrFolder);
                    Directory.Delete(fileOrFolder, true);

                }
                else
                {
                    //fileInfo.Attributes &= ~(FileAttributes.Hidden | FileAttributes.ReadOnly);
                    //fileInfo.Attributes &= ~FileAttributes.Archive;
                    //Console.WriteLine(fileInfo);
                    File.Delete(fileOrFolder);
                }
            }
        }
    }


    static long CountSizeFolder(string folderPath)
    {
        long sizeFolder = 0;
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        foreach (FileInfo fileInfo in directoryInfo.GetFiles())
        {
            sizeFolder += fileInfo.Length;
        }


        foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories())
        {
            sizeFolder += CountSizeFolder(dirInfo.FullName);
        }

        return sizeFolder;
    }


    static void Main(string[] args)
    {
        /// если путь небыл передан в программу, ввести в поле программы
        string PathCatalog = "";
        long cleared = 0;

        if (args.Length == 0)
        {

            Console.WriteLine("Пожалуйста, введите путь каталога:");
            PathCatalog = Console.ReadLine();
            // return;
        }
        else
        {
            if (!string.IsNullOrEmpty(args[0]))
            {
                PathCatalog = args[0];
            }
            else
            {
                Console.WriteLine("Путь введен не корректно! Перезапустите программу и попробуйте снова!");
                Console.ReadKey();
                return;
            }
        }

        if (!Directory.Exists(PathCatalog))
        {
            Console.WriteLine("Путь введен не корректно! Перезапустите программу и попробуйте снова!");
            Console.ReadKey();
            return;
        }

        try
        {
            Console.WriteLine($" Исходный размер папки:{ cleared = CountSizeFolder(PathCatalog) } байт"); 
            CleanCatalog(PathCatalog);
            Console.WriteLine($" Освобождено:{ (long)(cleared  - CountSizeFolder(PathCatalog)) } байт");
            Console.WriteLine($" Текущий размер папки:{ (long)(CountSizeFolder(PathCatalog)) } байт");
            //Console.WriteLine("Каталог успешно очищен от содержимого, которое не использовались более 30 минут.");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Упс! Ошибка: {ex.Message}");
            Console.ReadKey();
        }
    }

}
