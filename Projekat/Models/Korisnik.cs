using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
	public class Korisnik
	{
		public string KorisnickoIme { get; set; }
		public string Lozinka { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }
		public string Pol { get; set; }
		public string Email { get; set; }
		public string DatumRodjenja { get; set; }
		public string Uloga { get; set; }
		public List<string> PrijavljeniTreninzi { get; set; } = new List<string>();
		public List<string> AngazovaniTreninzi { get; set; } = new List<string>();
		public string FitnesCentarAngazovan { get; set; }
		public List<string> FitnesCentriVlasnistvo { get; set; } = new List<string>();
		public bool Blokiran { get; set; } = false;
	}
}