using AutomatisationControlCsv.Models;
using AutomatisationControlCsv.Models.Entities;
using AutomatisationControlCsv.Tools;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace AutomatisationControlCsv.Controllers
{
    public class PivotController : ApiController
    {

        IPivotMethod pivotMethod = new PivotMethod();

        static string fileRepository = ConfigurationManager.AppSettings.GetAppConfigValues("fileRepository");


        // GET: api/Pivot
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Pivot/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Pivot
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Pivot/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Pivot/5
        public void Delete(int id)
        {
        }

        //On va controler la conformité de l'object en retour
        //champ fichier_pivot_f ==> l'extension doit être un .csv
        //==> le nom du fichier doit commencer par un nombre suivi d'un point suivi d'un nom
        [Route("api/Pivot/GetPivotReport")]
        public List<PivotReportClassesPivotReportClass> GetPivotReport()
        {
            TextWriterTraceListener tr1 = new TextWriterTraceListener(System.Console.Out);
            Debug.Listeners.Add(tr1);

            TextWriterTraceListener tr2 = new TextWriterTraceListener(System.IO.File.CreateText(fileRepository + "logout.txt"));
            Debug.Listeners.Add(tr2);

            Debug.WriteLine("Debug Method Starting");


            //Listes des object à tester
            PivotReportClasses pivotReportClasses = pivotMethod.GetPivotReport();

            PivotReportClassesPivotReportClass[] reportClassesPivotReportClasses = pivotReportClasses.Items;

            List<PivotReportClassesPivotReportClass> GetPivotReportClassToAnalyse = new List<PivotReportClassesPivotReportClass>() ;

            foreach (PivotReportClassesPivotReportClass p in reportClassesPivotReportClasses)
            {
                //Je vais chercher la taille du stringBuilder pour connaitre le rang des 3 derniers chiffres pour déterminer l'extension
                StringBuilder sb01 = new StringBuilder(p.fichier_pivot);
                int taille = sb01.Length;
                string ext = "" + sb01[taille - 4] + sb01[taille - 3] + sb01[taille - 2] + sb01[taille - 1];




                if (ext.Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                {
                    Debug.WriteLine("id " + p.id_de_suivi + " - extension : " + ext + " A contrôler");

                    string pre = "";
                    int rang = 0;
                    int cumul = 0;
                    Boolean toContinue = true;

                    //On est en dehors de l'extension cad nom du fichier réel
                    for(int i = 0; i < sb01.Length - 4; i++)
                    {
                        //i > 0 pour éviter une exception
                        if( i > 0 && toContinue == true)
                        {
                            pre = pre + sb01[i - 1].ToString();
                        }

                        //je cherche le rang du 1er point
                        if(sb01[i].ToString().Equals(".") && toContinue == true)
                        {
                            cumul = cumul + 1;
                            rang = i;
                            toContinue = false;
                        }
                    }

                //condition OK
                if(cumul == 1 && rang != 0)
                    {
                        //je dois vérifier si c'est un int avant le point
                        try
                        {
                            int.Parse(pre);
                            GetPivotReportClassToAnalyse.Add(p);
                            Debug.WriteLine("id " + p.id_de_suivi + " rang 1er point : " + rang + " préfixe : "+ pre + " conforme");
                        }
                        catch(FormatException ex)
                        {
                            //la valeure avant le point n'est pas un integer
                            Debug.WriteLine("id " + p.id_de_suivi + " rang 1er point : " + rang + " préfixe : " + pre + " non conforme - la valeure avant le point n'est pas un integer");
                        }
                    }

                //condition non Ok - commencant par un point
                if(rang == 0 && cumul != 0)
                    {
                        Debug.WriteLine("id " + p.id_de_suivi + " rang 1er point : " + rang + " non conforme - commencant par un point");
                    }
                //condition non ok - absence de point
                if(cumul == 0)
                    {
                        Debug.WriteLine("id " + p.id_de_suivi + " non conforme - absence de point");
                    }


                }

                else if (!ext.Equals(".csv", StringComparison.CurrentCultureIgnoreCase))
                {
                    Debug.WriteLine("id " + p.id_de_suivi + " - extension : " + ext + " Non valide ");
                }

            }

            // GetPivotReportClassToAnalyse comprend tous les .csv qu'on va analyser

            var pack = new List<ActifSupportClass>();

            foreach(PivotReportClassesPivotReportClass p in GetPivotReportClassToAnalyse)
            {
                //test sur 1 seul valeure
                if (p.id_de_suivi.Equals("3537337"))
                {
                    StreamReader streamReader = new StreamReader(@"" + fileRepository + p.fichier_pivot_renommee + ".csv", System.Text.Encoding.Default);

                    //lire la 1ere ligne
                    //string line = "";
                    //int i = 0;

                    //while((line = streamReader.ReadLine()) !=null)
                    //{
                    //    if (i == 0)
                    //    {
                    //        Debug.WriteLine("line 0 : " + line);
                    //    }
                    //    i++;
                    //}

                    Debug.WriteLine("chemin : " + fileRepository + p.fichier_pivot_renommee + ".csv", System.Text.Encoding.Default);

                    CsvReader csv = new CsvReader(streamReader);

                    csv.Configuration.RegisterClassMap<ActifSupportMap>();
                    pack = csv.GetRecords<ActifSupportClass>().ToList();

                }


            }

            foreach (ActifSupportClass aa in pack)
            {
                Debug.WriteLine("aa.Nom : " + aa.Nom);
            }

            Debug.WriteLine("Debug Method Ending");

            Debug.Flush();

            return GetPivotReportClassToAnalyse;
        }



    }
}
