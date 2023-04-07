using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Projekat.Controllers
{
    public class KomentariController : ApiController
    {

		public IHttpActionResult Get(string vlasnik)
		{
			return Ok(KomentariBaza.SviKomentari(vlasnik));	
		}



		public IHttpActionResult Post(Komentar komentar)
		{
			if (ProveraUnosa(komentar))
			{
				if (KomentariBaza.DodajKomentar(komentar))
				{
					return Ok(komentar);
				}
				else
				{ return BadRequest(); }
			}
			return BadRequest();
		}

		public IHttpActionResult Get(string id,string placeholder)
		{

			if (KomentariBaza.OdobriKomentar(id))
			{
				return Ok(id);
			}

			return BadRequest(); 
		}

		[NonAction]
		public bool ProveraUnosa(Komentar komentar)
		{
			if (String.IsNullOrWhiteSpace(komentar.Korisnik) || String.IsNullOrWhiteSpace(komentar.FitnesCentar) || String.IsNullOrWhiteSpace(komentar.Tekst))
			{
				return false;
			}
			return true;
		}

	}
}
