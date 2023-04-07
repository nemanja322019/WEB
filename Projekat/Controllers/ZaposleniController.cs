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
    public class ZaposleniController : ApiController
    {
		public IHttpActionResult GetKorisnici()
		{
			return Ok(KorisniciBaza.KorisniciList);
		}

		public IHttpActionResult GetBlokiraj(string KorisnickoIme)
		{
			if (KorisniciBaza.BlokirajTrenera(KorisnickoIme))
			{
				return Ok(KorisnickoIme);
			}
			return BadRequest();
		}

		public IHttpActionResult GetZaposli(string KorisnickoIme,string NazivFitnesCentra)
		{
			if(KorisniciBaza.ZaposliTrenera(KorisnickoIme,NazivFitnesCentra))
			{
				return Ok(KorisnickoIme);
			}
			return BadRequest();
		}

		public IHttpActionResult PutUkloniTreningTreneru([FromBody]JObject jObject)
		{
			string KorisnickoIme = jObject.Value<string>("KorisnickoIme");
			string TreningNaziv = jObject.Value<string>("TreningNaziv");

			KorisniciBaza.UcitajKorisnike();
			KorisniciBaza.KorisniciList[KorisniciBaza.KorisniciList.FindIndex(item => item.KorisnickoIme == KorisnickoIme)].AngazovaniTreninzi.Remove(TreningNaziv);
			KorisniciBaza.SacuvajKorisnike();
			return Ok(KorisnickoIme);
		}

	}
}
