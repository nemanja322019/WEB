using Newtonsoft.Json;
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
    public class TreningController : ApiController
    {
		public List<GrupniTrening> Get()
		{
			GrupniTreninziBaza.UcitajTreninge();
			return GrupniTreninziBaza.GrupniTreninziList;
		}

		public IHttpActionResult Put(GrupniTrening grupniTrening) 
		{

			if (GrupniTreninziBaza.IzmeniTrening(grupniTrening))
			{
				return Ok(grupniTrening);
			}
			return BadRequest();
		}

		public IHttpActionResult Post(GrupniTrening grupniTrening)
		{
			if (ProveraUnosa(grupniTrening))
			{
				if (GrupniTreninziBaza.DodajTrening(grupniTrening))
				{
					return Ok(grupniTrening);
				}
				
			}
			return BadRequest();
		}

		public IHttpActionResult Get(string TreningNaziv)
		{
			if (GrupniTreninziBaza.ObrisiTrening(TreningNaziv))
			{
				return Ok(TreningNaziv);
			}
			return BadRequest();
		}

		public IHttpActionResult Get(string treningNaziv, string korisnickoIme)
		{
			GrupniTreninziBaza.DodajKorisnikaNaTrening(treningNaziv, korisnickoIme);
			return Ok(treningNaziv);
		}

		[NonAction]
		public bool ProveraUnosa(GrupniTrening grupniTrening)
		{
			if (String.IsNullOrWhiteSpace(grupniTrening.Naziv) || String.IsNullOrWhiteSpace(grupniTrening.TipTreninga) || grupniTrening.Trajanje==0 || String.IsNullOrWhiteSpace(grupniTrening.Vreme) || grupniTrening.MaksimalanBrojPosetilaca==0 )
			{
				return false;
			}
			return true;
		}

	}
}
