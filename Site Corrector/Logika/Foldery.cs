using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Corrector
{
    class Foldery
    {
        public static ObservableCollection<Plik> lista_plikow(string typ, string folder)
        {
            ObservableCollection<Plik> lista = new ObservableCollection<Plik>();
            try {


                System.IO.DirectoryInfo dirInfo = new DirectoryInfo(folder);
                System.IO.FileInfo[] fileNames = dirInfo.GetFiles(typ, SearchOption.AllDirectories);

                foreach (System.IO.FileInfo fi in fileNames)
                {
                    Plik plik = new Plik();
                    plik.AdresNaDysku = fi.FullName;
                    plik.LokalizacjaNaSerwerze = fi.FullName.Replace(folder, "").Replace('\\', '/');
                    plik.Nazwa = fi.Name;
                    plik.Rozmiar = fi.Length;
                    plik.Rozmiar_po_kompresji = fi.Length;

                    plik.Status = "Przed";

                    plik.Typ = Path.GetExtension(fi.Name);
                    plik.Procent_pobrania = 0;

                    lista.Add(plik);
                }
                
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Program.loguj("Nie autoryzowany dostęp: " + UAEx.Message);
                //wypisz_komunikat("Nie autoryzowany dostęp: " + UAEx.Message);
                //Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Program.loguj("Za długa ścierzka: " + PathEx.Message);
                //wypisz_komunikat("Za długa ścierzka: " + PathEx.Message);
                //Console.WriteLine(PathEx.Message);
            }

            return lista;
        }
        public static void copy(string sourceDirName, string destDirName, bool copySubDirs, bool nadpisac)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, nadpisac);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    copy(subdir.FullName, temppath, copySubDirs, nadpisac);
                }
            }
        }
    }
}
