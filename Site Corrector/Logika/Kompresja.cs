using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMarkupMin.Core;
using WebMarkupMin.Core.Minifiers;
using WebMarkupMin.Core.Settings;
using Yahoo.Yui.Compressor;

namespace Site_Corrector
{
    class Kompresja
    {

        public static void html(Etap etap, ObservableCollection<Plik> pliki_html)
        {
            Parallel.ForEach(pliki_html, p =>
            {
                p.Status = "W trakcie ...";
                
                string adres = p.AdresNaDysku;
                string inputFileData = File.ReadAllText(adres, Encoding.UTF8);

                string outputFile = p.AdresNaDysku;

                //http://webmarkupmin.codeplex.com/wikipage?title=WebMarkupMin 1.0.0#HtmlMinifier_Chapter
                HtmlMinificationSettings ustawienia = new HtmlMinificationSettings();
                ustawienia.WhitespaceMinificationMode = WhitespaceMinificationMode.Medium; //Medium whitespace minification: executes all operations of the safe whitespace minification + removes all leading and trailing whitespace characters from outer and internal contents of block-level tags. 

                try
                {
                    var htmlMinifier = new HtmlMinifier(ustawienia);

                    MarkupMinificationResult result = htmlMinifier.Minify(inputFileData);


                    string output = result.MinifiedContent;
                    using (StreamWriter writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                    {
                        writer.Write(output);
                    }

                    FileInfo fi = new FileInfo(p.AdresNaDysku);
                    p.Rozmiar_po_kompresji = fi.Length;

                    p.Status = "Po";
                    p.Status_Pliku = StatusPliku.zoptymalizowany;

                    etap.Liczba_operacji_zrobionych++;
                }
                catch (Exception ex)
                {
                    Program.loguj("Wystąpił błąd podczas kompresji html: " + ex.Message);


                    FileInfo fi = new FileInfo(p.AdresNaDysku);
                    p.Rozmiar_po_kompresji = fi.Length;

                    p.Status = "BŁĄD";

                    p.Status_Pliku = StatusPliku.problem;

                    etap.Liczba_operacji_zrobionych++;
                }

            });
        }

        public static void css(Etap etap, ObservableCollection<Plik> pliki_css)
        {
            Parallel.ForEach(pliki_css, p =>
            {
                if (p.Nazwa.Count() < 60)
                {
                    p.Status = "W trakcie ...";

                    CssCompressor compressor = new CssCompressor();
                    string adres = p.AdresNaDysku;
                    string inputFileData = File.ReadAllText(adres, Encoding.UTF8);
                    
                    string outputFile = p.AdresNaDysku;

                    //jesli cos jest w pliku
                    if (inputFileData!="")
                    {
                        try
                        {
                            string output = compressor.Compress(inputFileData);
                            using (StreamWriter writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                            {
                                writer.Write(output);
                            }

                            FileInfo fi = new FileInfo(p.AdresNaDysku);
                            p.Rozmiar_po_kompresji = fi.Length;

                            p.Status = "Po";
                            p.Status_Pliku = StatusPliku.zoptymalizowany;

                            etap.Liczba_operacji_zrobionych++;
                        }
                        catch (Exception ex)
                        {
                            Program.loguj("Wystąpił błąd podczas kompresji css: " + ex.Message);


                            FileInfo fi = new FileInfo(p.AdresNaDysku);
                            p.Rozmiar_po_kompresji = fi.Length;

                            p.Status = "BŁĄD";

                            p.Status_Pliku = StatusPliku.problem;

                            etap.Liczba_operacji_zrobionych++;
                        }
                    }
                    else//pusty plik
                    {
                        Program.loguj("Plik nie został zoptymalizowany bo był pusty: " + p.Nazwa);


                        FileInfo fi = new FileInfo(p.AdresNaDysku);
                        p.Rozmiar_po_kompresji = fi.Length;

                        p.Status = "Po";

                        p.Status_Pliku = StatusPliku.zoptymalizowany;

                        etap.Liczba_operacji_zrobionych++;
                    }
                }
            });
        }


        public static void js(Etap etap, ObservableCollection<Plik> pliki_js)
        {
            Parallel.ForEach(pliki_js, p =>
            {
                p.Status = "W trakcie ...";

                JavaScriptCompressor compressor = new JavaScriptCompressor();
                string adres = p.AdresNaDysku;
                string inputFileData = File.ReadAllText(adres, Encoding.UTF8);

                string outputFile = p.AdresNaDysku;
                
                //jesli cos jest w pliku
                if (inputFileData != "")
                {
                    try
                    {
                        string output = compressor.Compress(inputFileData);
                        using (StreamWriter writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                        {
                            writer.Write(output);
                        }


                        FileInfo fi = new FileInfo(p.AdresNaDysku);
                        p.Rozmiar_po_kompresji = fi.Length;

                        p.Status = "Po";
                        p.Status_Pliku = StatusPliku.zoptymalizowany;

                        etap.Liczba_operacji_zrobionych++;
                    }
                    catch (Exception ex)
                    {
                        Program.loguj("Wystąpił błąd podczas kompresji js: " + ex.Message);


                        FileInfo fi = new FileInfo(p.AdresNaDysku);
                        p.Rozmiar_po_kompresji = fi.Length;

                        p.Status = "BŁĄD";

                        p.Status_Pliku = StatusPliku.problem;

                        etap.Liczba_operacji_zrobionych++;
                    }

                }
                else//pusty plik
                {
                    Program.loguj("Plik nie został zoptymalizowany bo był pusty: "+p.Nazwa);


                    FileInfo fi = new FileInfo(p.AdresNaDysku);
                    p.Rozmiar_po_kompresji = fi.Length;

                    p.Status = "Po";

                    p.Status_Pliku = StatusPliku.zoptymalizowany;

                    etap.Liczba_operacji_zrobionych++;
                }

            });
        }


        public static void png(Projekt projekt, Etap etap, ObservableCollection<Plik> pliki_png, int procent_kompresji_png)
        {
            Parallel.ForEach(pliki_png, p =>
            {
                p.Status = "Trwa...";

                int exitCode;
                ProcessStartInfo processInfo;
                Process process;

                processInfo = new ProcessStartInfo("Exe/optipng.exe");
                // processInfo.Arguments = "AAAAAAAAAAAAAAAAAAAAAAAAAAA";
                //processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", "\"ble\"", "\"ble\"");

                List<string> lista = new List<string>();

                lista.Add("\"" + p.AdresNaDysku + "\"");


                lista.Add("-o" + projekt.ustawienia.jakosc_PNG.ToString());

                processInfo.Arguments = String.Join(" ", lista);

                // processInfo.Arguments += " "+"\"-o7" + "\"";

                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                //   *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                process = Process.Start(processInfo);
                //process.WaitForExit();

                // *** Read the streams ***
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                exitCode = process.ExitCode;

                //wypisz_komunikat("Output: " + output);
                //wypisz_komunikat("Error: " + error);
                //wypisz_komunikat("Exitcode: " + exitCode);

                FileInfo fi = new FileInfo(p.AdresNaDysku);
                p.Rozmiar_po_kompresji = fi.Length;

                p.Status = "Po";
                p.Status_Pliku = StatusPliku.zoptymalizowany;

                etap.Liczba_operacji_zrobionych++;
            });
        }
    

        public static void jpg(Etap etap, ObservableCollection<Plik> pliki_jpg, int procent_kompresji_jpg)
        {
            Parallel.ForEach(pliki_jpg, p =>
            {
                int exitCode;
                ProcessStartInfo processInfo;
                Process process;

                processInfo = new ProcessStartInfo("Exe/jpegoptim.exe");
                // processInfo.Arguments = "AAAAAAAAAAAAAAAAAAAAAAAAAAA";
                //processInfo.Arguments = String.Format("\"{0}\" \"{1}\"", "\"ble\"", "\"ble\"");

                List<string> lista = new List<string>();
                

                lista.Add("\"" + p.AdresNaDysku + "\"");


                lista.Add("--strip-all");
                lista.Add("--max=" + procent_kompresji_jpg.ToString());


                processInfo.Arguments = String.Join(" ", lista);

                // processInfo.Arguments += " "+"\"-o7" + "\"";

                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                //   *** Redirect the output ***
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;

                process = Process.Start(processInfo);
                //process.WaitForExit();

                // *** Read the streams ***
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                exitCode = process.ExitCode;

                //wypisz_komunikat("Output: " + output);
                //wypisz_komunikat("Error: " + error);
                //wypisz_komunikat("Exitcode: " + exitCode);

                FileInfo fi = new FileInfo(p.AdresNaDysku);
                p.Rozmiar_po_kompresji = fi.Length;

                p.Status = "Po";
                p.Status_Pliku = StatusPliku.zoptymalizowany;

                etap.Liczba_operacji_zrobionych++;
            });

        }
    }
}
