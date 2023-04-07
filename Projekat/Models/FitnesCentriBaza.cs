using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Projekat.Models
{
	public class FitnesCentriBaza
	{
		public static List<FitnesCentar> FitnesCentriList { get; set; } = new List<FitnesCentar>()
		{
		//	new FitnesCentar() { Naziv = "Fitnes Centar 5", Adresa = "Adresa 1", GodinaOtvaranja = "2005", MesecnaClanarina = 2000, GodisnjaClanarina = 12000, CenaTreninga = 200, CenaGrupnogTreninga = 400, CenaPersonalnogTreninga = 700 },
		//	new FitnesCentar() { Naziv = "Fitnes Centar 2", Adresa = "Adresa 2", GodinaOtvaranja = "2007", Vlasnik="Vlasnik2", MesecnaClanarina = 2200, GodisnjaClanarina = 14000, CenaTreninga = 250, CenaGrupnogTreninga = 500, CenaPersonalnogTreninga = 1000 }
		//	new FitnesCentar() { Naziv = "Fitnes Centar 3", Adresa = "Adresa 3", GodinaOtvaranja = "2009", MesecnaClanarina = 1700, GodisnjaClanarina = 9000, CenaTreninga = 200, CenaGrupnogTreninga = 400, CenaPersonalnogTreninga = 500 },
		//	new FitnesCentar() { Naziv = "Fitnes Centar 4", Adresa = "Adresa 4", GodinaOtvaranja = "2007", MesecnaClanarina = 2500, GodisnjaClanarina = 15000, CenaTreninga = 300, CenaGrupnogTreninga = 450, CenaPersonalnogTreninga = 800 }
		};

		public static List<FitnesCentar> SviFitnesCentri()
		{
			UcitajFitnesCentre();
			return FitnesCentriList.Where(item => item.Obrisan == false).ToList();
		}

		public static bool DodajFitnesCentar(FitnesCentar fitnesCentar)
		{
			UcitajFitnesCentre();

			foreach (FitnesCentar fc in FitnesCentriList)
			{
				if (fc.Naziv == fitnesCentar.Naziv && fc.Obrisan==false)
				{
					return false;
				}
			}
			FitnesCentriList.Add(fitnesCentar);
			SacuvajFitnesCentre();
			var vlasnik = KorisniciBaza.KorisniciList.Find(item => item.KorisnickoIme == fitnesCentar.Vlasnik);
			vlasnik.FitnesCentriVlasnistvo.Add(fitnesCentar.Naziv);
			KorisniciBaza.SacuvajKorisnike();
			return true;
		}

		public static bool ObrisiFitnesCentar(string nazivFitnesCentra, string vlasnikKorisnickoIme)
		{
			UcitajFitnesCentre();
			if (!FitnesCentriList.Exists(item => item.Naziv == nazivFitnesCentra && item.Obrisan == false))
			{
				return false;
			}
			GrupniTreninziBaza.UcitajTreninge();
			foreach(var trening in GrupniTreninziBaza.GrupniTreninziList)
			{
				if(trening.Istekao == false && trening.FitnesCentar == nazivFitnesCentra)
				{
					return false;
				}
			}
			KorisniciBaza.UcitajKorisnike();
			foreach(var korisnik in KorisniciBaza.KorisniciList)
			{
				if(korisnik.FitnesCentarAngazovan == nazivFitnesCentra)
				{
					var trener = KorisniciBaza.KorisniciList.Find(item => item.KorisnickoIme == korisnik.KorisnickoIme);
					trener.Blokiran = true;
					KorisniciBaza.SacuvajKorisnike();
				}
			}
			var vlasnik = KorisniciBaza.KorisniciList.Find(item => item.KorisnickoIme == vlasnikKorisnickoIme);
			vlasnik.FitnesCentriVlasnistvo.Remove(nazivFitnesCentra);
			KorisniciBaza.SacuvajKorisnike();

			var fc = FitnesCentriList.Find(item => item.Naziv == nazivFitnesCentra);
			fc.Obrisan = true;
			SacuvajFitnesCentre();
			return true;
		}

		public static bool IzmeniFitnesCentar(FitnesCentar fitnesCentar)
		{
			UcitajFitnesCentre();

			if(!FitnesCentriList.Exists(item => item.Naziv == fitnesCentar.Naziv && item.Obrisan==false))
			{
				return false;
			}
			var fcentar = FitnesCentriList.Find(item => item.Naziv == fitnesCentar.Naziv && item.Obrisan == false);

			fcentar.Adresa = fitnesCentar.Adresa != null ? fitnesCentar.Adresa : fcentar.Adresa;
			fcentar.GodinaOtvaranja = fitnesCentar.GodinaOtvaranja != null ? fitnesCentar.GodinaOtvaranja : fcentar.GodinaOtvaranja;
			fcentar.MesecnaClanarina = fitnesCentar.MesecnaClanarina != 0 ? fitnesCentar.MesecnaClanarina : fcentar.MesecnaClanarina;
			fcentar.GodisnjaClanarina = fitnesCentar.GodisnjaClanarina != 0 ? fitnesCentar.GodisnjaClanarina : fcentar.GodisnjaClanarina;
			fcentar.CenaTreninga = fitnesCentar.CenaTreninga != 0 ? fitnesCentar.CenaTreninga : fcentar.CenaTreninga;
			fcentar.CenaGrupnogTreninga = fitnesCentar.CenaGrupnogTreninga != 0 ? fitnesCentar.CenaGrupnogTreninga : fcentar.CenaGrupnogTreninga;
			fcentar.CenaPersonalnogTreninga = fitnesCentar.CenaPersonalnogTreninga != 0 ? fitnesCentar.CenaPersonalnogTreninga : fcentar.CenaPersonalnogTreninga;

			SacuvajFitnesCentre();
			return true;

		}

		public static void UcitajFitnesCentre()
		{
			string putanja = "~/fitnescentri.json";
			putanja = HostingEnvironment.MapPath(putanja);

			if (!File.Exists(putanja))
			{
				File.Create(putanja);
			}

			string json = File.ReadAllText(putanja);

			FitnesCentriList = JsonConvert.DeserializeObject<List<FitnesCentar>>(json);
		}

		public static void SacuvajFitnesCentre()
		{
			string putanja = "~/fitnescentri.json";
			putanja = HostingEnvironment.MapPath(putanja);

			using (StreamWriter file = File.CreateText(putanja))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, FitnesCentriList);
			}
		}


	}
}