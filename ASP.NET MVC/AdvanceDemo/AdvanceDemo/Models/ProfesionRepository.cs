using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeCamp2009Demos.Models
{
    public class ProfesionRepository
    {
          public static List<String> listaProfesiones  = new List<string>{"Arquitecto Sr.", 
                                                                          "Arquitecto Jr.",
                                                                          "Desarrollador Sr.", 
                                                                          "Desarrollador Jr.", 
                                                                          "Líder de Proyecto", 
                                                                          "Líder Técnico",
                                                                          "Analista de Sistemas",
                                                                          "Project Manager"};

          public string[] GetProfesiones(string filter)
          {
              return listaProfesiones.Where(p => p.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase)).ToArray();
          }
                                                     
    }
}
