using Newtonsoft.Json.Linq;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projekat.Controllers
{
    public class ProfilnaController : ApiController
    {
		public IHttpActionResult Get(string korisnickoIme,string nazivTreninga)
		{
			if(KorisniciBaza.DodajTreningTreneru(korisnickoIme, nazivTreninga))
			{
				return Ok(KorisniciBaza.KorisniciList.Find(item => item.KorisnickoIme == korisnickoIme));
			}
			return BadRequest();
		}

		public IHttpActionResult Post([FromBody]JObject jObject)
		{
			string staroKorisnickoIme = jObject.Value<string>("staroKorisnickoIme");
			Korisnik korisnik = new Korisnik();

			JToken temp = jObject["korisnik"];
			korisnik = temp.ToObject<Korisnik>();

			if (ProveraUnosa(korisnik))
			{
				if (KorisniciBaza.IzmeniKorisnika(staroKorisnickoIme, korisnik))
				{
					return Ok(korisnik);
				}
				else
				{
					return BadRequest("Greska pri izmeni!");
				}
			}
			else
			{
				return BadRequest("Greska pri unosu!");
			}
		}

		public IHttpActionResult Put([FromBody]JToken jToken)
		{
			string TreningNaziv = jToken.Value<string>("TreningNaziv");
			string KorisnickoIme = jToken.Value<string>("KorisnickoIme");
			if (KorisniciBaza.DodajTreningKorisniku(TreningNaziv, KorisnickoIme))
			{
				return Ok(TreningNaziv);
			}
			return BadRequest();
		}

		[NonAction]
		public bool ProveraUnosa(Korisnik korisnik)
		{
			if (String.IsNullOrWhiteSpace(korisnik.KorisnickoIme) || String.IsNullOrWhiteSpace(korisnik.Lozinka) || String.IsNullOrWhiteSpace(korisnik.Ime) || String.IsNullOrWhiteSpace(korisnik.Prezime) || String.IsNullOrWhiteSpace(korisnik.Pol) || String.IsNullOrWhiteSpace(korisnik.Email) || String.IsNullOrWhiteSpace(korisnik.DatumRodjenja))
			{
				return false;
			}
			return true;
		}
	}
}
