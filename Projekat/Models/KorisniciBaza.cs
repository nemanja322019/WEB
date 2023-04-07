using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Projekat.Models
{
	public class KorisniciBaza
	{
		public static List<Korisnik> KorisniciList { get; set; } = new List<Korisnik>()
		{
			//new Korisnik(){ KorisnickoIme = "Vlasnik2",Lozinka = "vlasnik2", Ime = "Pera", Prezime = "Peric", Pol="Muski",Email = "peraperic@gmail.com",DatumRodjenja = new DateTime(1988, 7, 7, 0, 0, 0).ToString("dd/MM/yyyy"),Uloga = Uloga.Vlasnik.ToString(),FitnesCentriVlasnistvo={FitnesCentriBaza.FitnesCentriList[1].Naziv } }
		};

		public static Korisnik Prijava(string korisnickoime, string lozinka)
		{
			UcitajKorisnike();

			return KorisniciList.Find(item => item.KorisnickoIme == korisnickoime && item.Lozinka == lozinka && item.Blokiran==false);
		}

		public static bool Registracija(Korisnik korisnik)
		{
			UcitajKorisnike();

			if (!DateTime.TryParseExact(korisnik.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
			{
				return false;
			}

				bool postoji = false;
			foreach(Korisnik k in KorisniciList)
			{
				if(k.KorisnickoIme == korisnik.KorisnickoIme)
				{
					postoji = true;
				}
			}
			if(postoji)
			{
				return false;
			}
			else
			{
				korisnik.Uloga = Uloga.Posetilac.ToString();
				KorisniciList.Add(korisnik);
				SacuvajKorisnike();
				return true;
			}
		}

		public static bool IzmeniKorisnika(string staroKorisnickoIme, Korisnik korisnik)
		{
			UcitajKorisnike();
			if (!DateTime.TryParseExact(korisnik.DatumRodjenja, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
			{
				return false;
			}
			foreach (Korisnik k in KorisniciList)
			{
				if (k.KorisnickoIme == staroKorisnickoIme)
				{
					string uloga = k.Uloga;
					var index = KorisniciList.IndexOf(k);
					KorisniciList[index] = korisnik;
					KorisniciList[index].Uloga = uloga;
					KorisniciList[index].KorisnickoIme = staroKorisnickoIme;
					SacuvajKorisnike();
					return true;
				}
			}
			return false;
		}

		public static bool BlokirajTrenera(string korisnickoIme)
		{
			if (KorisniciList.Exists(item => item.KorisnickoIme == korisnickoIme))
			{
				Korisnik korisnik = KorisniciList.Find(item => item.KorisnickoIme == korisnickoIme);
				korisnik.Blokiran = true;
				SacuvajKorisnike();
				return true;
			}
			return false;
		}

		public static bool ZaposliTrenera(string korisnickoIme, string nazivFitnesCentra)
		{
			if(KorisniciList.Exists(item => item.KorisnickoIme == korisnickoIme))
			{
				Korisnik korisnik = KorisniciList.Find(item => item.KorisnickoIme == korisnickoIme);
				if(korisnik.FitnesCentarAngazovan == null)
				{
					korisnik.Uloga = Uloga.Trener.ToString();
					korisnik.FitnesCentarAngazovan = nazivFitnesCentra;
					SacuvajKorisnike();
					return true;
				}
			}
			return false;
		}

		public static bool DodajTreningTreneru(string korisnickoIme, string nazivTreninga)
		{
			if (KorisniciList.Exists(item => item.KorisnickoIme == korisnickoIme))
			{
				KorisniciList[KorisniciList.FindIndex(item => item.KorisnickoIme == korisnickoIme)].AngazovaniTreninzi.Add(nazivTreninga);
				SacuvajKorisnike();
				return true;
			}
			return false;
		}

		public static bool DodajTreningKorisniku(string nazivTreninga, string korisnickoIme)
		{
			if(KorisniciList.Exists(item => item.KorisnickoIme == korisnickoIme))
			{
				KorisniciList[KorisniciList.FindIndex(item => item.KorisnickoIme == korisnickoIme)].PrijavljeniTreninzi.Add(nazivTreninga);
				SacuvajKorisnike();
				return true;
			}
			return false;
		}


		public static void UcitajKorisnike()
		{
			string putanja = "~/korisnici.json";
			putanja = HostingEnvironment.MapPath(putanja);

			if (!File.Exists(putanja))
			{
				File.Create(putanja);
			}

			string json = File.ReadAllText(putanja);

			KorisniciList = JsonConvert.DeserializeObject<List<Korisnik>>(json);
		}

		public static void SacuvajKorisnike()
		{
			string putanja = "~/korisnici.json";
			putanja = HostingEnvironment.MapPath(putanja);

			using (StreamWriter file = File.CreateText(putanja))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, KorisniciList);
			}
		}
	}
}