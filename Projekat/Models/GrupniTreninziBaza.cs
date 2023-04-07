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
	public class GrupniTreninziBaza
	{
		public static List<GrupniTrening> GrupniTreninziList { get; set; } = new List<GrupniTrening>() {
			//new GrupniTrening() { Naziv="Trening 1", TipTreninga =TipTreninga.BodyPump.ToString(),FitnesCentar = FitnesCentriBaza.FitnesCentriList[1],Trajanje=30,Vreme = new DateTime(2022, 7, 17, 20, 30, 0).ToString("dd/MM/yyyy HH:mm"),MaksimalanBrojPosetilaca = 5 },
			//new GrupniTrening() { Naziv="Trening 2", TipTreninga =TipTreninga.Yoga.ToString(),FitnesCentar = FitnesCentriBaza.FitnesCentriList[1],Trajanje=60,Vreme = new DateTime(2022, 7, 18, 20, 30, 0).ToString("dd/MM/yyyy HH:mm"),MaksimalanBrojPosetilaca = 5 }
		};

		public static bool DodajTrening(GrupniTrening grupniTrening)
		{
			UcitajTreninge();

			if (GrupniTreninziList.Exists(item => item.Naziv == grupniTrening.Naziv))
			{
				return false;
			}

			if (DateTime.TryParseExact(grupniTrening.Vreme, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime dt))
			{
				if ((dt - DateTime.Now).TotalDays >= 3 && Enum.IsDefined(typeof(TipTreninga), grupniTrening.TipTreninga))
				{
					GrupniTreninziList.Add(grupniTrening);
					SacuvajTreninge();
					return true;
				}
			}
			return false;
		}

		public static bool ObrisiTrening(string nazivTreninga)
		{
			UcitajTreninge();
			var trening = GrupniTreninziList.Find(item => item.Naziv == nazivTreninga);
			if(trening.SpisakPosetilaca.Count == 0)
			{
				trening.Istekao = true;
				SacuvajTreninge();
				return true;
			}
			return false;
		}

		public static bool IzmeniTrening( GrupniTrening grupniTrening)
		{
			UcitajTreninge();
			
			if(grupniTrening.Naziv == null || !GrupniTreninziList.Exists(item => item.Naziv == grupniTrening.Naziv) || grupniTrening.Istekao==true)
			{
				return false;
			}
			var trening = GrupniTreninziList.Find(item => item.Naziv == grupniTrening.Naziv);

			if(grupniTrening.TipTreninga!=null && Enum.IsDefined(typeof(TipTreninga),grupniTrening.TipTreninga))
			{
				trening.TipTreninga = grupniTrening.TipTreninga;
			}
			
			trening.Trajanje = grupniTrening.Trajanje != 0 ? grupniTrening.Trajanje : trening.Trajanje;
			if (grupniTrening.Vreme != null)
			{
				if (DateTime.TryParseExact(grupniTrening.Vreme, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
				{
					if ((dt - DateTime.Now).TotalDays >= 3)
					{
						trening.Vreme = grupniTrening.Vreme;
					}
					else
					{
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			trening.MaksimalanBrojPosetilaca = grupniTrening.MaksimalanBrojPosetilaca != 0 ? grupniTrening.MaksimalanBrojPosetilaca : trening.MaksimalanBrojPosetilaca;

			SacuvajTreninge();
			return true;
		}


		public static void DodajKorisnikaNaTrening(string treningNaziv,string korisnickoIme)
		{
			UcitajTreninge();

			GrupniTreninziList[GrupniTreninziList.FindIndex(item => item.Naziv == treningNaziv)].SpisakPosetilaca.Add(korisnickoIme);
			SacuvajTreninge();
		}

		public static void UcitajTreninge()
		{
			string putanja = "~/treninzi.json";
			putanja = HostingEnvironment.MapPath(putanja);

			if (!File.Exists(putanja))
			{
				File.Create(putanja);
			}

			string json = File.ReadAllText(putanja);

			GrupniTreninziList = JsonConvert.DeserializeObject<List<GrupniTrening>>(json);
			
		}

		public static void SacuvajTreninge()
		{
			string putanja = "~/treninzi.json";
			putanja = HostingEnvironment.MapPath(putanja);

			using (StreamWriter file = File.CreateText(putanja))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, GrupniTreninziList);
			}
		}

	}
}