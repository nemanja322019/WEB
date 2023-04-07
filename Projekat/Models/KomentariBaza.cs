using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Projekat.Models
{
	public class KomentariBaza
	{
		public static List<Komentar> KomentariList { get; set; } = new List<Komentar>()
		{
			//new Komentar(){ Korisnik = "aaa",FitnesCentar="Fitnes Centar 3", Tekst="asdasdsad",Ocena = 3}
		};

		public static List<Komentar> SviKomentari(string vlasnik)
		{
			UcitajKomentare();
			List<Komentar> rezultat = new List<Komentar>();
			List<FitnesCentar> fitnescentri = FitnesCentriBaza.FitnesCentriList.Where(item => item.Vlasnik == vlasnik).ToList();
			foreach(var fc in fitnescentri)
			{
				foreach(var kom in KomentariList)
				{
					if(kom.FitnesCentar == fc.Naziv)
					{
						rezultat.Add(kom);
					}
				}
			}
			return rezultat;
		}

		public static bool OdobriKomentar(string id)
		{
			UcitajKomentare();
			if(!KomentariList.Exists(item => item.Id == id))
			{
				return false;
			}
			var komentar = KomentariList.Find(item => item.Id == id);
			if(komentar.Odobren == false)
			{
				komentar.Odobren = true;
			}
			else
			{
				komentar.Odobren = false;
			}
			SacuvajKomentare();
			return true;
		}

		public static bool DodajKomentar(Komentar komentar)
		{
			SacuvajKomentare();
			UcitajKomentare();

			GrupniTreninziBaza.UcitajTreninge();
			var listaTreninga = GrupniTreninziBaza.GrupniTreninziList.Where(item => item.FitnesCentar == komentar.FitnesCentar).ToList();
			bool posetio = false;
			foreach (var trening in listaTreninga)
			{
				if (trening.Istekao == true)
				{
					foreach (var pos in trening.SpisakPosetilaca)
					{
						if (pos == komentar.Korisnik)
						{
							posetio = true;
						}
					}
				}
			}
			if(!posetio)
			{
				return false;
			}
			KomentariList.Add(komentar);
			SacuvajKomentare();
			return true;

		}

		public static void UcitajKomentare()
		{
			string putanja = "~/komentari.json";
			putanja = HostingEnvironment.MapPath(putanja);

			if (!File.Exists(putanja))
			{
				File.Create(putanja);
			}

			string json = File.ReadAllText(putanja);

			KomentariList = JsonConvert.DeserializeObject<List<Komentar>>(json);
		}

		public static void SacuvajKomentare()
		{
			string putanja = "~/komentari.json";
			putanja = HostingEnvironment.MapPath(putanja);

			using (StreamWriter file = File.CreateText(putanja))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, KomentariList);
			}
		}


	}
}