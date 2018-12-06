using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatisationControlCsv.Models.Entities
{
    sealed class ActifSupportMap : ClassMap<ActifSupportClass>
    {
        public ActifSupportMap()
        {
            AutoMap();
            {
                Map(m => m.Nom).Name("Nom");
                Map(m => m.Source).Name("Source");
                Map(m => m.Code_Local).Name("Code Local");
                Map(m => m.Depositaire).Name("Dépositaire");
                Map(m => m.Type_AS).Name("Type AS");
                Map(m => m.Domaine_Homogene).Name("Domaine Homogène");
                Map(m => m.Description).Name("Description");
                Map(m => m.Environnement).Name("Environnement");
                Map(m => m.SI_D_Origine).Name("SI d'origine");
                Map(m => m.Statut).Name("Statut");
                Map(m => m.Univers).Name("Univers");
            }
        }
    }
}