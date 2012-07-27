using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeCamp2009Demos.Models
{
    public static class AttenderRepository
    {
        private static List<Attender> listaAsistentes  = new List<Attender>{
                                                        new Attender() { Id = 1, Age = 20, Name = "Esteban", Profesion = "Desarrollador Jr." },
                                                        new Attender() { Id = 2, Age = 12, Name = "Ricardo", ReturnNextYear = true},
                                                        new Attender() { Id = 3, Age = 15, Name = "Claudio", Profesion = "Desarrollador Sr." },
                                                        new Attender() { Id = 4, Age = 22, Name = "Mariano",  ReturnNextYear = true, Profesion = "Desarrollador Jr." },
                                                        new Attender() { Id = 5, Age = 33, Name = "Rodolfo" , ReturnNextYear = true},
                                                        new Attender() { Id = 6, Age = 41, Name = "Leandro", Profesion = "Arquitecto" },
                                                        new Attender() { Id = 7, Age = 51, Name = "Marcos" },
                                                        new Attender() { Id = 8, Age = 32, Name = "Ruben",  ReturnNextYear = true, Profesion = "Desarrollador Jr." },
                                                        new Attender() { Id = 9, Age = 57, Name = "Ernesto", Profesion = "Desarrollador Jr." },
                                                        new Attender() { Id = 10, Age = 6, Name = "Mario",  ReturnNextYear = true, Profesion = "Líder Técnico" }};

        public static IEnumerable<Attender> GetAttenders()
        {
            return listaAsistentes
                .OrderBy(u => u.Name);
        }

        public static IEnumerable<Attender> GetAttenders(string filter)
        {
            return listaAsistentes
                .Where(u => u.Name.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase))
                .OrderBy(u => u.Name);
        }

        public static Attender GetAttender(int id)
        {
            return listaAsistentes
                .Where(u => u.Id == id).FirstOrDefault();
        }

        public static void AddAttender(Attender attender)
        {
            attender.Id = listaAsistentes.Count() + 1;
            listaAsistentes.Add(attender);
        }
    }
}
