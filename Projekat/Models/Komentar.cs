using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
	public class Komentar
	{
		public string Id { get; set; } = "i" + DateTime.Now.Ticks.GetHashCode().ToString();
		public string Korisnik { get; set; }
		public string FitnesCentar { get; set; }
		public string Tekst { get; set; }
		public int Ocena { get; set; }
		public bool Odobren { get; set; } = false;
	}
}