using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
	public class FitnesCentar
	{
		public string Naziv { get; set; }
		public string Adresa { get; set; }
		public string GodinaOtvaranja { get; set; }
		public string Vlasnik { get; set; }
		public double MesecnaClanarina { get; set; }
		public double GodisnjaClanarina { get; set; }
		public double CenaTreninga { get; set; }
		public double CenaGrupnogTreninga { get; set; }
		public double CenaPersonalnogTreninga { get; set; }
		public bool Obrisan { get; set; } = false;
	}
}