using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Projekat.Models;
using System.Web.Http.Results;

namespace Projekat.Controllers
{
    public class Fitnes_CentriController : ApiController
    {
		[HttpGet, Route("")]
		public RedirectResult Index()
		{
			var requestUri = Request.RequestUri;
			return Redirect(requestUri.AbsoluteUri + "index.html");
		}

		public List<FitnesCentar> Get()
		{
			return FitnesCentriBaza.SviFitnesCentri();
		}

		public FitnesCentar Get(string naziv)
		{
			return FitnesCentriBaza.FitnesCentriList.Find(item => item.Naziv == naziv);
		}

		public IHttpActionResult Get(string FitnesCentarNaziv, string vlasnik)
		{
			if (FitnesCentriBaza.ObrisiFitnesCentar(FitnesCentarNaziv, vlasnik))
			{
				return Ok(FitnesCentarNaziv);
			}
			return BadRequest();
		}

		public List<FitnesCentar> Get(string naziv, string adresa, string godinaotvaranja)
		{
			List<FitnesCentar> fitnesCentriList = new List<FitnesCentar>();

			fitnesCentriList = FitnesCentriBaza.FitnesCentriList.Where(c => (naziv == null || c.Naziv == naziv) &&
											   (adresa == null || c.Adresa == adresa) &&
											   (godinaotvaranja == null || c.GodinaOtvaranja == godinaotvaranja)).ToList();

			return fitnesCentriList;
		}

		public IHttpActionResult Post(FitnesCentar fitnesCentar)
		{

			if (ProveraUnosa(fitnesCentar))
			{
				if (FitnesCentriBaza.DodajFitnesCentar(fitnesCentar))
				{
					return Ok(fitnesCentar);
				}
				else
				{ return BadRequest("Greska pri dodavanju."); }
			}
			return BadRequest("Polja nisu dobro popunjena!");
		}

		public IHttpActionResult Put(FitnesCentar fitnesCentar)
		{

			if (FitnesCentriBaza.IzmeniFitnesCentar(fitnesCentar))
			{
				return Ok(fitnesCentar);
			}
			return BadRequest();

		}

		[NonAction]
		public bool ProveraUnosa(FitnesCentar fitnesCentar)
		{
			if (String.IsNullOrWhiteSpace(fitnesCentar.Naziv) || String.IsNullOrWhiteSpace(fitnesCentar.Adresa) || String.IsNullOrWhiteSpace(fitnesCentar.GodinaOtvaranja) || String.IsNullOrWhiteSpace(fitnesCentar.Vlasnik) || fitnesCentar.MesecnaClanarina == 0 || fitnesCentar.GodisnjaClanarina == 0 || fitnesCentar.CenaTreninga == 0 || fitnesCentar.CenaGrupnogTreninga == 0 || fitnesCentar.CenaPersonalnogTreninga == 0)
			{
				return false;
			}
			return true;
		}


	}
}
